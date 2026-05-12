using System.Collections.Generic;

namespace Coreapi.Common.ViewModels;

public class ElectProjectViewMainModel
{
    public IEnumerable<ElectProjectViewModel> ElectProjectViewModel { get; set; }
    public int CountRow { get; set; }
}