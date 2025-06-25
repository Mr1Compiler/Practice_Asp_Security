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

    // We should work here now
    public async Task OnGet()
    {
        // get token from the session
        JwtToken token = new JwtToken();
        var strTokenObj = HttpContext.Session.GetString("access_token");

        if (string.IsNullOrEmpty(strTokenObj))
        {
            token = await Authenticate();
        }
        else
        {
            token = JsonConvert.DeserializeObject<JwtToken>(strTokenObj) ?? new JwtToken();
        }

        if (token == null ||
            string.IsNullOrWhiteSpace(token.AccessToken) ||
            token.ExpiresAt <= DateTime.UtcNow)
        {
            token = await Authenticate();
        }

        var httpclient = _httpClientFactory.CreateClient("OurWebAPI");
        httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken ?? string.Empty);
        WeatherForecastItems = await httpclient.GetFromJsonAsync<List<WeatherForecastDTO>>("WeatherForecast") ?? new List<WeatherForecastDTO>();
    }

    private async Task<JwtToken> Authenticate()
    {
        var httpclient = _httpClientFactory.CreateClient("OurWebAPI");
        var res = await httpclient.PostAsJsonAsync("auth",
            new Credentials { Username = "admin", Password = "password" });
        res.EnsureSuccessStatusCode();
        string strJwt = await res.Content.ReadAsStringAsync();

        HttpContext.Session.SetString("access_token", strJwt);
        return JsonConvert.DeserializeObject<JwtToken>(strJwt)?? new JwtToken();    
    }
}