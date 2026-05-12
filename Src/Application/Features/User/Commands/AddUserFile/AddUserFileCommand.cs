using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Coreapi.Application.Features.User.Commands.AddUserFile
{
    public class AddUserFileCommand:IRequest<int>
    {
        public IFormFile File { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Des { get; set; }
        public UserFileTypeEnum UserFileTypeEnum { get; set; }
        public string FolderName { get; set; }
        public string FileName { get; set; }
        public string UserId { get; set; }
        public string ToUserId { get; set; }
    }
}
