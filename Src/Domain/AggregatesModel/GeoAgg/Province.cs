namespace Coreapi.Domain.AggregatesModel.GeoAgg;

public class Province
{
    private Province(){}


    public Province(int idProvince, string provinceName, string provinceCenter)
    {
        Id = idProvince;
        ProvinceName = provinceName;
        ProvinceCenter = provinceCenter;
    }

    public int Id { get; set; }
    public string ProvinceName { get; set; }
    public string ProvinceCenter { get; set; }

}