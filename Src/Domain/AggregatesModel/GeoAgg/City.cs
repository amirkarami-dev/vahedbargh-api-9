namespace Coreapi.Domain.AggregatesModel.GeoAgg;

public class City
{
    private City(){}

    public int Id { get; set; }
    public string CityName { get; set; }
    public float Longitude { get; set; }
    public float Latitude { get; set; }
    public Province Province { get; set; }
}