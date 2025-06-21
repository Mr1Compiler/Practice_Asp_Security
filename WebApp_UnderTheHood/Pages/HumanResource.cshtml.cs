using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp_UnderTheHood.Pages;

[Authorize(Policy = "MustBelongToHRDepartembnt")]
public class HumanResource : PageModel
{
    public void OnGet()
    {
        
    }
}