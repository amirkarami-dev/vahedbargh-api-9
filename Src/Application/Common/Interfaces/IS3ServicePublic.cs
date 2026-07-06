using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Coreapi.Application.Common.Interfaces;

public interface IS3ServicePublic
{
    Task<Stream> GetFullPath(string path);
    string GetLocalPath(string path);
    Task<string> UploadFileAttach(IFormFile file, string fileName, string folder, string folder2 = "");
    Task<string> UploadFileAttach(MemoryStream memoryStream, string key);
    Task<byte[]> GetFile(string path);
    Task<byte[]> GetFileAttach(string path);
    Task DeleteFile(string path);
    Task DeleteFile(string folderName, string containName);
    Task DeleteDuplicateFiles(string folderName, string fileName);
}
