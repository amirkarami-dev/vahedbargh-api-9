using System.ComponentModel.DataAnnotations;
using Coreapi.Common.Enums;

namespace Coreapi.Application.Common.Models
{
    public class PayViewModel
    {
        [Display(Name = "Tracking number")]
        public long TrackingNumber { get; set; }

        [Display(Name = "Generate the Tracking number automatically?")]
        public bool GenerateTrackingNumberAutomatically { get; set; } = true;

        public decimal Amount { get; set; }

        [Display(Name = "Gateway")]
        public GatewayTypeEnum SelectedGateway { get; set; } = GatewayTypeEnum.Melli;
    }
}
