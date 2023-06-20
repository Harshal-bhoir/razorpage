using Microsoft.Azure.Cosmos;
using RazorApp.Services;
using Azure.Storage.Blobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<IEmployeeService>(options =>
{
    string url = builder.Configuration.GetSection("AzureCosmosDbSettings")
    .GetValue<string>("URL");
    string primaryKey = builder.Configuration.GetSection("AzureCosmosDbSettings")
    .GetValue<string>("PrimaryKey");
    string dbName = builder.Configuration.GetSection("AzureCosmosDbSettings")
    .GetValue<string>("DatabaseName");
    string containerName = builder.Configuration.GetSection("AzureCosmosDbSettings")
    .GetValue<string>("ContainerName");
    CosmosClient cosmosClient = new CosmosClient(
        url,
        primaryKey
    );
    return new EmployeeService(cosmosClient, dbName, containerName);
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
