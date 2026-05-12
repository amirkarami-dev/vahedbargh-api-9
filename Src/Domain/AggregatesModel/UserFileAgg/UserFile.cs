using Coreapi.Common.Enums;
using System;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Domain.AggregatesModel.UserFileAgg
{
    public class UserFile
    {
        private UserFile(){}

        public UserFile(string name, string des, string title, UserFileTypeEnum userFileTypeEnum, string folderName, string fileName, string userId, string toUserId, Client client)
        {
            Id = Guid.NewGuid();
            Name = name;
            Des = des;
            Title = title;
            UserFileTypeEnum = userFileTypeEnum;
            FolderName = folderName;
            FileName = fileName;
            UserId = userId;
            ToUserId = toUserId;
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }
        public Guid Id { get; init; }
        public string Name { get; private set; }
        public string Title { get; private set; }
        public string Des { get; private set; }
        public UserFileTypeEnum UserFileTypeEnum { get; private set; }
        public string FolderName { get; private set; }
        public string FileName { get; private set; }
        public string UserId { get; private set; }
        public string ToUserId { get; private set; }
        public Client Client { get; private set; }
    }
}
