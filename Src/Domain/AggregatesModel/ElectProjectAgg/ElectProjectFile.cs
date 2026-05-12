using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;

namespace Coreapi.Domain.AggregatesModel.ElectProjectAgg
{
    public class ElectProjectFile
    {
        private ElectProjectFile() { }

        public ElectProjectFile(string name, string des, FileTypeEnum fileTypeEnum, string folderName, string fileName, string userId, string toUserId, ElectProject electProject)
        {
            Id = Guid.NewGuid();
            Name = name;
            Des = des;
            FileTypeEnum = fileTypeEnum;
            FolderName = folderName;
            FileName = fileName;
            UserId = userId;
            ToUserId = toUserId;
            ElectProject = electProject;
        }

        public Guid Id { get; init; }
        public string Name { get; private set; }
        public string Des { get; private set; }
        public FileTypeEnum FileTypeEnum { get; private set; }
        public string FolderName { get; private set; }
        public string FileName { get; private set; }
        public string UserId { get; private set; }
        public string ToUserId { get; private set; }
        public ElectProject ElectProject { get; private set; }

        public void UpdateProjectFile(string fileName, ElectProject electProject)
        {
            FileName = fileName;
            ElectProject = electProject;
        }

    }
}
