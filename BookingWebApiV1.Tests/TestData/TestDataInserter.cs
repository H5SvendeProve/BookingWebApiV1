using System.Runtime.CompilerServices;
using BookingWebApiV1.Database;
using BookingWebApiV1.Models.DatabaseDTOs;
using BookingWebApiV1.Services.LoginService;

namespace BookingWebApiV1.Tests.TestData;

public static class TestDataInserter
{

    public static async Task InsertTestMachine(IDatabaseContext databaseContext)
    {
        var testMachine = TestDataCreator.GetTestMachine();

        await databaseContext.InsertNewMachine(testMachine);
    }

    public static  async Task InsertTestMachineProgram(IDatabaseContext databaseContext)
    {
        var testMachineProgram = TestDataCreator.GetTestMachineProgram();

        await databaseContext.InsertMachineProgram(testMachineProgram);
    }

    public static async Task InsertTestDepartment(IDatabaseContext databaseContext)
    {
        var newDepartment = TestDataCreator.GetTestDepartmentDTO();

        await databaseContext.InsertNewDepartment(newDepartment);
    }

    public static async Task InsertTestProgram(IDatabaseContext databaseContext)
    {
        var testProgram = TestDataCreator.GetTestWashingMachineProgram();
        await databaseContext.InsertProgram(testProgram);
    }

    public static async Task InsertAvailableBookingTimes(IDatabaseContext databaseContext)
    {
        await databaseContext.InsertAvailableBookingTimes();
    }

    public static async Task InsertElectricityPrices(IDatabaseContext databaseContext)
    {
        var testPrices = TestDataCreator.GetTestPrices();
        
        foreach (var price in testPrices)
        {
            await databaseContext.InsertElectricityPrice(price);
        }
    }

    public static async Task DeleteElectricityPrice(ElectricityPriceDTO electricityPriceDTO, IDatabaseContext databaseContext)
    {
        if (electricityPriceDTO != null)
        {
            await databaseContext.DeleteElectricityPrice(electricityPriceDTO);
        }
    }
    
    public static async Task InsertNewDepartmentTestData(IDatabaseContext databaseContext)
    {
        var newDepartment = TestDataCreator.GetTestDepartmentDTO();

        await databaseContext.InsertNewDepartment(newDepartment);
    }

    public static async Task DeleteTestUser(IDatabaseContext databaseContext, UserDTO userDTO)
    {
        await databaseContext.DeleteUser(userDTO);
    }

    public static async Task InsertTestUser(ILoginService loginService)
    {
        var testUserRequest = TestDataCreator.GetTestUserRequest();

        await loginService.RegisterNewUser(testUserRequest);
    }
    

    public static async Task DeleteElectricityPrice(IDatabaseContext databaseContext,ElectricityPriceDTO electricityPriceDTO)
    {
        if (electricityPriceDTO != null)
        {
            await databaseContext.DeleteElectricityPrice(electricityPriceDTO);
        }
    }

    public static async Task InsertTestRfidCard(IDatabaseContext databaseContext)
    {
        var rfidCardTest = TestDataCreator.GetTestRfidCard();

        await databaseContext.InsertNewRfidCard(rfidCardTest);
    }

    public static async Task<BookingDTO> InsertTestBooking(IDatabaseContext databaseContext, BookingDTO bookingDTO)
    {
        return await databaseContext.InsertNewBooking(bookingDTO);
    }

    public static async Task UpdateAvailableBookingTimes(IDatabaseContext databaseContext, AvailableBookingTimeDTO availableBookingTimeDTO)
    {
        await databaseContext.UpdateAvailableBookingToTaken(availableBookingTimeDTO);
    }

    public static async Task<List<AvailableBookingTimeDTO>> GetAvailableBookingTimes(IDatabaseContext databaseContext)
    {
        var testUser = TestDataCreator.GetTestUserRequest();
        return await databaseContext.GetAvailableBookingTimesInDepartment(testUser.Username);
    }

    public static async Task ResetAvailableBookingTime(IDatabaseContext databaseContext, AvailableBookingTimeDTO availableBookingTimeDTO)
    {
        await databaseContext.ResetAvailableBookingTime(availableBookingTimeDTO);
    }

    public static async Task UpdateAllBookingTimesToBeAvailableInTestDepartment(IDatabaseContext databaseContext)
    {
        await databaseContext.UpdateAllBookingTimesToBeAvailableInDepartment(TestDataCreator.GetTestDepartmentDTO()
            .DepartmentName);
    }
}