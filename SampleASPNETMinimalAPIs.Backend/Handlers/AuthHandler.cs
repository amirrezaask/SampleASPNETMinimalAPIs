using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SampleASPNETMinimalAPIs.Backend.Requests;
using SampleASPNETMinimalAPIs.Shared.Models;

namespace SampleASPNETMinimalAPIs.Backend.Handlers;

public static class AuthHandler
{
    private static string createJWTToken(IConfiguration _config, User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));    
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);    
    
        var claims = new[] {    
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),    
            new Claim(JwtRegisteredClaimNames.Email, user.Email),    
            new Claim(JwtRegisteredClaimNames.Jti, user.Id)    
        };    
    
        var token = new JwtSecurityToken("aspnet6",    
            "aspnet6",    
            claims,
            expires: DateTime.Now.AddYears(120), // till i'm dead    
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public static async Task<string> Register(SampleASPNETMinimalAPIsDbContext dbContext, IConfiguration _config, User user)
    {
        user.Id = Guid.NewGuid().ToString();
        await dbContext.Users.AddAsync(user);
        return createJWTToken(_config, user);
    }

    public static async Task<IResult> Login(SampleASPNETMinimalAPIsDbContext dbContext, IConfiguration _configuration, LoginRequest req)
    {
        var user = await dbContext.Users.Where(u => u.Email == req.Email).FirstOrDefaultAsync();
        if (user == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(createJWTToken(_configuration, user));
    }
}