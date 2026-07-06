using Amazon.S3;
using Amazon.S3.Model;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Coreapi.Infrastructure;

public class S3ServiceArvan : IS3ServicePublic
{
    private static readonly HttpClient _httpClient = new();

    private readonly string _bucketName;
    private readonly AmazonS3Client _client;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public S3ServiceArvan(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
    {
        var endpoint = configuration["ServiceS3:ENDPOINT"];
        _bucketName = configuration["ServiceS3:BUCKET_NAME"];
        var accessKey = configuration["ServiceS3:ACCESS_KEY"];
        var secretKey = configuration["ServiceS3:SECRET_KEY"];
        _hostingEnvironment = hostingEnvironment;

        // Extract auth region from endpoint hostname:
        // "hot.ir-central1.arvanstorage.ir" → "ir-central1"
        var hostParts = new Uri(endpoint).Host.Split('.');
        var authRegion = hostParts.Length > 1 ? hostParts[1] : "us-east-1";

        var config = new AmazonS3Config
        {
            ServiceURL = endpoint,
            ForcePathStyle = true,
            SignatureVersion = "4",
            AuthenticationRegion = authRegion,
        };
        _client = new AmazonS3Client(new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey), config);
    }

    // Arvan returns non-hex ETags the AWS SDK cannot parse and may send 301 redirects when the
    // signing region is wrong. All downloads bypass SDK response parsing via presigned URL + HttpClient.
    public async Task<Stream> GetFullPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new NotFoundException("مسیر فایل نامعتبر است");
        return await DownloadViaPresignedUrlAsync(path);
    }

    public async Task<byte[]> GetFile(string path)
    {
        var ms = (MemoryStream)await DownloadViaPresignedUrlAsync(path);
        return ms.GetBuffer()[..(int)ms.Length];
    }

    public async Task<byte[]> GetFileAttach(string path) => await GetFile(path);

    public string GetLocalPath(string path)
    {
        var fullPath = Path.Combine(_hostingEnvironment.ContentRootPath, path);
        return File.Exists(fullPath) ? fullPath : null;
    }

    private async Task<Stream> DownloadViaPresignedUrlAsync(string path)
    {
        var url = _client.GetPreSignedURL(new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = path,
            Expires = DateTime.UtcNow.AddMinutes(5),
            Verb = HttpVerb.GET
        });

        using var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new NotFoundException($"فایل یافت نشد: {path}");
        response.EnsureSuccessStatusCode();

        var stream = new MemoryStream();
        await response.Content.CopyToAsync(stream);
        stream.Position = 0;
        return stream;
    }

    public async Task<string> UploadFileAttach(IFormFile file, string fileName, string folder, string folder2 = "")
    {
        var folderPath = "Upload/" + folder;
        if (!string.IsNullOrEmpty(folder2))
            folderPath += "/" + folder2;
        var key = folderPath + "/" + fileName;

        try
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms).ConfigureAwait(false);
            if (ms.CanSeek) ms.Position = 0;

            await _client.PutObjectAsync(new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = ms,
                AutoCloseStream = false,
                ContentType = file.ContentType ?? "application/octet-stream"
            });
            return key;
        }
        catch (AmazonS3Exception e)
        {
            throw new NotFoundException($"خطا در S3: {e.Message}");
        }
    }

    public async Task<string> UploadFileAttach(MemoryStream memoryStream, string key)
    {
        try
        {
            await _client.PutObjectAsync(new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = memoryStream,
            });
        }
        catch (AmazonS3Exception e)
        {
            throw new NotFoundException($"خطا در S3: {e.Message}");
        }
        return key;
    }

    public async Task DeleteFile(string path)
    {
        try
        {
            await _client.DeleteObjectAsync(new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = path,
            });
        }
        catch (AmazonS3Exception e)
        {
            throw new NotFoundException($"Error: {e.Message}");
        }
    }

    public async Task DeleteFile(string folderName, string containName)
    {
        var searchKey = folderName + "/" + containName;
        var listResponse = await _client.ListObjectsV2Async(new ListObjectsV2Request
        {
            BucketName = _bucketName,
            Prefix = folderName + "/"
        });

        foreach (var obj in listResponse.S3Objects)
        {
            if (!obj.Key.Contains(searchKey)) continue;
            try
            {
                await _client.DeleteObjectAsync(new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = obj.Key
                });
            }
            catch (AmazonS3Exception e)
            {
                throw new NotFoundException($"Error: {e.Message}");
            }
        }
    }

    public async Task DeleteDuplicateFiles(string folderName, string fileName)
    {
        var nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
        var typePrefix = new string(nameWithoutExt.TakeWhile(char.IsLetter).ToArray());
        if (string.IsNullOrEmpty(typePrefix)) return;

        var fileToKeep = folderName + "/" + fileName;
        var listResponse = await _client.ListObjectsV2Async(new ListObjectsV2Request
        {
            BucketName = _bucketName,
            Prefix = folderName + "/" + typePrefix
        });

        foreach (var obj in listResponse.S3Objects)
        {
            if (obj.Key == fileToKeep) continue;
            try
            {
                await _client.DeleteObjectAsync(new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = obj.Key
                });
            }
            catch (AmazonS3Exception e)
            {
                throw new NotFoundException($"Error: {e.Message}");
            }
        }
    }
}
