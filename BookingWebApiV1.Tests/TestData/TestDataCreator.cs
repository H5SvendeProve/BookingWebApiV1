using System.Text.Json;
using BookingWebApiV1.Api.RequestDTOs;
using BookingWebApiV1.Models.DatabaseDTOs;

namespace BookingWebApiV1.Tests.TestData;

public static class TestDataCreator
{
    public static CreateNewUserRequest GetTestUserRequest()
    {
        return new CreateNewUserRequest
        {
            Username = "tester",
            Password = "password",
            DepartmentName = "test"
        };
    }
    
    

    public static DepartmentDTO GetTestDepartmentDTO()
    {
        return new DepartmentDTO
        {
            DepartmentName = "test",
            Address = "test vej 123",
            Zipcode = "4100",
            City = "testen"
        };
    }

    public static CreateNewUserRequest GetTestCreateUserRequest()
    {
        return new CreateNewUserRequest
        {
            DepartmentName = "test",
            Password = "password",
            Username = "tester",
            UserRole = 0
        };
    }

    public static MachineDTO GetTestMachine()
    {
        return new MachineDTO
        {
            MachineManufacturer = "Tester",
            ModelName = "Test model",
            MachineType = "Vaskemaskine",
            EffectKWh = 1
        };
    }

    public static MachineProgramDTO GetTestMachineProgram()
    {
        return new MachineProgramDTO
        {
            MachineManufacturer = "Tester",
            ModelName = "Test model",
            ProgramId = 1
        };
    }

    public static ProgramDTO GetTestWashingMachineProgram()
    {
        return new ProgramDTO
        {
            ProgramId = 1,
            ProgramName = "Koge vask",
            ProgramRunTimeMinutes = 180
        };
    }
    
    public static CreateNewBookingRequest GetTestBookingRequest()
    {
        return new CreateNewBookingRequest
        {
            MachineManufacturer = "Tester",
            ModelName = "Test model",
            ProgramId = 1,
            Username = "tester"
        };
    }

