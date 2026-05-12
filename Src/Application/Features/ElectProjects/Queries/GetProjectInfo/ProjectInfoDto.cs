using System;

namespace Coreapi.Application.Features.ElectProjects.Queries.GetProjectInfo;

public class ProjectInfoDto
{
    public Guid Id { get; set; }
    public string Address { get; set; }
    public string PostalCode { get; set; }
    public string LandlordName { get; set; }
    public long AmountPay { get; set; }
    public string NaCode { get; set; }
    public long FileNumber { get; set; }
    public long ElectRequestNumber { get; set; }
}