using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp_UnderTheHood.Pages.Account;

public class Login : PageModel
{
    [BindProperty]
    public Credentials Credential { get; set; } = new Credentials();
    
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        // Verify the credential
        if (Credential.Username == "admin" && Credential.Password == "password")
        {
            // Creating the security context
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.Email, "admin@mywebsite.com"),
                new Claim("Department", "HR"),
                new Claim("Admin", "true"),
                new Claim("Manager", "true"),
                new Claim("EmploymentDate", "2023-01-01")
            };
            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = Credential.RememberMe
            };

            await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal, authProperties);

            return RedirectToPage("/Index");
        }

        return Page();
    }
}

public class Credentials
{
    [Required, Display(Name = "User Name")]
    public string Username { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Remember Me")] public bool RememberMe { get; set; }
}