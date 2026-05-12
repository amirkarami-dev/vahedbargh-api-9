using Coreapi.Common.Enums;

namespace Coreapi.Common.RequestModel;

public class ElectProjectFilterModel
{
    public ElectProjectFilterModel(string searchValue, long fileNumber, string solarRegisterDate, int idSection, ProjectLevelEnum projectLevelEnum, string exeName, string landLordName, bool isStop)
    {
        SearchValue = searchValue;
        FileNumber = fileNumber;
        SolarRegisterDate = solarRegisterDate;
        IdSection = idSection;
        ProjectLevelEnum = projectLevelEnum;
        ExeName = exeName;
        LandLordName = landLordName;
        IsStop = isStop;
    }
    public string SearchValue { get; set; }
    public long FileNumber { get; set; }
    public string SolarRegisterDate { get; set; }
    public int IdSection { get; set; }
    public ProjectLevelEnum ProjectLevelEnum { get; set; }
    public string ExeName { get; set; }
    public string LandLordName { get; set; }
    public bool IsStop { get; set; }
}