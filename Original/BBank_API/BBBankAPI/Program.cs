using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;
using Services.Contracts;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// Reading the appsettings.json from current directory.
var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

// Fetching the value BBBankDBConnString from connectionstring section.
var connectionString = configuration.GetConnectionString("BBBankDBConnString");


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                                                  .AllowAnyMethod();
                      });
});
// Add services to the container.

builder.Services.AddControllers();


///...Dependency Injection settings
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<DbContext, BBBankContext>();
builder.Services.AddScoped<IAccountsService, AccountService>();
//Adding EF DBContext in the application services using the connectionString fetched above.
//UseLazyLoadingProxies : Lazy loading means that the related data is transparently loaded from the database when the navigation property is accessed.
builder.Services.AddDbContext<BBBankContext>(
b => b.UseSqlServer(connectionString)
.UseLazyLoadingProxies(false)
);

//AddJsonOptions to remove object cycle error in Account-User
builder.Services.AddControllers().AddJsonOptions(x =>
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Disabling default model validation because we have to use custom validation
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();

app.MapControllers();

app.Run();
