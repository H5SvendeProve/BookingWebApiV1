using BookingWebApiV1.Api.Mapper;
using BookingWebApiV1.Api.RequestDTOs;
using BookingWebApiV1.Authentication;
using BookingWebApiV1.Database;
using BookingWebApiV1.Exceptions;
using BookingWebApiV1.Services.FrontendService;
using BookingWebApiV1.Tests.TestData;
using Microsoft.Extensions.Options;
using Moq;

namespace BookingWebApiV1.Tests.Service.AngularService;

public class AngularServiceTests
{
    private IFrontendService FrontendService { get; }
    private IDatabaseContext DatabaseContext { get; }
    private IRequestMapper RequestMapper { get; }
    private IJwtProvider JwtProvider { get; }

    public AngularServiceTests()
    {
        DatabaseContext = new DatabaseContext(Constants.Constant.testDbConStringVaskeriServer);
        RequestMapper = new RequestMapper();
        var jwtOptionsMock = new Mock<IOptions<JwtOptions>>();
        jwtOptionsMock.Setup(j => j.Value).Returns(new JwtOptions
        {
            Issuer = "testIssuer",
            Audience = "testAudience",
            Secretkey = "+KbPdSgVkYp3s6v9"
        });

        JwtProvider = new JwtProvider(jwtOptionsMock.Object);
        FrontendService = new FrontendService(DatabaseContext, JwtProvider, RequestMapper);
    }

    [Fact]
    public async Task CreateNewBooking_with_No_ElectricityPrices_should_throw_notFoundException()
    {
        // Arrange
        var bookingRequest = TestDataCreator.GetTestBookingRequest();

        bookingRequest.Username = "tester";
        // set the booking to be started at today's date at 18 o'clock
        bookingRequest.StartTime =
            new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, DateTime.Now.Date.Day, 18, 0, 0);

        // get list of electricityPrices
        var prices = await DatabaseContext.GetElectricityPrices(bookingRequest.Username);

        // find the electricityPrice that starts on today's date 18 o'clock
        var priceToDeleteFromDb = prices.FirstOrDefault(price =>
            price.TimeStart.Date == bookingRequest.StartTime.Date &&
            price.TimeStart.Hour == bookingRequest.StartTime.Hour);
        
        // insert available booking times
        await TestDataInserter.InsertAvailableBookingTimes(DatabaseContext);

        // delete the electricityPrice that the booking is supposed to use
        await TestDataInserter.DeleteElectricityPrice(DatabaseContext, priceToDeleteFromDb);

        // Actual should throw a notFoundException, telling that the electricityPrice is not presented
        var actual =
            await Assert.ThrowsAsync<NotFoundException>(() => FrontendService.CreateNewBooking(bookingRequest));

        Assert.Contains("electricityPrice is not presented", actual.Message);
    }

    [Fact]
    public async Task CreateNewBooking_with_No_program_should_throw_notFoundException()
    {
        // Arrange
        // programId 230 does not exists in the database
        var bookingRequest = new CreateNewBookingRequest
        {
            Username = "tester",
            MachineManufacturer = "Tester",
            ModelName = "Test model",
            ProgramId = 230
        };
        // insert a machine
        await TestDataInserter.InsertTestMachine(DatabaseContext);
        // insert programs
        await TestDataInserter.InsertTestProgram(DatabaseContext);
        
        // map machine and programs together
        await TestDataInserter.InsertTestMachineProgram(DatabaseContext);
        // insert a department
        await TestDataInserter.InsertTestDepartment(DatabaseContext);
        // insert times to be booked
        await TestDataInserter.InsertAvailableBookingTimes(DatabaseContext);
        // insert electricityPrices
        await TestDataInserter.InsertElectricityPrices(DatabaseContext);
        // clear taken bookings
        await TestDataInserter.UpdateAllBookingTimesToBeAvailableInTestDepartment(DatabaseContext);
        // get list of booking times
        var availableBookingTimes = await FrontendService.GetAvailableBookingTimes(bookingRequest.Username);
        // take the first time
        bookingRequest.StartTime = availableBookingTimes.First().StartTime;

        // create booking with programId = 230 which does not exists in the database
        var actual =
            await Assert.ThrowsAsync<NotFoundException>(() => FrontendService.CreateNewBooking(bookingRequest));

        // Assert
        Assert.Contains("program is not presented", actual.Message);
    }

    
    [Fact]
    public async Task CreateNewBooking_with_No_AvailableBookingTimes_should_throw_notFoundException()
    {
        // Arrange
        var bookingRequest = TestDataCreator.GetTestBookingRequest();

        // Available booking times is determined from the department that the user is in. therefore we set a username that is not valid
        bookingRequest.Username = "does not exists";
        
        var actual =
            await Assert.ThrowsAsync<NotFoundException>(() => FrontendService.CreateNewBooking(bookingRequest));

        Assert.Contains("no available booking times", actual.Message);
    }

    [Fact]
    public async Task CreateNewBooking_Should_Return_BookingDTO()
    {
        // Arrange
        var bookingRequest = TestDataCreator.GetTestBookingRequest();

        // insert available booking times
        await TestDataInserter.InsertAvailableBookingTimes(DatabaseContext);

        // clear all taken booking times
        await TestDataInserter.UpdateAllBookingTimesToBeAvailableInTestDepartment(DatabaseContext);
        // get list of available booking times
        var availableBookingTimes = await FrontendService.GetAvailableBookingTimes(bookingRequest.Username);
        // take the first time
        var firstBookingTime = availableBookingTimes.First();

        // update the bookings startTime to be the first available booking time. (simulate that the booking is created to be started at the same time as the available one)
        bookingRequest.StartTime = firstBookingTime.StartTime;

        // insert test machine
        await TestDataInserter.InsertTestMachine(DatabaseContext);
        // insert programs
        await TestDataInserter.InsertTestProgram(DatabaseContext);
        // map machines and programs 
        await TestDataInserter.InsertTestMachineProgram(DatabaseContext);
        // insert a test department
        await TestDataInserter.InsertTestDepartment(DatabaseContext);
        
        // Actual should be a booking
        var actual = await FrontendService.CreateNewBooking(bookingRequest);

        // Assert
        Assert.True(actual.BookingId > 0);
    }
}