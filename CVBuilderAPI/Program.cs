using CVBuilderAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ── Database ──────────────────────────────────────────────────
string connectionString;

// Railway individual Postgres variables
var pgHost     = Environment.GetEnvironmentVariable("PGHOST");
var pgPort     = Environment.GetEnvironmentVariable("PGPORT") ?? "5432";
var pgDatabase = Environment.GetEnvironmentVariable("PGDATABASE");
var pgUser     = Environment.GetEnvironmentVariable("PGUSER");
var pgPassword = Environment.GetEnvironmentVariable("PGPASSWORD");

if (pgHost != null && pgDatabase != null && pgUser != null && pgPassword != null)
{
    connectionString = $"Host={pgHost};Port={pgPort};Database={pgDatabase};Username={pgUser};Password={pgPassword};SSL Mode=Require;Trust Server Certificate=true";
}
else
{
    // Local development — SQL Server
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
}

// Check if using Postgres or SQL Server
if (pgHost != null)
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(connectionString));
}
else
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString));
}

// ── JWT Auth ──────────────────────────────────────────────────
var jwtKey    = Environment.GetEnvironmentVariable("JWT_KEY")
                ?? builder.Configuration["Jwt:Key"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = jwtIssuer,
            ValidAudience            = jwtIssuer,
            IssuerSigningKey         = new SymmetricSecurityKey(
                                           Encoding.UTF8.GetBytes(jwtKey)),
        };
    });

builder.Services.AddAuthorization();

// ── CORS ──────────────────────────────────────────────────────
var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL") ?? "http://localhost:3000";

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactApp", policy =>
        policy.WithOrigins(frontendUrl, "http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ── Auto migrate ──────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("ReactApp");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();