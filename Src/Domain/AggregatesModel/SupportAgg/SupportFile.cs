using Coreapi.Common.Enums;
using System;

namespace Coreapi.Domain.AggregatesModel.SupportAgg;

public class SupportFile
{
    private SupportFile(){}


    public SupportFile(string name, string des, FileTypeEnum fileTypeEnum, string folderName, string fileName, string userId, string toUserId, Support support)
    {
        Id = Guid.NewGuid();
        Name = name;
        Des = des;
        FileTypeEnum = fileTypeEnum;
        FolderName = folderName;
        FileName = fileName;
        UserId = userId;
        ToUserId = toUserId;
        Support = support;
    }

    public Guid Id { get; init; }
    public string Name { get; private set; }
    public string Des { get; private set; }
    public FileTypeEnum FileTypeEnum { get; private set; }
    public string FolderName { get; private set; }
    public string FileName { get; private set; }
    public string UserId { get; private set; }
    public string ToUserId { get; private set; }

    public Support Support { get; set; }
}