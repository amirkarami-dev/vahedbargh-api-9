using System;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.EngineerAgg;

namespace Coreapi.Domain.AggregatesModel.ElectProjectAgg;

public class CommentEngForm
{
    private CommentEngForm(){}
    public CommentEngForm(BranchingTypeEnum branchingTypeEnum,FazNumberEnum fazNumberEnum, int branchingCount, float ampere, float power, float powerSum, string des, ElectProject electProject, Engineer engineer)
    {
        Id = Guid.NewGuid();
        BranchingTypeEnum = branchingTypeEnum;
        FazNumberEnum = fazNumberEnum;
        BranchingCount = branchingCount;
        Ampere = ampere;
        Power = power;
        PowerSum = powerSum;
        Des = des;
        ElectProject = electProject;
        Engineer = engineer;
    }

    public Guid Id { get; set; }
    public BranchingTypeEnum BranchingTypeEnum { get; set; }
    public FazNumberEnum FazNumberEnum { get; set; }
    public int BranchingCount { get; set; }
    public float Ampere { get; set; }
    public float Power { get; set; }
    public float PowerSum { get; set; }
    public string Des { get; set; }
    public ElectProject ElectProject { get; set; }
    public Engineer Engineer { get; set; }

    public void UpdateComment(BranchingTypeEnum requestBranchingTypeEnum,FazNumberEnum fazNumberEnum, int requestBranchingCount, float requestAmpere, float requestPower, float requestPowerSum, string requestDes)
    {
        BranchingTypeEnum = requestBranchingTypeEnum;
        FazNumberEnum = fazNumberEnum;
        BranchingCount = requestBranchingCount;
        Ampere = requestAmpere;
        Power = requestPower;
        PowerSum = requestPowerSum;
        Des = requestDes;

    }
}