using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Web_API.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration configuration;

    public AuthController(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    [HttpPost]
    public IActionResult Authenticate([FromBody] Credential credential)
    {
        // Verify the credential
        if (credential.Username == "admin" && credential.Password == "password")
        {
            // Creating the security context
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.Email, "admin@mywebsite.com"),
                new Claim("Department", "HR"),
                new Claim("Admin", "true"),
                new Claim("Manager", "true"),
                new Claim("EmploymentDate", "2023-01-01")
            };

            var expiresAt = DateTime.UtcNow.AddMinutes(10);

            return Ok(new
            {
                access_token = CreateToken(claims, expiresAt),
                expires_At = expiresAt
            });
        }

        ModelState.AddModelError("Unauthorized", "You are not authroized to access the endpoint");
        return Unauthorized(ModelState);
    }

    private string CreateToken(IEnumerable<Claim> claims, DateTime expireAt)
    {
        // secret key + cliaims + hash function
        var secretKey = Encoding.ASCII.GetBytes(configuration.GetValue<string>("SecretKey"));

        var jwt = new JwtSecurityToken(
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expireAt,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(secretKey),
                SecurityAlgorithms.HmacSha256Signature)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }    
}

public class Credential
{
    public string Username { get; set; }
    public string Password { get; set; }
}
