using ElectionAppReact.Server.Data;
using ElectionAppReact.Server.Services;
using Microsoft.EntityFrameworkCore;
using static ElectionAppReact.Server.Data.BankSeedData;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "http://localhost:5001";
        options.RequireHttpsMetadata = false;
        options.Audience = "electionapi";
    });

builder.Services.AddAuthorization();
builder.Services.AddHttpClient();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddScoped<IBankDbContextFactory, BankDbContextFactory>();
builder.Services.AddScoped<BankSeedService>();

builder.Services.AddDbContext<ElectionDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ElectionPostgres")));

var app = builder.Build();

var runMode = builder.Configuration["RunMode"] ?? "Windows";

// --------------------- DB INIT ---------------------
using (var scope = app.Services.CreateScope())
{
    var factory = scope.ServiceProvider.GetRequiredService<IBankDbContextFactory>();

    foreach (var bank in new[] { "monobank", "admin" })
    {
        var db = factory.Create(bank);
        db.Database.EnsureCreated();
    }
}

// --------------------- MIDDLEWARE ---------------------

app.UseCors("AllowReact");
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<BankSeedService>();
    await seeder.SeedAllAsync();
}

// ---------------- SPA ONLY ON WINDOWS ----------------
if (runMode != "Linux")
{
    app.UseDefaultFiles();
    app.UseStaticFiles();
    app.MapFallbackToFile("/index.html");
}

app.Run();
