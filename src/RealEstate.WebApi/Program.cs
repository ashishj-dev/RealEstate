using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application;
using RealEstate.Application.Behaviors;
using RealEstate.Infrastructure;
using RealEstate.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<RealEstateDbContext>(options => options.UseSqlite("DataSource = ../RealEstate.Infrastructure/RealEstate.db"));
builder.Services.AddScoped<IRealEstateDbContext>(provider => provider.GetRequiredService<RealEstateDbContext>());

builder.Services.AddScoped<IApartmentRepository, ApartmentRepository>();
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IApartmentRepository).Assembly));
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(LoggingPipelineBehavior<,>).Assembly);
    cfg.AddOpenBehavior(typeof(LoggingPipelineBehavior<,>));
});

//builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();


var app = builder.Build();


app.UseExceptionHandler();

app.MapControllers();
app.UseAuthorization();


app.Run();

public partial class Program { }