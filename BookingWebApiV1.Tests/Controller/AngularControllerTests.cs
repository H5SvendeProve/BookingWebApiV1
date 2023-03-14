using BookingWebApiV1.Controllers;
using BookingWebApiV1.Models.DatabaseDTOs;
using BookingWebApiV1.Services.AngularService;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace BookingWebApiV1.Tests.Controller;

public class AngularControllerTests
{
    private readonly Mock<IAngularService> angularServiceMock;
    private readonly AngularController controller;

    public AngularControllerTests()
    {
        angularServiceMock = new Mock<IAngularService>();

        controller = new AngularController(angularServiceMock.Object);
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
}