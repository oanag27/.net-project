using AutoMapper;
using Learn.Net.Container;
using Learn.Net.Helper;
using Learn.Net.Repository;
using Learn.Net.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//dependency injection
builder.Services.AddTransient<ICustomerService,CustomerService>();
//basic config for the entity framework core
builder.Services.AddDbContext<LearnContext>(x =>
x.UseSqlServer(builder.Configuration.GetConnectionString("apiconnection")));



//automapper configuration
var automapper = new MapperConfiguration(item=>
{
    item.AddProfile(new AutoMapperHandler());
});
IMapper mapper = automapper.CreateMapper();
builder.Services.AddSingleton(mapper);


string logPath = builder.Configuration.GetSection("Logging:Logpath").Value;
var log = new LoggerConfiguration()
    .MinimumLevel.Debug()   //capture all log level
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning) //capture only information level
    .Enrich.FromLogContext()
    .WriteTo.File(logPath)
    .CreateLogger();
builder.Logging.AddSerilog(log);


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
