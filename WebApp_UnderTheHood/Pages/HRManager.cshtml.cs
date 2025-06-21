using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp_UnderTheHood.Pages;

public class HRManager : PageModel
{
    private readonly IHttpClientFactory httpClientFactory;
    
    public HRManager(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }
    public async Task OnGet()
    {
        var httpclient = httpClientFactory.CreateClient("OurWebAPI");
        httpclient.GetFromJsonAsync<>
    }
}