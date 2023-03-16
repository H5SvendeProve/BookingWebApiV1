using BookingWebApiV1.Api.RequestDTOs;
using BookingWebApiV1.Models.DatabaseDTOs;

namespace BookingWebApiV1.Tests.TestData;

public static class TestDataCreator
{
    public static CreateNewUserRequest GetTestUserRequest()
    {
        return new CreateNewUserRequest
        {
            Username = "testUser",
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
            Username = "testUser",
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

    public static DepartmentElectricityPricesDTO GetTestDepartmentElectricityPrice()
    {
        return new DepartmentElectricityPricesDTO
        {
            DepartmentName = "test",
            Location = "WestDenmark",
            TimeStart = DateTime.Now,
            TimeEnd = DateTime.Now.AddHours(1),
            Exr = (float)7.44
        };
    }

    public static ElectricityPriceDTO GetTestElectricityPrice()
    {
        return new ElectricityPriceDTO
        {
            Location = "WestDenmark",
            DKKPerKWh = 0.312,
            EURPerKWh = 0.042,
            Exr = (float)7.44,
            TimeStart = DateTime.Now,
            TimeEnd = DateTime.Now.AddHours(1)
        };
    }

    public static CreateNewBookingRequest GetTestBookingRequest()
    {
        return new CreateNewBookingRequest
        {
            MachineManufacturer = "Tester",
            ModelName = "Test model",
            ProgramId = 1,
            Username = "testUser"
        };
    }
}

