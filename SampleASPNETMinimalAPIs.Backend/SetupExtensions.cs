using System.Text;
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
                Description = "JWT Bearer Token",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
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

    public static WebApplicationBuilder WithJWTConfigurations(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton(builder.Configuration.GetJWTConfigs());
        return builder;
    }
    public static WebApplicationBuilder WithAuth(this WebApplicationBuilder builder)
    {
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetJWTConfigs().Secret))
        };
        // Adds my validation config to DI for further usage.
        builder.Services.AddSingleton(tokenValidationParameters);


        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            builder.Services.AddNpgsql<SampleASPNETMinimalAPIsDbContext>(
                builder.Configuration.GetConnectionString("Database"));
        }

        return builder;
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SampleASPNETMinimalAPIs Cloud APIs");
                c.RoutePrefix = String.Empty;
            });
        }
        
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}