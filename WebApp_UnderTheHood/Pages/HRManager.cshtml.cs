using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp_UnderTheHood.Authorization.DTO;

namespace WebApp_UnderTheHood.Pages;

public class HrManager : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    [BindProperty] public List<WeatherForecastDTO> WeatherForecastItems { get; set; } = new List<WeatherForecastDTO>();
    public HrManager(IHttpClientFactory httpClientFactory)
    {
        this._httpClientFactory = httpClientFactory;
    }

    public async Task OnGet()
    {
        var httpclient = _httpClientFactory.CreateClient("OurWebAPI");
        WeatherForecastItems = await httpclient.GetFromJsonAsync<List<WeatherForecastDTO>>("WeatherForecast") ?? new List<WeatherForecastDTO>();
    }
}