using Azure.Identity;
using Microsoft.Extensions.Logging;
using TrackerApi;

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
app.Services.GetService<IDatabaseHelper>()?.CheckConnection();

app.Run();
