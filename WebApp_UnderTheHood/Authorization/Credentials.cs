using System.ComponentModel.DataAnnotations;

namespace WebApp_UnderTheHood.Authorization;

public class Credentials
{
    [Required, Display(Name = "User Name")]
    public string Username { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Remember Me")] public bool RememberMe { get; set; }
}