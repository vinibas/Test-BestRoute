using BestRoute.Controllers;
using BestRoute.Data;
using BestRoute.Data.Repositories;
using BestRoute.DTOs;
using BestRoute.Models;
using BestRoute.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BestRoute.UnitTests.Controllers;

public class RouteControllerTest
{
    private readonly Mock<IRouteRepository> _mockRouteRepository;
    private readonly Mock<IRouteCalculatorService> _mockRouteCalculatorService;

    private readonly RouteController _controller;

    public RouteControllerTest()
    {
        _mockRouteRepository = new Mock<IRouteRepository>();
        _mockRouteCalculatorService = new Mock<IRouteCalculatorService>();
        _controller = new RouteController(_mockRouteRepository.Object, _mockRouteCalculatorService.Object);
    }

    [Fact]
    public async Task Get_ReturnsOkResult_WithListOfRoutes()
    {
        // Arrange
        var routes = new List<SingleRoute> { new SingleRoute("AAA", "BBB", 10) };
        _mockRouteRepository
            .Setup(repo => repo.ListAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(routes);

        // Act
        var result = await _controller.Get("AAA", "BBB");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<SingleRoute>>(okResult.Value);
        Assert.Equal(routes, returnValue);
    }

    [Fact]
    public async Task Post_ReturnsCreatedResult_WhenRouteIsValid()
    {
        // Arrange
        var routeDto = new RouteUpsertDto("AAA", "BBB", 10);

        // Act
        var result = await _controller.Post(routeDto);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        _mockRouteRepository.Verify(repo => repo.AddAsync(It.IsAny<SingleRoute>()), Times.Once);
        _mockRouteRepository.Verify(ctx => ctx.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Post_ReturnsBadRequest_WhenRouteIsInvalid()
    {
        // Arrange
        var routeDto = new RouteUpsertDto("AA", "BB", 0);
        List<string> errors = 
        [
            "Origem deve ter 3 caracteres.",
            "Destino deve ter 3 caracteres.",
            "Valor deve ser maior que zero.",
        ];
        _mockRouteRepository
            .Setup(repo => repo.AddAsync(It.IsAny<SingleRoute>()))
            .ThrowsAsync(new InvalidRouteException(errors));

        // Act
        var result = await _controller.Post(routeDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(errors, badRequestResult.Value);
    }

    [Fact]
    public async Task Put_ReturnsNoContent_WhenRouteIsUpdated()
    {
        // Arrange
        var routeId = Guid.NewGuid();
        var routeDto = new RouteUpsertDto("AAA", "BBB", 10);
        var route = new SingleRoute("CCC", "DDD", 10);
        _mockRouteRepository.Setup(repo => repo.GetByIdAsync(routeId)).ReturnsAsync(route);

        // Act
        var result = await _controller.Put(routeId, routeDto);

        // Assert
        var noContentResult = Assert.IsType<NoContentResult>(result);
        _mockRouteRepository.Verify(ctx => ctx.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Put_ReturnsNotFound_WhenRouteDoesNotExist()
    {
        // Arrange
        var routeId = Guid.NewGuid();
        var routeDto = new RouteUpsertDto("AAA", "BBB", 10);
        _mockRouteRepository.Setup(repo => repo.GetByIdAsync(routeId)).ReturnsAsync((SingleRoute?)null);

        // Act
        var result = await _controller.Put(routeId, routeDto);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenRouteIsDeleted()
    {
        // Arrange
        var routeId = Guid.NewGuid();
        var route = new SingleRoute("AAA", "BBB", 10);
        _mockRouteRepository.Setup(repo => repo.GetByIdAsync(routeId)).ReturnsAsync(route);

        // Act
        var result = await _controller.Delete(routeId);

        // Assert
        var noContentResult = Assert.IsType<NoContentResult>(result);
        _mockRouteRepository.Verify(repo => repo.Delete(route), Times.Once);
        _mockRouteRepository.Verify(ctx => ctx.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenRouteDoesNotExist()
    {
        // Arrange
        var routeId = Guid.NewGuid();
        _mockRouteRepository.Setup(repo => repo.GetByIdAsync(routeId)).ReturnsAsync((SingleRoute?)null);

        // Act
        var result = await _controller.Delete(routeId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task SearchBestRoute_ReturnsOkResult_WithBestRoute()
    {
        // Arrange
        var origin = "AAA";
        var destination = "CCC";
        var allRoutes = new List<SingleRoute> { new ("AAA", "BBB", 10), new ("BBB", "CCC", 5) };
        var bestRouteMock = new Mock<IRoute>();

        bestRouteMock.Setup(r => r.ListTerminals()).Returns([ "AAA", "BBB", "CCC" ]);
        bestRouteMock.Setup(r => r.GetValue()).Returns(15);

        _mockRouteRepository
            .Setup(repo => repo.ListAsync())
            .ReturnsAsync(allRoutes);
        _mockRouteCalculatorService
            .Setup(service => service.CalculateRoute(origin, destination, allRoutes))
            .Returns(bestRouteMock.Object);

        // Act
        var result = await _controller.SearchBestRoute(origin, destination);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = okResult.Value as BestRoutesDto;
        Assert.NotNull(returnValue);
        Assert.Equal([ "AAA", "BBB", "CCC" ], returnValue.Routes);
        Assert.Equal(15, returnValue.Value);
    }

    [Fact]
    public async Task SearchBestRoute_ReturnsNotFound_WhenNoRoutesExist()
    {
        // Arrange
        var origin = "AAA";
        var destination = "BBB";

        _mockRouteRepository.Setup(repo => repo.ListAsync()).ReturnsAsync([]);

        // Act
        var result = await _controller.SearchBestRoute(origin, destination);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(result);
    }

}


