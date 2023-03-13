using BookingWebApiV1.Controllers;
using BookingWebApiV1.Models.DatabaseDTOs;
using BookingWebApiV1.Services.AngularService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ILogger = BookingWebApiV1.Logging.ILogger;

namespace BookingWebApiV1.Tests.Controller;

public class AngularControllerTests
{
    private readonly Mock<IAngularService> angularServiceMock;
    private readonly Mock<ILogger> loggerMock;
    private readonly AngularController controller;

    public AngularControllerTests()
    {
        angularServiceMock = new Mock<IAngularService>();
        loggerMock = new Mock<ILogger>(new Mock<ILogger>());

        controller = new AngularController(
            angularService: angularServiceMock.Object,
            logger: loggerMock.Object);
    }

    [Fact]
    public async Task GetElectricityPrices_ReturnsOkResult_WhenPricesExist()
    {
        // Arrange
        var prices = new List<ElectricityPriceDTO>
        {
            new()
            {
                DKKPerKWh = 100,
                EURPerKWh = 100,
                Exr = 1,
                Location = "WestDenmark",
                TimeStart = DateTime.Now,
                TimeEnd = DateTime.Now.AddHours(1)
            }
        };
        angularServiceMock.Setup(s => s.GetPrices()).ReturnsAsync(prices);

        // Act
        var result = await controller.GetElectricityPrices();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<ElectricityPriceDTO>>(okResult.Value);
        Assert.Equal(prices, model);
    }

    [Fact]
    public async Task GetElectricityPrices_ReturnsBadRequest_WhenPricesDoNotExist()
    {
        // Arrange
        var prices = new List<ElectricityPriceDTO>
        {
            new()
            {
                DKKPerKWh = 100,
                EURPerKWh = 100,
                Exr = 1,
                Location = "WestDenmark",
                TimeStart = DateTime.Now,
                TimeEnd = DateTime.Now.AddHours(1)
            }
        };
        angularServiceMock.Setup(s => s.GetPrices()).ReturnsAsync(prices);

        // Act
        var result = await controller.GetElectricityPrices();

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("theres no prices", badRequestResult.Value);
    }

    [Fact]
    public async Task ValidateToken_ReturnsBadRequest_WhenTokenIsNotProvided()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };
        controller.ControllerContext = controllerContext;

        // Act
        var result = await controller.ValidateToken();

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("missing jwt token", badRequestResult.Value);
    }

    [Fact]
    public async Task ValidateToken_ReturnsUnauthorized_WhenTokenIsInvalid()
    {
        // Arrange
        var token = "invalid-token";
        angularServiceMock.Setup(s => s.ValidateToken(token)).ReturnsAsync(false);
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["Authorization"] = "Bearer " + token;
        var controllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
        controller.ControllerContext = controllerContext;

        // Act
        var result = await controller.ValidateToken();

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal("token is invalid or expired", unauthorizedResult.Value);
    }
}