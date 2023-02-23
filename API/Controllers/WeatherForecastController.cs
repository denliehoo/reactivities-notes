using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
/*
    [ApiController] is an attribute. It indicates that a type and all derived types
    are used to serve HTTP API responses
    [Route] is so that our app knows where to redirect the HTTP request to
    the ["controller"] is like a placeholder for the name of the actual name 
    of the controller. In this case, it would be localhost:5000/weatherforecast
    Hence, how it got the name is that WeatherForecastController gave the
    /weatherforecast portion
*/

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    /*
        This is the end point
        It specifies which method is going to be used
        e.g. is it a HttpGet HttpPut etc.
        Inside thatm we can see that return (?) type is <WeatherForecast>
        which is the file from our our WeatherForecast.cs. 
        Hence, this is why when we do CD into our API folder and do
        "dotnet run" to start the server and go to http://localhost:5000/swagger 
        date, temperatureC, temperaturF and summary
    */
    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
