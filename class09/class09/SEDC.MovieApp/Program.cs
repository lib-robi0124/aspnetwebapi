using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SEDC.MovieApp.DataAccess;
using SEDC.MovieApp.DataAccess.Implementation;
using SEDC.MovieApp.Helpers;
using SEDC.MovieApp.Services.Implementations;
using SEDC.MovieApp.Services.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//manual DI
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

//read from appSettings.Json, find the property AppSettings from the main Object
var appSettings = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettings);
AppSettings appSettingsObject = appSettings.Get<AppSettings>();

DependencyInjectionHelper.InjectDbContext(builder.Services, appSettingsObject.ConnectionString);
DependencyInjectionHelper.InjectRepositories(builder.Services);
DependencyInjectionHelper.InjectServices(builder.Services);

//configure JWT authentication
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false; //should be true in production, obligatory of metada for https
    x.SaveToken = true; //save token in the HttpContext
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        //the secret key
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettingsObject.SecretKey)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
