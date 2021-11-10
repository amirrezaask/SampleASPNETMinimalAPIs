using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SampleASPNETMinimalAPIs.Backend.Configurations;
using SampleASPNETMinimalAPIs.Backend.Contracts;
using SampleASPNETMinimalAPIs.Shared.Models;

namespace SampleASPNETMinimalAPIs.Backend.Handlers;

public static class AuthHandler
{
    private static string createJWTToken(JWTConfigurations _config, [FromBody] User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Secret));    
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);    
    
        var claims = new[] {    
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),    
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),    
            new Claim(JwtRegisteredClaimNames.Jti, user.Id)    
        };    
    
        var token = new JwtSecurityToken("aspnet6",    
            user.Username,    
            claims,
            expires: DateTime.Now.Add(_config.ExpiresIn),
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public static async Task<string> Register(SampleASPNETMinimalAPIsDbContext dbContext, JWTConfigurations _config, User user)
    {
        user.Id = Guid.NewGuid().ToString();
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
        return createJWTToken(_config, user);
    }

    public static async Task<IResult> Login(SampleASPNETMinimalAPIsDbContext dbContext, JWTConfigurations _configuration, LoginRequest req)
    {
        var user = await dbContext.Users.Where(u => u.Username == req.Username).FirstOrDefaultAsync();
        if (user == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(createJWTToken(_configuration, user));
    }

    public static ClaimsPrincipal ValidateToken(TokenValidationParameters tokenValidationParameters,JWTConfigurations _configuration, [FromQuery] string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var claims = handler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
        if (validatedToken != null)
        {
            return claims;
        }

        return null;
    }
}