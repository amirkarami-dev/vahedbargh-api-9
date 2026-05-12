using Coreapi.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.User.Commands.AddUserFile
{
    public class UserFileOutput
    {
        public Guid Id { get; set; }
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
