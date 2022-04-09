using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OData;
using OdataDemo;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System.Reflection;
using OData.Swagger.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddOData(options => options.AddRouteComponents("odata", GetEdmModel()).Select().Filter().Expand().OrderBy());
IServiceCollection serviceCollection = builder.Services.AddDbContext<OdataOrderContext>(
    options => options.UseNpgsql("Host=localhost;Port=5432;database=OdataOrders;User Id=postgres;Password=root;"));
builder.Services.AddSwaggerGen((config) =>
    {
    config.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Title = "Swagger Odata Demo Api",
        Description = "Swagger Odata Demo",
        Version = "v1"
    });
});

builder.Services.AddOdataSwaggerSupport();
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI((config) =>
{
    config.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger Odata Demo Api");
});
app.MapControllers();


app.Run();

static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    builder.EntitySet<Customer>("Customer");
    return builder.GetEdmModel();
}