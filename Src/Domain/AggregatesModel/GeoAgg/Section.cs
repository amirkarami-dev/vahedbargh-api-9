namespace Coreapi.Domain.AggregatesModel.GeoAgg;

public class Section
{
    private Section(){}

    public int Id { get; set; }
    public string SectionName { get; set; }
    public City City { get; set; }
}