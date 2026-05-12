using Coreapi.Domain.AggregatesModel.ClientAggregate;
using System;

namespace Coreapi.Domain.AggregatesModel.PanelMakerAgg;

public class PanelMaker
{
    private PanelMaker(){}

    public PanelMaker(Client client,string userId, string naCode, string fullName, string mobileNumber, bool isActive, string companyName, string companyCode, string licenseNumber, string signatureFileName, string provinceName, string cityName, string address, int idSection, string moreInfo, string tel)
    {
        Id = Guid.NewGuid();
        Client = client;
        NaCode = naCode;
        FullName = fullName;
        MobileNumber = mobileNumber;
        IsActive = isActive;
        CompanyName = companyName;
        CompanyCode = companyCode;
        LicenseNumber = licenseNumber;
        SignatureFileName = signatureFileName;
        ProvinceName = provinceName;
        CityName = cityName;
        Address = address;
        IdSection = idSection;
        MoreInfo = moreInfo;
        Tel = tel;
        UserId = userId;
    }

    public Guid Id { get; set; }
    public Client Client { get; init; }
    public string UserId { get; set; }

    public string NaCode { get; private set; }
    public string FullName { get; private set; }
    public string MobileNumber { get; private set; }
    public string Tel { get; private set; }

    public bool IsActive { get; private set; }
    public string CompanyName { get; private set; }
    public string CompanyCode { get; private set; }
    public string LicenseNumber { get; private set; }
    public string SignatureFileName { get; private set; }
    public string ProvinceName { get; private set; }
    public string CityName { get; private set; }
    public int IdSection { get; private set; }
    public string MoreInfo { get; private set; }

    public string Address { get; private set; }

}