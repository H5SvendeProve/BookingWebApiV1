namespace BookingWebApiV1.Models.DatabaseDTOs;

public class ElectricityPriceDTO
{
    public double DKKPerKWh { get; set; }

    public double EURPerKWh { get; set; }

    public double Exr { get; set; }
 
    public DateTime TimeStart { get; init; }

    public DateTime TimeEnd { get; set; }

    public string Location { get; set; }
    
}