    public static List<ElectricityPriceDTO> GetTestPrices()
    {
        string westDenmarkJson =
            "[{\"DKK_per_kWh\":0.3608,\"EUR_per_kWh\":0.04845,\"EXR\":7.446844,\"time_start\":\"2023-03-17T00:00:00+01:00\",\"time_end\":\"2023-03-17T01:00:00+01:00\"},{\"DKK_per_kWh\":0.37212,\"EUR_per_kWh\":0.04997,\"EXR\":7.446844,\"time_start\":\"2023-03-17T01:00:00+01:00\",\"time_end\":\"2023-03-17T02:00:00+01:00\"},{\"DKK_per_kWh\":0.37279,\"EUR_per_kWh\":0.05006,\"EXR\":7.446844,\"time_start\":\"2023-03-17T02:00:00+01:00\",\"time_end\":\"2023-03-17T03:00:00+01:00\"},{\"DKK_per_kWh\":0.44629,\"EUR_per_kWh\":0.05993,\"EXR\":7.446844,\"time_start\":\"2023-03-17T03:00:00+01:00\",\"time_end\":\"2023-03-17T04:00:00+01:00\"},{\"DKK_per_kWh\":0.46759,\"EUR_per_kWh\":0.06279,\"EXR\":7.446844,\"time_start\":\"2023-03-17T04:00:00+01:00\",\"time_end\":\"2023-03-17T05:00:00+01:00\"},{\"DKK_per_kWh\":0.60759,\"EUR_per_kWh\":0.08159,\"EXR\":7.446844,\"time_start\":\"2023-03-17T05:00:00+01:00\",\"time_end\":\"2023-03-17T06:00:00+01:00\"},{\"DKK_per_kWh\":0.68377,\"EUR_per_kWh\":0.09182,\"EXR\":7.446844,\"time_start\":\"2023-03-17T06:00:00+01:00\",\"time_end\":\"2023-03-17T07:00:00+01:00\"},{\"DKK_per_kWh\":0.69874,\"EUR_per_kWh\":0.09383,\"EXR\":7.446844,\"time_start\":\"2023-03-17T07:00:00+01:00\",\"time_end\":\"2023-03-17T08:00:00+01:00\"},{\"DKK_per_kWh\":0.71668,\"EUR_per_kWh\":0.09624,\"EXR\":7.446844,\"time_start\":\"2023-03-17T08:00:00+01:00\",\"time_end\":\"2023-03-17T09:00:00+01:00\"},{\"DKK_per_kWh\":0.67238,\"EUR_per_kWh\":0.09029,\"EXR\":7.446844,\"time_start\":\"2023-03-17T09:00:00+01:00\",\"time_end\":\"2023-03-17T10:00:00+01:00\"},{\"DKK_per_kWh\":0.62583,\"EUR_per_kWh\":0.08404,\"EXR\":7.446844,\"time_start\":\"2023-03-17T10:00:00+01:00\",\"time_end\":\"2023-03-17T11:00:00+01:00\"},{\"DKK_per_kWh\":0.59538,\"EUR_per_kWh\":0.07995,\"EXR\":7.446844,\"time_start\":\"2023-03-17T11:00:00+01:00\",\"time_end\":\"2023-03-17T12:00:00+01:00\"},{\"DKK_per_kWh\":0.56417,\"EUR_per_kWh\":0.07576,\"EXR\":7.446844,\"time_start\":\"2023-03-17T12:00:00+01:00\",\"time_end\":\"2023-03-17T13:00:00+01:00\"},{\"DKK_per_kWh\":0.55144,\"EUR_per_kWh\":0.07405,\"EXR\":7.446844,\"time_start\":\"2023-03-17T13:00:00+01:00\",\"time_end\":\"2023-03-17T14:00:00+01:00\"},{\"DKK_per_kWh\":0.65569,\"EUR_per_kWh\":0.08805,\"EXR\":7.446844,\"time_start\":\"2023-03-17T14:00:00+01:00\",\"time_end\":\"2023-03-17T15:00:00+01:00\"},{\"DKK_per_kWh\":0.72354,\"EUR_per_kWh\":0.09716,\"EXR\":7.446844,\"time_start\":\"2023-03-17T15:00:00+01:00\",\"time_end\":\"2023-03-17T16:00:00+01:00\"},{\"DKK_per_kWh\":0.81573,\"EUR_per_kWh\":0.10954,\"EXR\":7.446844,\"time_start\":\"2023-03-17T16:00:00+01:00\",\"time_end\":\"2023-03-17T17:00:00+01:00\"},{\"DKK_per_kWh\":0.7878,\"EUR_per_kWh\":0.10579,\"EXR\":7.446844,\"time_start\":\"2023-03-17T17:00:00+01:00\",\"time_end\":\"2023-03-17T18:00:00+01:00\"},{\"DKK_per_kWh\":0.79987,\"EUR_per_kWh\":0.10741,\"EXR\":7.446844,\"time_start\":\"2023-03-17T18:00:00+01:00\",\"time_end\":\"2023-03-17T19:00:00+01:00\"},{\"DKK_per_kWh\":0.78088,\"EUR_per_kWh\":0.10486,\"EXR\":7.446844,\"time_start\":\"2023-03-17T19:00:00+01:00\",\"time_end\":\"2023-03-17T20:00:00+01:00\"},{\"DKK_per_kWh\":0.75675,\"EUR_per_kWh\":0.10162,\"EXR\":7.446844,\"time_start\":\"2023-03-17T20:00:00+01:00\",\"time_end\":\"2023-03-17T21:00:00+01:00\"},{\"DKK_per_kWh\":0.74722,\"EUR_per_kWh\":0.10034,\"EXR\":7.446844,\"time_start\":\"2023-03-17T21:00:00+01:00\",\"time_end\":\"2023-03-17T22:00:00+01:00\"},{\"DKK_per_kWh\":0.74737,\"EUR_per_kWh\":0.10036,\"EXR\":7.446844,\"time_start\":\"2023-03-17T22:00:00+01:00\",\"time_end\":\"2023-03-17T23:00:00+01:00\"},{\"DKK_per_kWh\":0.74245,\"EUR_per_kWh\":0.0997,\"EXR\":7.446844,\"time_start\":\"2023-03-17T23:00:00+01:00\",\"time_end\":\"2023-03-18T00:00:00+01:00\"}]";
        string eastDenmarkJson =
            "[{\"DKK_per_kWh\":0.25252,\"EUR_per_kWh\":0.03391,\"EXR\":7.446844,\"time_start\":\"2023-03-17T00:00:00+01:00\",\"time_end\":\"2023-03-17T01:00:00+01:00\"},{\"DKK_per_kWh\":0.24292,\"EUR_per_kWh\":0.03262,\"EXR\":7.446844,\"time_start\":\"2023-03-17T01:00:00+01:00\",\"time_end\":\"2023-03-17T02:00:00+01:00\"},{\"DKK_per_kWh\":0.2351,\"EUR_per_kWh\":0.03157,\"EXR\":7.446844,\"time_start\":\"2023-03-17T02:00:00+01:00\",\"time_end\":\"2023-03-17T03:00:00+01:00\"},{\"DKK_per_kWh\":0.22333,\"EUR_per_kWh\":0.02999,\"EXR\":7.446844,\"time_start\":\"2023-03-17T03:00:00+01:00\",\"time_end\":\"2023-03-17T04:00:00+01:00\"},{\"DKK_per_kWh\":0.22005,\"EUR_per_kWh\":0.02955,\"EXR\":7.446844,\"time_start\":\"2023-03-17T04:00:00+01:00\",\"time_end\":\"2023-03-17T05:00:00+01:00\"},{\"DKK_per_kWh\":0.2208,\"EUR_per_kWh\":0.02965,\"EXR\":7.446844,\"time_start\":\"2023-03-17T05:00:00+01:00\",\"time_end\":\"2023-03-17T06:00:00+01:00\"},{\"DKK_per_kWh\":0.23391,\"EUR_per_kWh\":0.03141,\"EXR\":7.446844,\"time_start\":\"2023-03-17T06:00:00+01:00\",\"time_end\":\"2023-03-17T07:00:00+01:00\"},{\"DKK_per_kWh\":0.24791,\"EUR_per_kWh\":0.03329,\"EXR\":7.446844,\"time_start\":\"2023-03-17T07:00:00+01:00\",\"time_end\":\"2023-03-17T08:00:00+01:00\"},{\"DKK_per_kWh\":0.24992,\"EUR_per_kWh\":0.03356,\"EXR\":7.446844,\"time_start\":\"2023-03-17T08:00:00+01:00\",\"time_end\":\"2023-03-17T09:00:00+01:00\"},{\"DKK_per_kWh\":0.24433,\"EUR_per_kWh\":0.03281,\"EXR\":7.446844,\"time_start\":\"2023-03-17T09:00:00+01:00\",\"time_end\":\"2023-03-17T10:00:00+01:00\"},{\"DKK_per_kWh\":0.2447,\"EUR_per_kWh\":0.03286,\"EXR\":7.446844,\"time_start\":\"2023-03-17T10:00:00+01:00\",\"time_end\":\"2023-03-17T11:00:00+01:00\"},{\"DKK_per_kWh\":0.24247,\"EUR_per_kWh\":0.03256,\"EXR\":7.446844,\"time_start\":\"2023-03-17T11:00:00+01:00\",\"time_end\":\"2023-03-17T12:00:00+01:00\"},{\"DKK_per_kWh\":0.2421,\"EUR_per_kWh\":0.03251,\"EXR\":7.446844,\"time_start\":\"2023-03-17T12:00:00+01:00\",\"time_end\":\"2023-03-17T13:00:00+01:00\"},{\"DKK_per_kWh\":0.24053,\"EUR_per_kWh\":0.0323,\"EXR\":7.446844,\"time_start\":\"2023-03-17T13:00:00+01:00\",\"time_end\":\"2023-03-17T14:00:00+01:00\"},{\"DKK_per_kWh\":0.24083,\"EUR_per_kWh\":0.03234,\"EXR\":7.446844,\"time_start\":\"2023-03-17T14:00:00+01:00\",\"time_end\":\"2023-03-17T15:00:00+01:00\"},{\"DKK_per_kWh\":0.22445,\"EUR_per_kWh\":0.03014,\"EXR\":7.446844,\"time_start\":\"2023-03-17T15:00:00+01:00\",\"time_end\":\"2023-03-17T16:00:00+01:00\"},{\"DKK_per_kWh\":0.33511,\"EUR_per_kWh\":0.045,\"EXR\":7.446844,\"time_start\":\"2023-03-17T16:00:00+01:00\",\"time_end\":\"2023-03-17T17:00:00+01:00\"},{\"DKK_per_kWh\":0.60818,\"EUR_per_kWh\":0.08167,\"EXR\":7.446844,\"time_start\":\"2023-03-17T17:00:00+01:00\",\"time_end\":\"2023-03-17T18:00:00+01:00\"},{\"DKK_per_kWh\":0.60833,\"EUR_per_kWh\":0.08169,\"EXR\":7.446844,\"time_start\":\"2023-03-17T18:00:00+01:00\",\"time_end\":\"2023-03-17T19:00:00+01:00\"},{\"DKK_per_kWh\":0.60803,\"EUR_per_kWh\":0.08165,\"EXR\":7.446844,\"time_start\":\"2023-03-17T19:00:00+01:00\",\"time_end\":\"2023-03-17T20:00:00+01:00\"},{\"DKK_per_kWh\":0.33518,\"EUR_per_kWh\":0.04501,\"EXR\":7.446844,\"time_start\":\"2023-03-17T20:00:00+01:00\",\"time_end\":\"2023-03-17T21:00:00+01:00\"},{\"DKK_per_kWh\":0.18647,\"EUR_per_kWh\":0.02504,\"EXR\":7.446844,\"time_start\":\"2023-03-17T21:00:00+01:00\",\"time_end\":\"2023-03-17T22:00:00+01:00\"},{\"DKK_per_kWh\":0.18625,\"EUR_per_kWh\":0.02501,\"EXR\":7.446844,\"time_start\":\"2023-03-17T22:00:00+01:00\",\"time_end\":\"2023-03-17T23:00:00+01:00\"},{\"DKK_per_kWh\":0.14209,\"EUR_per_kWh\":0.01908,\"EXR\":7.446844,\"time_start\":\"2023-03-17T23:00:00+01:00\",\"time_end\":\"2023-03-18T00:00:00+01:00\"}]";

        var westDenmarkPrices = JsonSerializer.Deserialize<List<ElectricityPriceDTO>>(westDenmarkJson);

        var eastDenmarkPrices = JsonSerializer.Deserialize<List<ElectricityPriceDTO>>(eastDenmarkJson);

        var prices = new List<ElectricityPriceDTO>();

        var dateNow = DateTime.Now;

        if (westDenmarkPrices != null)
            foreach (var westPrice in westDenmarkPrices)
            {
                westPrice.Location = "WestDenmark";
                
                if (westPrice.TimeStart.Date >= dateNow.Date) continue;

                var newDate = new DateTime(westPrice.TimeStart.Year, westPrice.TimeStart.Month, dateNow.Day,
                    westPrice.TimeStart.Hour, westPrice.TimeStart.Minute, westPrice.TimeStart.Second);

                westPrice.TimeStart = newDate;
                
            }

        if (eastDenmarkPrices != null)
        {
            foreach (var price in eastDenmarkPrices)
            {
                price.Location = "EastDenmark";
                
                if (price.TimeStart.Date >= dateNow.Date) continue;

                var newDate = new DateTime(price.TimeStart.Year, price.TimeStart.Month, dateNow.Day,
                    price.TimeStart.Hour, price.TimeStart.Minute, price.TimeStart.Second);

                price.TimeStart = newDate;

            }

        }

        if (westDenmarkPrices != null)
            prices.AddRange(westDenmarkPrices);
        if (eastDenmarkPrices != null)
            prices.AddRange(eastDenmarkPrices);

        return prices;
    }

    public static RfidCardDTO GetTestRfidCard()
    {
        return new RfidCardDTO
        {
            Username = "tester",
            RfidCardId = "2031720534"
        };
    }
    
    
    public static BookingDTO GetTestBooking()
    {
        return new BookingDTO
        {
            Username = "tester",
            ModelName = "Test model",
            ProgramId = 1,
            MachineManufacturer = "Tester",
            Price = 10,
            StartTime = new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, DateTime.Now.Date.Day, 18, 0, 0),
            EndTime =   new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, DateTime.Now.Date.Day, 21, 0, 0),
        };
    }
    
}

