using BookingWebApiV1.Api.Mappers;
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
        DatabaseContext = new DatabaseContext(Constants.Constant.testDbConStringMkServer);
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

        bookingRequest.Username = "testUser";
        bookingRequest.StartTime =
            new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, DateTime.Now.Date.Day, 18, 0, 0);

        var prices = await DatabaseContext.GetElectricityPrices(bookingRequest.Username);


        var priceToDeleteFromDb = prices.FirstOrDefault(price =>
            price.TimeStart.Date == bookingRequest.StartTime.Date &&
            price.TimeStart.Hour == bookingRequest.StartTime.Hour);


        await TestDataInserter.DeleteElectricityPrice(DatabaseContext, priceToDeleteFromDb);

        var actual =
            await Assert.ThrowsAsync<NotFoundException>(() => FrontendService.CreateNewBooking(bookingRequest));

        Assert.Contains("not presented", actual.Message);
    }

    [Fact]
    public async Task CreateNewBooking_with_No_program_should_throw_notFoundException()
    {
        // Arrange
        // programId 1 exists in db 230 doesnt 
        var bookingRequest = new CreateNewBookingRequest
        {
            Username = "testUser",
            MachineManufacturer = "Tester",
            ModelName = "Test model",
            ProgramId = 230
        };

        await TestDataInserter.InsertTestMachine(DatabaseContext);

        await TestDataInserter.InsertTestProgram(DatabaseContext);

        await TestDataInserter.InsertTestMachineProgram(DatabaseContext);

        await TestDataInserter.InsertTestDepartment(DatabaseContext);

        await TestDataInserter.InsertAvailableBookingTimes(DatabaseContext);

        await TestDataInserter.InsertElectricityPrices(DatabaseContext);
        
        await TestDataInserter.UpdateAllBookingTimesToBeAvailableInTestDepartment(DatabaseContext);

        var availableBookingTimes = await FrontendService.GetAvailableBookingTimes(bookingRequest.Username);
        // available time 
        bookingRequest.StartTime = availableBookingTimes.First().StartTime;

        var actual =
            await Assert.ThrowsAsync<NotFoundException>(() => FrontendService.CreateNewBooking(bookingRequest));

        Assert.Contains("program is not presented", actual.Message);
    }


    [Fact]
    public async Task CreateNewBooking_with_No_AvailableBookingTimes_should_throw_notFoundException()
    {
        // Arrange
        var bookingRequest = TestDataCreator.GetTestBookingRequest();

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

        await TestDataInserter.UpdateAllBookingTimesToBeAvailableInTestDepartment(DatabaseContext);
        
        var availableBookingTimes = await FrontendService.GetAvailableBookingTimes(bookingRequest.Username);

        var firstBookingTime = availableBookingTimes.First();

        bookingRequest.StartTime = firstBookingTime.StartTime;

        await TestDataInserter.InsertTestMachine(DatabaseContext);

        await TestDataInserter.InsertTestProgram(DatabaseContext);

        await TestDataInserter.InsertTestMachineProgram(DatabaseContext);

        await TestDataInserter.InsertTestDepartment(DatabaseContext);

        await TestDataInserter.InsertAvailableBookingTimes(DatabaseContext);

        // Actual
        var actual = await FrontendService.CreateNewBooking(bookingRequest);

        // Assert
        Assert.True(actual.BookingId > 0);
    }
}