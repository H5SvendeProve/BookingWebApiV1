using System.ComponentModel.DataAnnotations;

namespace BookingWebApiV1.Api.Validation;

public class TimeValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        var dateTime = (DateTime)value;
        return dateTime > DateTime.Now;
    }
}