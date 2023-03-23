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
        DatabaseContext = new DatabaseContext(Constants.Constant.testDbConStringVaskeriServer);
        RequestMapper = new RequestMapper();
        WashingMachineService = new Services.WashingMachineService.WashingMachineService(DatabaseContext, RequestMapper);
    }

    [Fact]
    public async Task GetBookingConnectedToRfid_should_return_bookingDTO()
    {
        // Arrange
        var testBooking = TestDataCreator.GetTestBooking();
       
        // Insert available booking times
        await TestDataInserter.InsertAvailableBookingTimes(DatabaseContext);

        // clear all taken times. (as we only have 5 times per day, they all gets taken during tests)
        await TestDataInserter.UpdateAllBookingTimesToBeAvailableInTestDepartment(DatabaseContext);

        // get available booking times
        var availableBookingTimes = await TestDataInserter.GetAvailableBookingTimes(DatabaseContext);
        
        // take the first time from the list
        var firstAvailableTime = availableBookingTimes.First();

        // update the test bookings start time to be the first available start time
        testBooking.StartTime = firstAvailableTime.StartTime;

        // update the test bookings end time to be the first available end time
        testBooking.EndTime = firstAvailableTime.EndTime;

        // simulate that the user is going to scan his RFID-card 5 minutes after the booking is created
        var scannedTime = firstAvailableTime.StartTime.AddMinutes(5);
        
        // insert RFID card to database
        await TestDataInserter.InsertTestRfidCard(DatabaseContext);
        // get the RFID-card from database
        var testRfidCard = TestDataCreator.GetTestRfidCard();

        // create the test booking
        var bookingCreated = await TestDataInserter.InsertTestBooking(DatabaseContext, testBooking);

        // set the booking Id on the available time 
        firstAvailableTime.bookingId = bookingCreated.BookingId;

        // update database with the available time. Which makes it taken by the created booking
        await TestDataInserter.UpdateAvailableBookingTimes(DatabaseContext, firstAvailableTime);

        // get the booking from RFID-card
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
        // Insert a test booking to database
        var testBookingCreated = await TestDataInserter.InsertTestBooking(DatabaseContext, testBooking);
        
        // Actual the booking program
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
        
        // insert a valid RFID-card
        await TestDataInserter.InsertTestRfidCard(DatabaseContext);
        
        // Actual get the RFID-card
        var actual = await WashingMachineService.RfidCardExists(rfidTest.RfidCardId);

        // Assert
        Assert.True(actual);
    }
    
    [Fact]
    public async Task RfidCardExists_rfid_not_in_database_should_return_false()
    {
        // Arrange
        var rfidTest = TestDataCreator.GetTestRfidCard();
        // RfidCardId that does not exists in the database
        rfidTest.RfidCardId = "does not exist";
        
        // insert the correct RFID-Card
        await TestDataInserter.InsertTestRfidCard(DatabaseContext);
        
        // Actual check if the rfidCard exists
        var actual = await WashingMachineService.RfidCardExists(rfidTest.RfidCardId);

        // Assert
        Assert.False(actual);
    }
    
    [Fact]
    public async Task RfidCardExists_rfidCardId_is_empty_should_throw_argumentException()
    {
        // Arrange
        var rfidTest = TestDataCreator.GetTestRfidCard();
        // Empty RfidCardId to simulate that the user didnt parse the RFIDCard to the method
        rfidTest.RfidCardId = "";
        
        // insert a valid RfidCard
        await TestDataInserter.InsertTestRfidCard(DatabaseContext);
        
        // Actual, check if rfidTest exists in the databae
        var actual =
            await Assert.ThrowsAsync<ArgumentException>(async () => await WashingMachineService.RfidCardExists(rfidTest.RfidCardId));

        // Assert
        Assert.Contains("cannot be null or empty", actual.Message);
    }
}