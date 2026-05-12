using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Coreapi.Application.Common.Interfaces
{
    public interface IFileService
    {
        string GetFullPath(string path);
        Task<string> UploadAvatar(IFormFile file);
        Task<string> UploadFile(IFormFile file, string folder, string folder2 = "");
        Task<string> UploadFileAttach(IFormFile file, string folder, string folder2 = "");
        Task<string> UploadFileAttach(IFormFile file,string fileName, string folder, string folder2 = "");
        Task<byte[]> GetFile(string path);
        Task<byte[]> GetFileAttach(string path);
        Task<byte[]> ExportToPDF(string content);
        void DeleteFile(string path);
        void DeleteFile(string folderName, string containName);
    }
}
