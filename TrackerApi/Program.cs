using Azure.Identity;
using TrackerApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
if (builder.Environment.IsProduction())
{
    builder.Configuration.AddAzureKeyVault(
        new Uri($"http://{builder.Configuration["AZURE_KEY_VAULT_NAME"]}.vault.azure.net/"),
        new DefaultAzureCredential(),
        new PrefixKeyVaultSecretManager(builder.Configuration["AZURE_KEY_VAULT_SECRET_PREFIX"]));
}
else if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<IConfiguration>();
    
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
