namespace BookingWebApiV1.Models.DatabaseResultDTOs;

public class BookingElectricityPriceDTO
{
    public string DepartmentName { get; set; }
    public decimal DKKPerKWh { get; set; }
    public DateTime TimeStart { get; set; }
    public DateTime TimeEnd { get; set; }
    public string Location { get; set; }
}