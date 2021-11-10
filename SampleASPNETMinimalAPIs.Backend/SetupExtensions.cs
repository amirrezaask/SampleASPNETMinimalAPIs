﻿using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SampleASPNETMinimalAPIs.Backend.Configurations;
using SampleASPNETMinimalAPIs.Backend.Handlers;

namespace SampleASPNETMinimalAPIs.Backend;

public static class SetupExtensions
{
    public static WebApplicationBuilder WithSwagger(this WebApplicationBuilder builder)
    {
        // Swagger
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.ApiKey,
                Description = "JWT Token",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });
        });
        builder.Services.AddEndpointsApiExplorer();
        return builder;
    }
    
    public static JWTConfigurations GetJWTConfigs(this IConfiguration configuration)
    {
        var fromConfiguration = configuration.GetSection("Jwt").Get<JWTConfigurations>();
        return fromConfiguration;
    }
    public static WebApplicationBuilder WithAuth(this WebApplicationBuilder builder)
    {
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetJWTConfigs().Secret))
        };
        builder.Services.AddSingleton(tokenValidationParameters);

// Adds Authentication using jwt.
        builder.Services.AddAuthentication(o => { o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
            .AddJwtBearer(o => { o.TokenValidationParameters = tokenValidationParameters; });

        builder.Services.AddAuthorization();

        return builder;
    }
    public static WebApplicationBuilder WithDB(this WebApplicationBuilder builder)
    {
        // Database, in dev environment use sqlite
        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddSqlite<SampleASPNETMinimalAPIsDbContext>("Data Source=SampleASPNETMinimalAPIs.db");
        }
// In Production environment use posgres
        else
        {
            builder.Services.AddNpgsql<SampleASPNETMinimalAPIsDbContext>(builder.Configuration.GetConnectionString("Database"));
        }

        return builder;
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseAuthorization();
        app.UseAuthentication();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SampleASPNETMinimalAPIs Cloud APIs");
                c.RoutePrefix = String.Empty;
            });
        }
        return app;
    }
}