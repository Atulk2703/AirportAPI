using AirpotAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;

[Route("api/airports")]
[ApiController]
public class AirportController : ControllerBase
{
    private static Dictionary<string, List<string>> routes = new()
    {
        { "NYC", new List<string> { "LON", "PAR", "LAX", "ROM" } },
        { "LON", new List<string> { "NYC", "PAR" } },
        { "PAR", new List<string> { "NYC", "LON" } },
        { "LAX", new List<string> { "NYC" } },
        { "ROM", new List<string> { "NYC" } }
    };

    [HttpGet("destinations")]
    public ActionResult<IEnumerable<Airport>> GetDestinations(string originAirportCode)
    {
        Log.Information("Request received for origin airport code: {OriginAirportCode}", originAirportCode);

        if (string.IsNullOrEmpty(originAirportCode))
        {
            return BadRequest("Origin airport code is required.");
        }

        if (!routes.ContainsKey(originAirportCode))
        {
            Log.Warning("Origin airport code is missing in the request.");
            return BadRequest("Origin airport code is not valid.");
        }

        
        var destinationAirportCodes = routes[originAirportCode];

       
        var airports = new List<Airport>();

        foreach (var destinationCode in destinationAirportCodes)
        {
            airports.Add(new Airport
            {
                Code = destinationCode,
                Name = GetAirportName(destinationCode) 
            });
        }
        Log.Information("Returning {DestinationCount} destinations for origin airport code: {OriginAirportCode}", destinationAirportCodes.Count, originAirportCode);
        return Ok(airports);
    }


    private string GetAirportName(string airportCode)
    {
        var airportNames = new Dictionary<string, string>
    {
        { "NYC", "New York City" },
        { "LON", "London" },
        { "PAR", "Paris" },
        { "LAX", "Los Angeles" },
        { "ROM", "Rome" }
    };

        if (airportNames.ContainsKey(airportCode))
        {
            return airportNames[airportCode];
        }
        else
        {
            return "Unknown";
        }
    }
}
