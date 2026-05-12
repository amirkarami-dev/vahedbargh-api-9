using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Amazon.S3.Model;
using System.IO;

namespace Coreapi.Application.Common.Interfaces;

public interface IS3Service
{
    Task<Stream> GetFullPath(string path);
    Task<string> UploadFileAttach(MemoryStream memoryStream, string key);
    Task<string> UploadFileAttach(IFormFile file, string fileName, string folder, string folder2 = "");
    Task<byte[]> GetFile(string path);
    Task<byte[]> GetFileAttach(string path);
    Task DeleteFile(string path);
    Task DeleteFile(string folderName, string containName);
    Task DeleteDuplicateFiles(string folderName, string fileName);
}