using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using WebApp_UnderTheHood.Authorization;
using WebApp_UnderTheHood.Authorization.DTO;
using WebApp_UnderTheHood.Pages.Account;

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
        // Post to authentication endpoint to generate jwtToken and send it back to the endopint that require jwt auth
        var res = await httpclient.PostAsJsonAsync("auth", new Credentials { Username = "admin", Password = "password" });
        //== Loggning ==
        Console.WriteLine("jwtToken is : {0}", res);

        res.EnsureSuccessStatusCode();
        Console.WriteLine(res.EnsureSuccessStatusCode());

        string strJwt = await res.Content.ReadAsStringAsync();
        var token = JsonConvert.DeserializeObject<JwtToken>(strJwt);


        httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken ?? string.Empty);
        WeatherForecastItems = await httpclient.GetFromJsonAsync<List<WeatherForecastDTO>>("WeatherForecast") ?? new List<WeatherForecastDTO>();
    }
}