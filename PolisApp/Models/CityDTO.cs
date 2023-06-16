
namespace PolisApp.Models;

public class CityDTO
{
    public int Id { get; set; }
    public string Datetime { get; set; }
    public string Name { get; set; }
    public string Summary { get; set; }
    public string Url { get; set; }
    public string Type { get; set; }
    public Location Location { get; set; }
}
