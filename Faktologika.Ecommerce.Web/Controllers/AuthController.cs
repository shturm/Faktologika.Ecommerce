using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;
using Microsoft.AspNetCore.Identity;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AuthController(IConfiguration configuration, SignInManager<IdentityUser> signInManager)
    {
        _configuration = configuration;
        _signInManager = signInManager;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        // Simulate user authentication (replace with your actual authentication logic)
        if (await IsUserValid(model.Username, model.Password))
        {
            // Create claims for the authenticated user
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(ClaimTypes.Email, model.Username),
                new Claim("age", "42"),
                // Add more claims as needed
            };

            // Create a symmetric security key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            // Create a signing credential using the key
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create a JWT token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:DurationInMinutes"])),
                signingCredentials: creds
            );

            // Return the token as a response
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        // Return unauthorized if authentication fails
        return Unauthorized();
    }

    [HttpGet("whoami")]
    public IActionResult WhoAmI()
    {
        List<string> claimValues = new List<string>();
        Dictionary<string, string> vals = new Dictionary<string, string>();

        foreach (var claim in User.Claims)
        {
            claimValues.Add(claim.Value);
            vals[claim.Type] = claim.Value;
        }

        return Ok(new {
            isAuthenticated = User.Identity.IsAuthenticated,
            name = User.Identity.Name,
            claims = claimValues,
            claimsDict = vals
        });
    }

    private async Task<bool> IsUserValid(string username, string password)
    {
        // Replace this with your actual user authentication logic.
        // Check the provided username and password against your database or user store.
        // Return true if the user is valid; otherwise, return false.
        // This is just a placeholder example.
        // return username == "demo" && password == "password";
        var signInResult = await _signInManager.PasswordSignInAsync(username, password, true, lockoutOnFailure: false);
        if (signInResult.Succeeded)
        {
            return true;
        }

        return false;
    }

}
