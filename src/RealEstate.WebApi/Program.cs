using Microsoft.EntityFrameworkCore;
using RealEstate.Application;
using RealEstate.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<RealEstateDbContext>(options => options.UseSqlite("DataSource = ../RealEstate.Infrastructure/RealEstate.db"));

builder.Services.AddScoped<IApartmentRepository, ApartmentRepository>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IApartmentRepository).Assembly));

builder.Services.AddControllers();


var app = builder.Build();


//app.UseHttpsRedirection();

app.MapControllers();
app.UseAuthorization();



app.Run();

