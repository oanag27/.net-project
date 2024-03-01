using AutoMapper;
using Learn.Net.Container;
using Learn.Net.Helper;
using Learn.Net.Modal;
using Learn.Net.Repository;
using Learn.Net.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//dependency injection
builder.Services.AddTransient<ICustomerService,CustomerService>();
builder.Services.AddTransient<IRefreshHandler, RefreshHandler>();
//basic config for the entity framework core
builder.Services.AddDbContext<LearnContext>(x =>
x.UseSqlServer(builder.Configuration.GetConnectionString("apiconnection")));

//authentication
var authKey = builder.Configuration.GetValue<string>("JwtSettings:securityKey");
builder.Services.AddAuthentication(i =>
{
    i.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    i.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(jwt=>
    {
        jwt.RequireHttpsMetadata = true;
        jwt.SaveToken = true;
        jwt.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authKey)),
            ValidateAudience = false,
            ValidateIssuer = false,
            ClockSkew = TimeSpan.Zero
        };
    }
);


//automapper configuration
var automapper = new MapperConfiguration(item=>
{
    item.AddProfile(new AutoMapperHandler());
});
IMapper mapper = automapper.CreateMapper();
builder.Services.AddSingleton(mapper);
//cors policy
builder.Services.AddCors(options =>
{
    //policy name, allow any origin, method and header
    options.AddPolicy("corspolicy",
               builder =>
               {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

//rate limiter
builder.Services.AddRateLimiter(_=>_.AddFixedWindowLimiter(policyName:"fixedwindow", options =>
{
    options.Window = TimeSpan.FromSeconds(10);
    options.PermitLimit = 1;
    options.QueueLimit = 0;
    options.QueueProcessingOrder=System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
}).RejectionStatusCode=401);

string logPath = builder.Configuration.GetSection("Logging:Logpath").Value;
var log = new LoggerConfiguration()
    .MinimumLevel.Debug()   //capture all log level
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning) //capture only information level
    .Enrich.FromLogContext()
    .WriteTo.File(logPath)
    .CreateLogger();
builder.Logging.AddSerilog(log);

//jwt settings
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettings);

var app = builder.Build();

app.UseRateLimiter();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("corspolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
