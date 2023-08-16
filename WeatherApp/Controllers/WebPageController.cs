using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WeatherApp.Data;
using WeatherApp.Models;

namespace WeatherApp.Controllers;

public class WebPageController : Controller
{
    private readonly ILogger<WebPageController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public WebPageController(ILogger<WebPageController> logger,IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Main()
    {
        using (HttpClient client = _httpClientFactory.CreateClient())
        {
            HttpResponseMessage response = await client.GetAsync("https://localhost:7246/api/Weather/GetTodaysForecast?location=Dnipropetrovsk");
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                WeatherData.Root weatherData = JsonConvert.DeserializeObject<WeatherData.Root>(responseBody);

                ViewBag.ForecastData = weatherData;
            } else
            {
                ViewBag.Error = $"Err: {response.StatusCode} -${response.ReasonPhrase} ";
            }
        }
            return View();
    }
    [HttpGet("GetPlace")]
    [HttpPost("GetPlace")]
    public async Task<IActionResult> GetPlace([FromForm] string location = "Dnipropetrovsk")
    {
        using (HttpClient client = _httpClientFactory.CreateClient())
        {
            HttpResponseMessage response = await client.GetAsync($"https://localhost:7246/api/Weather/GetTodaysForecast?location={location}");
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                WeatherData.Root weatherData = JsonConvert.DeserializeObject<WeatherData.Root>(responseBody);

                ViewBag.ForecastData = weatherData;
            }
            else
            {
                ViewBag.Error = $"Err: {response.StatusCode} - ${response.ReasonPhrase}";
            }
        }
        return View("Main");
    }



    [Route("Hourly/{location}")]
    public async Task<IActionResult> Hourly([FromRoute] string location = "Dnipropetrovsk")
    {
        using (HttpClient client = _httpClientFactory.CreateClient())
        {
            HttpResponseMessage response = await client.GetAsync($"https://localhost:7246/api/Weather/GetTodaysForecast?location={location}");
            if(response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                WeatherData.Root weatherData = JsonConvert.DeserializeObject<WeatherData.Root>(responseBody);

                ViewBag.ForecastData = weatherData;
            } else
            {
                ViewBag.Error = $"Err: {response.StatusCode} - ${response.ReasonPhrase}";
            }
        }
            return View();
    }

    [Route("Monthly/{location}")]
    public async Task<IActionResult> Monthly([FromRoute] string location = "Dnipropetrovsk")
    {
        using(HttpClient client = _httpClientFactory.CreateClient())
        {
            HttpResponseMessage response = await client.GetAsync($"https://localhost:7246/api/Weather/GetMonthsForecast?location={location}");
            if(response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                WeatherData.Root weatherData = JsonConvert.DeserializeObject<WeatherData.Root>(responseBody);

                ViewBag.ForecastData = weatherData;
            } else
            {
                ViewBag.Error = $"Err: {response.StatusCode} - ${response.ReasonPhrase}";
            }
        }
        return View();
    }

    [Route("HistoricalData/{location}")]
    public async Task<IActionResult> HistoricalData([FromRoute] string location = "Dnipropetrovsk")
    {
        using(HttpClient client = _httpClientFactory.CreateClient())
        {
            HttpResponseMessage response= await client.GetAsync($"https://localhost:7246/api/Weather/GetHistoricalData?location={location}");
            if(response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                WeatherData.Root weatherData = JsonConvert.DeserializeObject<WeatherData.Root>(responseBody);

                ViewBag.ForecastData = weatherData;
            } else
            {
                ViewBag.Error = $"Err: {response.StatusCode} - ${response.ReasonPhrase}";
            }
        }
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

