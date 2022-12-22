using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using Auth.Half.Models.ApiModels;
using Auth.Half.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TokenLib;

namespace Auth.Half.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IDataService _dataService;

    public AuthController(IAuthService authService, IDataService dataService)
    {
        _authService = authService;
        _dataService = dataService;
    }

    [HttpPost]
    public  Task SignIn([FromBody] SignInRequest request)
    {
        //TODO: validation

        //TODO: sign in
       return _authService.SingInAsync(request.Email,request.Password,request.Login);

    }

    [HttpPost]
    public async Task<IActionResult> SingUp([FromBody] SignUpRequest request)
    {
        var identity = await GetIdentity(request.Email, request.Password);
        if (identity == null)
        {
            return BadRequest(new { errorText = "Invalid username or password." });
        }

        var now = DateTime.UtcNow;
        // создаем JWT-токен
        var jwt = new JwtSecurityToken(
            issuer: TokenAuthOptions.ISSUER,
            audience: TokenAuthOptions.AUDIENCE,
            notBefore: now,
            claims: identity.Claims,
            expires: now.Add(TimeSpan.FromMinutes(TokenAuthOptions.LIFETIME)),
            signingCredentials: new SigningCredentials(TokenAuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        var response = new
        {
            access_token = encodedJwt,
            username = identity.Name
        };

        var s = JsonSerializer.Serialize(response);
        return Ok(s);
    }

    private async Task<ClaimsIdentity> GetIdentity(string email, string password)
    {
       var user = await _authService.SingUpAsync(email, password);

        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, "user")
        };
        ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
        return claimsIdentity;


        // если пользователя не найдено
    }


    [HttpGet]
    [Authorize]
    public Task SignOut()
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    [Authorize]
    public Task<string> GetName()
    {
        return _dataService.GetNameAsync(User.Identity.Name);
    }
}