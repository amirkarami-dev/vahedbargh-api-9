using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums
{
    public enum StaffRangeEnum
    {
        [Display(Name = "Between 1 to 5")]
        BetweenOneToFive = 1,
        [Display(Name = "Between 5 to 15")]
        BetweenFiveToFifteen = 2,
        [Display(Name = "Between 15 to 20")]
        BetweenFifteenToTwenty = 3,
        [Display(Name = "Between 50 to 100")]
        BetweenFiftyToOneHundred = 4
    }
}
