using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using System;

namespace Coreapi.Domain.AggregatesModel.ElectProjectAgg;

public class CheckListEdc
{
    private CheckListEdc(){}


    public CheckListEdc(string solarChecked, DateTime julianChecked, CheckListEdcEnum checkListEdcEnum, bool? isComplete, string resultDes, ElectProject electProject)
    {
        Id = Guid.NewGuid();
        SolarChecked = solarChecked;
        JulianChecked = julianChecked;
        CheckListEdcEnum = checkListEdcEnum;
        IsComplete = isComplete;
        ResultDes = resultDes;
        ElectProject = electProject;
    }

    public Guid Id { get; set; }
    public string SolarChecked { get; set; }
    public DateTime JulianChecked { get; set; }
    public CheckListEdcEnum CheckListEdcEnum { get; set; }
    public bool? IsComplete { get; set; }
    public string ResultDes { get; set; }

    public ElectProject ElectProject { get; set; }

    public void UpdateComment(string solarChecked, DateTime julianChecked, CheckListEdcEnum checkListEdcEnum, bool? isComplete, string resultDes)
    {
        SolarChecked = solarChecked;
        JulianChecked = julianChecked;
        CheckListEdcEnum = checkListEdcEnum;
        IsComplete = isComplete;
        ResultDes = resultDes;
    }

}