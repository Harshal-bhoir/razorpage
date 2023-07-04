using RazorApp.CustomMiddlewares;
using Microsoft.Azure.Cosmos;
using RazorApp.Services;
using Azure.Storage.Blobs;
using Azure.Identity;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Azure.Security.KeyVault.Secrets;
using RazorApp.EnvConfig;
using RazorApp.EnvConfig;

var builder = WebApplication.CreateBuilder(args);

ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
    builder.AddDebug();
});

builder.Services.AddApplicationInsightsTelemetry();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddTransient<IAppConfig, AppConfig>();
builder.Services.AddSingleton<IEmployeeService>(options =>
{
    var keyVaultUrl = builder.Configuration.GetSection("KeyVault:KeyVaultUrl");
    var keyVaultClientId = builder.Configuration.GetSection("KeyVault:ClientId");
    var keyVaultSecret = builder.Configuration.GetSection("KeyVault:ClientSecret");
    var keyVaultDirectoryId = builder.Configuration.GetSection("KeyVault:DirectoryId");

    var credentials = new ClientSecretCredential(keyVaultDirectoryId.Value.ToString(), keyVaultClientId.Value!.ToString(), keyVaultSecret.Value!.ToString());
    builder.Configuration.AddAzureKeyVault(keyVaultUrl.Value!.ToString(), keyVaultClientId.Value!.ToString(), keyVaultSecret.Value!.ToString(), new DefaultKeyVaultSecretManager());
    var client = new SecretClient(new Uri(keyVaultUrl.Value!.ToString()), credentials);

    string connString = client.GetSecret("CosmosConn").Value.Value.ToString();

    string dbName = builder.Configuration.GetSection("AzureCosmosDbSettings")
    .GetValue<string>("DatabaseName");
    string containerName = builder.Configuration.GetSection("AzureCosmosDbSettings")
    .GetValue<string>("ContainerName");

    CosmosClient cosmosClient = new CosmosClient(
        connString
    );
    ILogger<EmployeeService> logger = loggerFactory.CreateLogger<EmployeeService>();
    return new EmployeeService(cosmosClient, dbName, containerName, logger);
});

builder.Services.AddSingleton<IAzBlobService>(options =>
{
    string connString = builder.Configuration.GetSection("AzureStorageSettings")
    .GetValue<string>("ConnString");
    BlobServiceClient blobServiceClient = new BlobServiceClient(connString);
    BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient("images");
    return new AzBlobService(blobServiceClient, blobContainerClient);
});

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
