using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using Coreapi.Application.Common.Exceptions;

namespace Coreapi.Infrastructure;

public class S3Service:IS3Service
{
    private readonly string _endPoint;
    private readonly string _bucketName;
    private readonly string _accessKey;
    private readonly string _secretKey;
    private readonly AmazonS3Client _client;

    public S3Service(IConfiguration configuration)
    {

        _endPoint = configuration["ConfigS3:ENDPOINT"];
        _bucketName = configuration["ConfigS3:BUCKET_NAME"];
        _accessKey = configuration["ConfigS3:ACCESS_KEY"];
        _secretKey = configuration["ConfigS3:SECRET_KEY"];
        // making s3 connections
        var config = new AmazonS3Config
        {
            ServiceURL = _endPoint,
            ForcePathStyle = true,
            SignatureVersion = "4"
        };
        var credentials = new Amazon.Runtime.BasicAWSCredentials(_accessKey, _secretKey);
        _client = new AmazonS3Client(credentials, config);

    }

    public async Task<Stream> GetFullPath(string path)
    {

        var urlRequest = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = path,
        };
        using var getObjectResponse = await _client.GetObjectAsync(urlRequest);
        await using var responseStream = getObjectResponse.ResponseStream;
        var stream = new MemoryStream();
        await responseStream.CopyToAsync(stream);
        stream.Position = 0;
        return stream;
    }

    public async Task<string> UploadFileAttach(IFormFile file, string fileName, string folder, string folder2 = "")
    {
        var folderName = "Upload/" + folder;
        if (!string.IsNullOrEmpty(folder2))
            folderName = folderName + "/" + folder2;
        var fullPath = folderName + "/" + fileName;
        try
        {

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream).ConfigureAwait(false);


            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = fullPath,
                InputStream = memoryStream,
            };
            // uploading image in bucket
            await _client.PutObjectAsync(request);
            return fullPath;
        }

        catch (AmazonS3Exception e)
        {
            throw new NotFoundException($"Error: {e.Message}");
        }
        return null;
    }

    public async Task<string> UploadFileAttach(MemoryStream memoryStream, string key)
    {

        try
        {
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = memoryStream,
            };
            // uploading image in bucket
            await _client.PutObjectAsync(request);
        }

        catch (AmazonS3Exception e)
        {
            throw new NotFoundException($"Error: {e.Message}");
        }
        return null;
    }

    public async Task<byte[]> GetFile(string path)
    {
        var urlRequest = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = path,
        };
        try
        {
            using var getObjectResponse = await _client.GetObjectAsync(urlRequest);
            await using var responseStream = getObjectResponse.ResponseStream;
            var stream = new MemoryStream();
            await responseStream.CopyToAsync(stream);
            stream.Position = 0;
            return stream.GetBuffer()[..(int)stream.Length];
        }
        catch (Exception e)
        {
            return null;
        }

    }

    public async Task<byte[]> GetFileAttach(string path)
    {
        var urlRequest = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = path,
        };
        using var getObjectResponse = await _client.GetObjectAsync(urlRequest);
        await using var responseStream = getObjectResponse.ResponseStream;
        var stream = new MemoryStream();
        await responseStream.CopyToAsync(stream);
        stream.Position = 0;
        return stream.GetBuffer()[..(int)stream.Length];
    }

    public async Task DeleteFile(string path)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = path,
        };
        try
        {
            await _client.DeleteObjectAsync(request);

        }
        catch (AmazonS3Exception e)
        {
            throw new NotFoundException($"Error: {e.Message}");

        }
    }

	public async Task DeleteFile(string folderName, string containName)
	{
		var searchKey = folderName + "/" + containName;
		var listObjectsRequest = new ListObjectsV2Request
		{
			BucketName = _bucketName,
			Prefix = searchKey
		};
		var listResponse = await _client.ListObjectsV2Async(listObjectsRequest);

		// Delete objects
		foreach (var obj in listResponse.S3Objects)
		{

			if (obj.Key.Contains(searchKey))
			{
				var deleteObjectRequest = new DeleteObjectRequest
				{
					BucketName = _bucketName,
					Key = obj.Key
				};

				try
				{
					await _client.DeleteObjectAsync(deleteObjectRequest);

				}
				catch (AmazonS3Exception e)
				{
					throw new NotFoundException($"Error: {e.Message}");

				}
			}
		}
	}

	public async Task DeleteDuplicateFiles(string folderName, string fileName)
	{
		// Extract the letters-only type prefix (e.g. "RelatedPermit" from "RelatedPermit14030230-55.jpg")
		var nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
		var typePrefix = new string(nameWithoutExt.TakeWhile(char.IsLetter).ToArray());

		if (string.IsNullOrEmpty(typePrefix))
			return;

		var searchKey = folderName + "/" + typePrefix;
		var fileToKeep = folderName + "/" + fileName;

		var listObjectsRequest = new ListObjectsV2Request
		{
			BucketName = _bucketName,
			Prefix = searchKey
		};
		var listResponse = await _client.ListObjectsV2Async(listObjectsRequest);

		foreach (var obj in listResponse.S3Objects)
		{
			if (obj.Key == fileToKeep)
				continue;

			var deleteObjectRequest = new DeleteObjectRequest
			{
				BucketName = _bucketName,
				Key = obj.Key
			};

			try
			{
				await _client.DeleteObjectAsync(deleteObjectRequest);
			}
			catch (AmazonS3Exception e)
			{
				throw new NotFoundException($"Error: {e.Message}");
			}
		}
	}
}