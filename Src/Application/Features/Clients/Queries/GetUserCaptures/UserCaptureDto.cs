using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;

namespace Coreapi.Application.Features.Clients.Queries.GetUserCaptures;

public class UserCaptureDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Avatar { get; set; }
    public string Created { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public Guid RefId { get; set; }
    public CaptureRefrenceEnum RefType { get; set; }
    public CaptureEventEnum EventType { get; set; }
    public CaptureStatusEnum Status { get; set; }

}
