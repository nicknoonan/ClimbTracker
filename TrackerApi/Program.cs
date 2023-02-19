using Azure.Identity;
using Microsoft.Extensions.Logging;
using TrackerApi;
using TrackerApi.CacheHelper;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ISqlToken, SqlToken>();
builder.Services.AddTransient<IDatabaseHelper, DatabaseHelper>();
builder.Services.AddTransient<ICacheHelper, CacheHelper>();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

/*app.Logger.LogInformation("Fetching DB auth token...");
app.Services.GetService<ISqlToken>()?.GetToken();*/
app.Logger.LogInformation("Authenticating with AAD and establishing connection with database...");
await app.Services.GetService<IDatabaseHelper>().CheckConnection();
app.Logger.LogInformation("Database service healthy!");
app.Logger.LogInformation("Checking distributed cache connection...");
if (await app.Services.GetService<ICacheHelper>().CheckConnectionAsync())
{
    app.Logger.LogInformation("SQL cache service healthy!");
}
else
{
    app.Logger.LogError("ERROR:SQL cache service unhealthy!");
}
app.Run();
