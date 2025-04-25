using BestRoute.Data.Repositories;
using BestRoute.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BestRoute.Controllers;

using BestRoute.Data;
using BestRoute.Models;
using BestRoute.Services;

[ApiController]
[Route("[controller]")]
public class RouteController : ControllerBase
{
    private readonly IRouteRepository _routeRepository;
    private readonly IRouteCalculatorService _routeCalculatorService;

    public RouteController(
        IRouteRepository routeRepository,
        IRouteCalculatorService routeCalculatorService)
    {
        _routeRepository = routeRepository;
        _routeCalculatorService = routeCalculatorService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(SingleRoute), 200)]
    public async Task<IActionResult> Get([FromQuery] string? origin, string? destination)
    {
        var routes = await _routeRepository.ListAsync(origin, destination);
        return Ok(routes);
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(typeof(IEnumerable<string>), 400)]
    public async Task<IActionResult> Post([FromBody] RouteUpsertDto routeDto)
    {
        try
        {
            var route = new SingleRoute(routeDto.Origin, routeDto.Destination, routeDto.Value);
            await _routeRepository.AddAsync(route);
        }
        catch (InvalidRouteException e)
        {
            return BadRequest(e.Errors);
        }

        await _routeRepository.SaveChangesAsync();

        return Created();
    }

    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(IEnumerable<string>), 400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Put(Guid id, [FromBody] RouteUpsertDto routeDto)
    {
        var route = await _routeRepository.GetByIdAsync(id);
        if (route == null) return NotFound();

        try
        {
            route.Update(routeDto.Origin, routeDto.Destination, routeDto.Value);
        }
        catch (InvalidRouteException e)
        {
            return BadRequest(e.Errors);
        }
        
        await _routeRepository.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var route = await _routeRepository.GetByIdAsync(id);
        if (route == null) return NotFound();

        _routeRepository.Delete(route);
        await _routeRepository.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("SearchBestRoute/{origin}/{destination}")]
    [ProducesResponseType(typeof(BestRoutesDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> SearchBestRoute(string origin, string destination)
    {
        origin = origin.ToUpper();
        destination = destination.ToUpper();
        
        var allRoutes = await _routeRepository.ListAsync();
        if (!allRoutes.Any()) return NotFound();

        IRoute bestRoute;
        try
        {
            bestRoute = _routeCalculatorService.CalculateRoute(origin, destination, allRoutes);
        }
        catch (InvalidRouteException e)
        {
            return NotFound(e.Errors);
        }

        return Ok(new BestRoutesDto(bestRoute.ListTerminals(), bestRoute.GetValue()));
    }
}
