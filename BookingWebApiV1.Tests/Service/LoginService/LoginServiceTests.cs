﻿using BookingWebApiV1.Api.Mappers;
using BookingWebApiV1.Api.RequestDTOs;
using BookingWebApiV1.Authentication;
using BookingWebApiV1.Database;
using BookingWebApiV1.Exceptions;
using BookingWebApiV1.Services.LoginService;
using BookingWebApiV1.Tests.TestData;
using Microsoft.Extensions.Options;
using Moq;

namespace BookingWebApiV1.Tests.Service.LoginService;

public class LoginServiceTests
{
    private ILoginService LoginService { get; }
    private IRequestMapper RequestMapper { get; }
    private IJwtProvider JwtProvider { get; }
    private IDatabaseContext DatabaseContext { get; }

    public LoginServiceTests()
    {
        RequestMapper = new RequestMapper();
        DatabaseContext = new DatabaseContext(Constants.Constant.testDbConStringVaskeriServer);
        var jwtOptionsMock = new Mock<IOptions<JwtOptions>>();
        jwtOptionsMock.Setup(j => j.Value).Returns(new JwtOptions
        {
            Issuer = "testIssuer",
            Audience = "testAudience",
            Secretkey = "+KbPdSgVkYp3s6v9"
        });

        JwtProvider = new JwtProvider(jwtOptionsMock.Object);
        
        LoginService = new Services.LoginService.LoginService(DatabaseContext, JwtProvider, RequestMapper);
    }

    [Fact]
    public async Task LoginUser_empty_password_should_throw_BadRequestException()
    {
        var loginRequest = new LoginUserRequest("testUser", "");

        var actual = await Assert.ThrowsAsync<BadRequestException>(() => LoginService.LoginUser(loginRequest));

        Assert.Contains("Password is required", actual.Message);
    }

    [Fact]
    public async Task LoginUser_empty_username_should_throw_BadRequestException()
    {
        var loginRequest = new LoginUserRequest("", "password");
        
        var actual = await Assert.ThrowsAsync<BadRequestException>(() => LoginService.LoginUser(loginRequest));
        
        Assert.Contains("Username is required", actual.Message);
    }

    [Fact]
    public async Task LoginUser_wrong_password_should_throw_BadRequestException()
    {
        // Arrange
        var loginRequest = new LoginUserRequest("testUser", "password213");

        await InsertTestUser();
        
        // Actual
        var actual = await Assert.ThrowsAsync<BadRequestException>(async () => await LoginService.LoginUser(loginRequest));

        // Assert
        Assert.Contains("password is incorrect", actual.Message);
    }

    [Fact]
    public async Task LoginUser_user_does_not_exists_should_throw_notFoundException()
    {
        // Arrange
        var loginRequest = new LoginUserRequest("testUser1234", "password213");
        
        // Actual
        var actual = await Assert.ThrowsAsync<NotFoundException>(async () => await LoginService.LoginUser(loginRequest));
        
        // Expected
        var expected = new NotFoundException($"user with username : {loginRequest.Username} does not exist");
        
        // Assert
        Assert.Equal(expected.Message, actual.Message);
    }

    [Fact]
    public async Task RegisterNewUser_should_return_true()
    {
        await InsertNewDepartmentTestData();

        await DeleteTestUser();
        // Arrange
        var newUser = TestDataCreator.GetTestUserRequest();
        // Actual
        var newUserCreatedActual = await LoginService.RegisterNewUser(newUser);
        
        // Excepted
        bool newUserCreatedExpected = true;

        // Assert
        Assert.Equal(newUserCreatedExpected, newUserCreatedActual);
        Assert.True(newUserCreatedExpected);
    }

    [Fact]
    public async Task LoginUser_should_return_jwtToken()
    {
        var loginUserRequest = new LoginUserRequest("testUser", "password");

        await InsertTestUser();

        var actual = await LoginService.LoginUser(loginUserRequest);

        Assert.StartsWith("Bearer ", actual);
        Assert.True(actual.StartsWith("Bearer ") && actual.Length > 10);
    }

    

    private async Task InsertTestUser()
    {
        var createNewUser = TestDataCreator.GetTestUserRequest();
        await LoginService.RegisterNewUser(createNewUser);
    }

    private async Task InsertNewDepartmentTestData()
    {
        var newDepartment = TestDataCreator.GetTestDepartmentDTO();

        await DatabaseContext.InsertNewDepartment(newDepartment);
    }

    private async Task DeleteTestUser()
    {
        var user = TestDataCreator.GetTestUserRequest();

        var userDTO = RequestMapper.MapRequestToDTO(user);

        await DatabaseContext.DeleteUser(userDTO);
    }
}