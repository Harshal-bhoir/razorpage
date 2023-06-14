using Microsoft.Azure.Cosmos;
var builder = WebApplication.CreateBuilder(args);

// New instance of CosmosClient class
using CosmosClient client = new(
    accountEndpoint: Environment.GetEnvironmentVariable("https/://cosmos-acc.documents.azure.com:443/")!,
    authKeyOrResourceToken: Environment.GetEnvironmentVariable("tzuchFmoUXSHkq2HXcBwsIBJPrlI3Evrt1QUK2yYnIkwPsHa930zfrsANbEZLyMi5DNQcKXVPYM5ACDb25EfwA==")
);

// Add services to the container.
builder.Services.AddRazorPages();

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

// Database reference with creation if it does not already exist
Database database = client.GetDatabase(id: "Employees");

Console.WriteLine($"New database:\t{database.Id}");

app.Run();
