using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherApp.Data;
using static WeatherApp.Data.WeatherData;

namespace WeatherApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public WeatherController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [Route("GetTodaysForecast")]
        [HttpGet]
        public async Task<IActionResult> GetTodaysForecast([FromQuery] string location = "Dnipropetrovsk")
        {
            string apiKey = "d37533c8b1fe4729848212941231408";
            string baseUrl = "http://api.weatherapi.com/v1";
            //string location = "Dnipropetrovsk";
            string apiUrl = $"{baseUrl}/forecast.json?key={apiKey}&q={location}&days=1";

            try
            {
                using (HttpClient client = _httpClientFactory.CreateClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        return Ok(responseBody);
                    }
                    else
                    {
                        return BadRequest($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }

        }
        [Route("GetMonthsForecast")]
        [HttpGet]
        public async Task<IActionResult> GetMonthsForecast([FromQuery] string location = "Dnipropetrovsk")
        {
            string apiKey = "d37533c8b1fe4729848212941231408";
            string baseUrl = "http://api.weatherapi.com/v1";
            //string location = "Dnipropetrovsk";
            int daysInCurrentMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            int daysRemaining = daysInCurrentMonth - DateTime.Now.Day + 1;

            
            string apiUrl = $"{baseUrl}/forecast.json?key={apiKey}&q={location}&days={daysRemaining}";

            try
            {
                using (HttpClient client = _httpClientFactory.CreateClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if(response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        return Ok(responseBody);
                    }
                    else
                    {
                        return BadRequest($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [Route("GetHistoricalData")]
        [HttpGet]
        public async Task<IActionResult> GetHistoricalData([FromQuery] string location = "Dnipropetrovsk", [FromQuery] int month = 1, [FromQuery] int year = 2023)
        {
            string apiKey = "d37533c8b1fe4729848212941231408";
            string baseUrl = "http://api.weatherapi.com/v1";

            int daysInSelectedMonth = DateTime.DaysInMonth(year, month);

            string apiUrl = $"{baseUrl}/history.json?key={apiKey}&q={location}&dt={year}-{month}-01&end_dt={year}-{month}-{daysInSelectedMonth}";

            try
            {
                using (HttpClient client = _httpClientFactory.CreateClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        return Ok(responseBody);
                    }
                    else
                    {
                        return BadRequest($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }   
        
    }
}



