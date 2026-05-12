using System;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.EngineerAgg;

namespace Coreapi.Domain.AggregatesModel.ElectProjectAgg;

public class CheckListForm
{
    private CheckListForm(){}


    public CheckListForm(string solarChecked, DateTime julianChecked, InspectionDesEnum inspectionDesEnum, bool? isComplete, string resultDes, ElectProject electProject, Engineer engineer)
    {
        Id = Guid.NewGuid();
        SolarChecked = solarChecked;
        JulianChecked = julianChecked;
        InspectionDesEnum = inspectionDesEnum;
        IsComplete = isComplete;
        ResultDes = resultDes;
        ElectProject = electProject;
        Engineer = engineer;
    }

    public Guid Id { get; set; }
    public string SolarChecked { get; set; }
    public DateTime JulianChecked { get; set; }
    public InspectionDesEnum InspectionDesEnum { get; set; }
    public bool? IsComplete { get; set; }
    public string ResultDes { get; set; }

    public ElectProject ElectProject { get; set; }
    public Engineer Engineer { get; set; }

    public void UpdateComment(string solarChecked, DateTime julianChecked, InspectionDesEnum inspectionDesEnum, bool? isComplete, string resultDes)
    {
        SolarChecked = solarChecked;
        JulianChecked = julianChecked;
        InspectionDesEnum = inspectionDesEnum;
        IsComplete = isComplete;
        ResultDes = resultDes;
    }
}