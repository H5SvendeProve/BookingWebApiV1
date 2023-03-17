using System.Text.Json.Serialization;

namespace BookingWebApiV1.Models.DatabaseDTOs;

public class ElectricityPriceDTO
{
    [JsonPropertyName("DKK_per_kWh")]
    public double DKKPerKWh { get; set; }
    [JsonPropertyName("EUR_per_kWh")]
    public double EURPerKWh { get; set; }
    [JsonPropertyName("EXR")]
    public double Exr { get; set; }
    [JsonPropertyName("time_start")]
    public DateTime TimeStart { get; set; }
    [JsonPropertyName("time_end")]
    public DateTime TimeEnd { get; set; }
    [JsonIgnore]
    public string Location { get; set; }
}