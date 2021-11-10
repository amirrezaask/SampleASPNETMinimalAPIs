using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SampleASPNETMinimalAPIs.Backend;
using SampleASPNETMinimalAPIs.Backend.Handlers;

var builder = WebApplication.CreateBuilder(args);

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

// Adds Authentication using jwt.
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthorization();
// Swagger
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

// Build application DI
var app = builder.Build();

// From this point order matters since we are configuring request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SampleASPNETMinimalAPIs Cloud APIs");
        c.RoutePrefix = String.Empty;
    });

}
// Add authentication middleware to middleware pipeline
app.UseAuthentication();
app.UseAuthorization();
// Map API endpoints
app.MapNotes("/api/v1");
app.MapSavedPassword("/api/v1");
app.MapAuth("/api/v1");
app.Run();