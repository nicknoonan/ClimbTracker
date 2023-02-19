using Azure.Identity;
using Microsoft.Extensions.Logging;
using TrackerApi.CacheHelper;
using Microsoft.Extensions.Caching.Distributed;
using TrackerApi.DatabaseHelper;

//create builder
var builder = WebApplication.CreateBuilder(args);

//add configuration provider
builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ISqlToken, SqlToken>();
builder.Services.AddTransient<IDatabaseHelper, DatabaseHelper>();
builder.Services.AddTransient<ICacheHelper, CacheHelper>();
builder.Services.AddHostedService<CacheCleanup>();
//build app
var app = builder.Build();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

//install middlewear
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//establish connection to db
app.Logger.LogInformation("Authenticating with AAD and establishing connection with database...");
await app.Services.GetService<IDatabaseHelper>().CheckConnectionAsync();
app.Logger.LogInformation("Database service healthy!");

//establish connection to distributed sql cache
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
