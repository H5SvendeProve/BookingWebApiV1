using System.Reflection;
using BookingWebApiV1.Api.Mapper;
using BookingWebApiV1.Api.RequestDTOs;
using BookingWebApiV1.Database;
using BookingWebApiV1.Models.DatabaseResultDTOs;
using BookingWebApiV1.Services.WashingMachineService;
using BookingWebApiV1.Tests.TestData;
using Microsoft.IdentityModel.Tokens;
using ArgumentException = BookingWebApiV1.Exceptions.ArgumentException;

namespace BookingWebApiV1.Tests.Service.WashingMachineService;

public class WashingMachineServiceTests
{
    private IWashingMachineService WashingMachineService { get; }
    private IDatabaseContext DatabaseContext { get; }
    private IRequestMapper RequestMapper { get; }

    public WashingMachineServiceTests()
    {
        DatabaseContext = new DatabaseContext(Constants.Constant.testDbConStringMkServer);
        RequestMapper = new RequestMapper();
        WashingMachineService = new Services.WashingMachineService.WashingMachineService(DatabaseContext, RequestMapper);
    }

    [Fact]
    public async Task GetBookingConnectedToRfid_should_return_bookingDTO()
    {
        // Arrange
        var testBooking = TestDataCreator.GetTestBooking();
        // take away 5 minutes from startTime to be able to scan the booking
        
        await TestDataInserter.InsertAvailableBookingTimes(DatabaseContext);

        await TestDataInserter.UpdateAllBookingTimesToBeAvailableInTestDepartment(DatabaseContext);

        var availableBookingTimes = await TestDataInserter.GetAvailableBookingTimes(DatabaseContext);
        
        var firstAvailableTime = availableBookingTimes.First();

        testBooking.StartTime = firstAvailableTime.StartTime;

        testBooking.EndTime = firstAvailableTime.EndTime;

        var scannedTime = firstAvailableTime.StartTime.AddMinutes(5);
        
        await TestDataInserter.InsertTestRfidCard(DatabaseContext);
        var testRfidCard = TestDataCreator.GetTestRfidCard();

        var bookingCreated = await TestDataInserter.InsertTestBooking(DatabaseContext, testBooking);

        firstAvailableTime.bookingId = bookingCreated.BookingId;

        await TestDataInserter.UpdateAvailableBookingTimes(DatabaseContext, firstAvailableTime);

        var actual = await WashingMachineService.GetBookingConnectedToRfid(testRfidCard.RfidCardId, scannedTime);
        
        // Assert
        Assert.True(actual.BookingId > 1);
        Assert.True(actual.BookingId  == bookingCreated.BookingId);
    }
    
    [Fact]
    public async Task GetBookingProgram_should_return_ProgramResultDTO()
    {
        // Arrange
        var testBooking = TestDataCreator.GetTestBooking();
        var testBookingCreated = await TestDataInserter.InsertTestBooking(DatabaseContext, testBooking);
        
        // Actual
        var actual = await WashingMachineService.GetBookingProgram(testBookingCreated);
        
        // Assert
        Assert.True(testBookingCreated.MachineManufacturer == actual.MachineManufacturer);
        Assert.True(testBookingCreated.ModelName == actual.ModelName);
        Assert.True(!actual.Equals(default(ProgramResultDTO)));
    }
    
    [Fact]
    public async Task RfidCardExists_should_return_true()
    {
        // Arrange
        var rfidTest = TestDataCreator.GetTestRfidCard();
        
        await TestDataInserter.InsertTestRfidCard(DatabaseContext);
        
        // Actual
        var actual = await WashingMachineService.RfidCardExists(rfidTest.RfidCardId);

        // Assert
        Assert.True(actual);
    }
    
    [Fact]
    public async Task RfidCardExists_rfid_not_in_database_should_return_false()
    {
        // Arrange
        var rfidTest = TestDataCreator.GetTestRfidCard();
        rfidTest.RfidCardId = "does not exist";
        
        await TestDataInserter.InsertTestRfidCard(DatabaseContext);
        
        // Actual
        var actual = await WashingMachineService.RfidCardExists(rfidTest.RfidCardId);

        // Assert
        Assert.False(actual);
    }
    
    [Fact]
    public async Task RfidCardExists_rfidCardId_is_empty_should_throw_argumentException()
    {
        // Arrange
        var rfidTest = TestDataCreator.GetTestRfidCard();
        rfidTest.RfidCardId = "";
        
        await TestDataInserter.InsertTestRfidCard(DatabaseContext);
        
        // Actual
        var actual =
            await Assert.ThrowsAsync<ArgumentException>(async () => await WashingMachineService.RfidCardExists(rfidTest.RfidCardId));

        // Assert
        Assert.Contains("cannot be null or empty", actual.Message);
    }
}