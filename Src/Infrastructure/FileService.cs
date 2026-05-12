using jsreport.Client;
using jsreport.Types;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Application.Common.Models;

namespace Coreapi.Infrastructure
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly IConfiguration configuration;

        public FileService(IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.configuration = configuration;
        }

        public void DeleteFile(string path)
        {
            string webRootPath = hostingEnvironment.ContentRootPath;

            string imageFile = path;
            if (File.Exists(
                    Path.Combine(webRootPath, imageFile)))
                File.Delete(Path.Combine(webRootPath, imageFile));
        }

        public void DeleteFile(string folderName,string containName)
        {

            var webRootPath = hostingEnvironment.ContentRootPath;
            if(!Directory.Exists(Path.Combine(webRootPath, folderName))) return;
            var files = Directory.GetFiles(Path.Combine(webRootPath, folderName));

            foreach (var file in files)
            {
                if (file.ToUpper().Contains(containName.ToUpper()))
                {
                    File.Delete(file);
                }
            }
        }

        public Task<IEnumerable<DocFileData>> GetFilesInFolder(string folderName, string folder2)
        {
            var newList = new List<DocFileData>();
            var webRootPath = hostingEnvironment.ContentRootPath;
            var realFolder = folderName;
            if(!string.IsNullOrEmpty(folder2) && folder2!="0") realFolder = Path.Combine(folderName,folder2);
            if (!Directory.Exists(Path.Combine(webRootPath, "Upload", "DocProjects", realFolder))) return Task.FromResult<IEnumerable<DocFileData>>(newList);
            var files = Directory.GetFiles(Path.Combine(webRootPath, "Upload", "DocProjects", realFolder));


            newList.AddRange(
                files.Select(file => new DocFileData
                {
                    FileName = Path.GetFileName(file),
                    FolderName = folderName,
                    Code = Convert.ToInt64(folderName)

                })
            );
            return Task.FromResult<IEnumerable<DocFileData>>(newList);
        }


        public async Task<string> UploadFileAttach(IFormFile file, string folder, string folder2)
        {
            try
            {
                if (file is not null)
                {
                    string result = null;
                    string folderName = Path.Combine("Upload", folder);
                    if (!string.IsNullOrEmpty(folder2))
                        folderName = Path.Combine(folderName, folder2);

                    string webRootPath = hostingEnvironment.ContentRootPath;
                    string newPath = Path.Combine(webRootPath, folderName);
                    if (!Directory.Exists(newPath))
                    {
                        Directory.CreateDirectory(newPath);
                    }
                    if (file.Length > 0)
                    {
                        string dt = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Split('.').ToList().Last().Trim('"');
                        //string name = Convert.ToBase64String(Encoding.UTF8.GetBytes(file.Name));
                        string fileName = file.FileName;
                        string fullPath = Path.Combine(newPath, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        result = fileName;
                        return "Upload/" + folder + "/" + (!string.IsNullOrEmpty(folder2) ? (folder2 + "/") : "") + result + "." + dt;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<string> UploadFileAttach(IFormFile file, string fileName, string folder, string folder2 = "")
        {
            try
            {
                if (file is not null)
                {
                    string result = null;
                    string folderName = Path.Combine("Upload", folder);
                    if (!string.IsNullOrEmpty(folder2))
                        folderName = Path.Combine(folderName, folder2);

                    string webRootPath = hostingEnvironment.ContentRootPath;
                    string newPath = Path.Combine(webRootPath, folderName);
                    if (!Directory.Exists(newPath))
                    {
                        Directory.CreateDirectory(newPath);
                    }
                    if (file.Length > 0)
                    {
                        string dt = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Split('.').ToList().Last().Trim('"');
                        //string name = Convert.ToBase64String(Encoding.UTF8.GetBytes(file.Name));
                        string fullPath = Path.Combine(newPath, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        result = fileName;
                        return "Upload/" + folder + "/" + (!string.IsNullOrEmpty(folder2) ? (folder2 + "/") : "") + result + "." + dt;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<string> UploadFileAttach(IFormFile file, string fileName, string folder, string folder2 = "", string folder3 = "")
        {
            try
            {
                if (file is not null)
                {
                    string result = null;
                    string folderName = Path.Combine("Upload", folder);
                    if (!string.IsNullOrEmpty(folder2))
                        folderName = Path.Combine(folderName, folder2);
                    if (!string.IsNullOrEmpty(folder2))
                        folderName = Path.Combine(folderName, folder3);

                    string webRootPath = hostingEnvironment.ContentRootPath;
                    string newPath = Path.Combine(webRootPath, folderName);
                    if (!Directory.Exists(newPath))
                    {
                        Directory.CreateDirectory(newPath);
                    }
                    if (file.Length > 0)
                    {
                        string dt = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Split('.').ToList().Last().Trim('"');
                        //string name = Convert.ToBase64String(Encoding.UTF8.GetBytes(file.Name));
                        string fullPath = Path.Combine(newPath, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        result = fileName;
                        return "Upload/" + folder + "/" + (!string.IsNullOrEmpty(folder2) ? (folder2 + "/") : "") + result + "." + dt;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<byte[]> GetFile(string path)
        {
            string webRootPath = hostingEnvironment.ContentRootPath;

            string imageFile = path;
            if (File.Exists(Path.Combine(webRootPath, imageFile)))
            {
                var filePath = Path.Combine(webRootPath, imageFile);
                using var file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                file.Close();
                var bytes = ms.ToArray();
                ms.Close();
                return bytes;
            }

            return null;
        }

        public async Task<byte[]> GetFileAttach(string path)
        {
            var webRootPath = hostingEnvironment.ContentRootPath;
          
            if (!File.Exists(Path.Combine(webRootPath, path))) return null;
            var filePath = Path.Combine(webRootPath, path);
            await using var file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            await using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            file.Close();
            var bytes = ms.ToArray();
            ms.Close();
            return bytes;

        }

        public string GetFullPath(string path)
        {
            string webRootPath = hostingEnvironment.ContentRootPath;
            if (File.Exists(Path.Combine(webRootPath, path)))
                return Path.Combine(webRootPath, path);

            return null;
        }

        public async Task<string> UploadAvatar(IFormFile file)
        {
            try
            {
                if (file is not null)
                {
                    string result = null;
                    string folderName = Path.Combine("Upload", "Avatars");
                    string webRootPath = hostingEnvironment.ContentRootPath;
                    string newPath = Path.Combine(webRootPath, folderName);
                    if (!Directory.Exists(newPath))
                    {
                        Directory.CreateDirectory(newPath);
                    }
                    if (file.Length > 0)
                    {
                        DateTime dt = DateTime.Now;
                        string name = dt.Year.ToString() + dt.Month.ToString() + dt.Day.ToString() + dt.Hour.ToString() + dt.Minute.ToString() + dt.Second.ToString();
                        string fileName = name + "Avatars" + "." + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Split('.').ToList().Last().Trim('"');
                        string fullPath = Path.Combine(newPath, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        result = fileName;
                    }
                    return "Upload/Avatars/" + result;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<string> UploadFile(IFormFile file, string folder, string folder2)
        {
            try
            {
                if (file is not null)
                {
                    string result = null;
                    string folderName = Path.Combine("Upload", folder);
                    if (!string.IsNullOrEmpty(folder2))
                        folderName = Path.Combine(folderName, folder2);

                    string webRootPath = hostingEnvironment.ContentRootPath;
                    string newPath = Path.Combine(webRootPath, folderName);
                    if (!Directory.Exists(newPath))
                    {
                        Directory.CreateDirectory(newPath);
                    }
                    if (file.Length > 0)
                    {
                        DateTime dt = DateTime.Now;
                        string name = dt.Year.ToString() + dt.Month.ToString() + dt.Day.ToString() + dt.Hour.ToString() + dt.Minute.ToString() + dt.Second.ToString();
                        string fileName = name + "Avatars" + "." + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Split('.').ToList().Last().Trim('"');
                        string fullPath = Path.Combine(newPath, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        result = fileName;
                        return "Upload/" + folder + "/" + (!string.IsNullOrEmpty(folder2) ? (folder2 + "/") : "") + result;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<byte[]> ExportToPDF(string content)
        {
            try
            {
                var rs = new ReportingService(configuration["JsReportUrl"]);
                var report = await rs.RenderAsync(new RenderRequest()
                {
                    Template = new Template()
                    {
                        Recipe = Recipe.ChromePdf,
                        Engine = Engine.JsRender,
                        Content = content,
                    }
                });

                using var ms = new MemoryStream();
                await report.Content.CopyToAsync(ms);
                report.Content.Close();
                var bytes = ms.ToArray();
                ms.Close();
                //string folderName = Path.Combine("Upload", "temp");
                //string webRootPath = hostingEnvironment.ContentRootPath;
                //string newPath = Path.Combine(webRootPath, folderName);
                //if (!Directory.Exists(newPath))
                //{
                //    Directory.CreateDirectory(newPath);
                //}
                //string fullPath = Path.Combine(newPath, "hello.pdf");

                //File.WriteAllBytes(fullPath, bytes);
                return bytes;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
