namespace BookingWebApiV1.Models.DatabaseDTOs;

public class DepartmentElectricityPricesDTO
{
    public string DepartmentName { get; set; }
    public float Exr { get; set; }
    public DateTime TimeStart { get; set; }
    public DateTime TimeEnd { get; set; }
    public string Location { get; set; }
}

