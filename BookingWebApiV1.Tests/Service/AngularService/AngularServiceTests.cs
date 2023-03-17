﻿using BookingWebApiV1.Api.Mappers;
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

        bookingRequest.Username = "does not exists";

        var actual = await Assert.ThrowsAsync<NotFoundException>(() => FrontendService.CreateNewBooking(bookingRequest));
        
        Assert.Contains("theres no electricityPrices", actual.Message);
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
        
        await InsertTestMachine();

        await InsertTestProgram();

        await InsertTestMachineProgram();

        await InsertTestElectricityPrice();

        await InsertTestDepartment();

        await InsertAvailableBookingTimes();


        var availableBookingTimes = await FrontendService.GetAvailableBookingTimes(bookingRequest.Username);
        // available time 
        bookingRequest.StartTime = availableBookingTimes.First().StartTime;
        
     
        var actual = await Assert.ThrowsAsync<NotFoundException>(() => FrontendService.CreateNewBooking(bookingRequest));
        
        Assert.Contains("machine program is not presented", actual.Message);
    }

    [Fact]
    public async Task CreateNewBooking_with_incorrect_machine_should_throw_NotFoundException()
    {
        
    }

    [Fact]
    public async Task CreateNewBooking_with_No_AvailableBookingTimes_should_throw_notFoundException()
    {
        // Arrange
        var bookingRequest = TestDataCreator.GetTestBookingRequest();

        bookingRequest.Username = "does not exists";

        var actual = await Assert.ThrowsAsync<NotFoundException>(() => FrontendService.CreateNewBooking(bookingRequest));
        
        Assert.Contains("theres no electricityPrices", actual.Message);
    }

    [Fact]
    public async Task CreateNewBooking_Should_Return_BookingDTO()
    {
        // Arrange
        var bookingRequest = TestDataCreator.GetTestBookingRequest();

        var availableBookingTimes = await FrontendService.GetAvailableBookingTimes(bookingRequest.Username);

        var firstBookingTime = availableBookingTimes.First();

        bookingRequest.StartTime = firstBookingTime.StartTime;

        await InsertTestMachine();

        await InsertTestProgram();

        await InsertTestMachineProgram();

        await InsertTestElectricityPrice();

        await InsertTestDepartment();

        await InsertAvailableBookingTimes();
        
        // Actual
        var actual = await FrontendService.CreateNewBooking(bookingRequest);

        // Assert
        Assert.True(actual.BookingId > 0);
    }

    private async Task InsertTestMachine()
    {
        var testMachine = TestDataCreator.GetTestMachine();

        await DatabaseContext.InsertNewMachine(testMachine);
    }

    private async Task InsertTestMachineProgram()
    {
        var testMachineProgram = TestDataCreator.GetTestMachineProgram();

        await DatabaseContext.insertMachineProgram(testMachineProgram);
    }

    private async Task InsertTestElectricityPrice()
    {
        var testElectricityPrice = TestDataCreator.GetTestElectricityPrice();

        await DatabaseContext.InsertElectricityPrice(testElectricityPrice);
    }
    
    private async Task InsertTestDepartment()
    {
        var newDepartment = TestDataCreator.GetTestDepartmentDTO();

        await DatabaseContext.InsertNewDepartment(newDepartment);
    }

    private async Task InsertTestProgram()
    {
        var testProgram = TestDataCreator.GetTestWashingMachineProgram();
        await DatabaseContext.InsertProgram(testProgram);
    }

    private async Task InsertAvailableBookingTimes()
    {
        await DatabaseContext.InsertAvailableBookingTimes();
    }
    
    
}