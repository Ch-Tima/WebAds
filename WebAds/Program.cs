using BLL.Services;
using BLL.Infrastructure;
using Domain.Models;
using Azure.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Serilog;
using WebAds.Filters;

var builder = WebApplication.CreateBuilder(args);

//Azure
var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

// Add services to the container.
builder.Services.AddControllersWithViews();

//Add Services
builder.Services.AddTransient<UserServices>();
builder.Services.AddTransient<AdsServices>();
builder.Services.AddTransient<CityServices>();
builder.Services.AddTransient<CommentServices>();
builder.Services.AddTransient<CategoryServices>();

builder.Services.AddTransient<IEmailSender, SendGridEmailService>();//SendGrid

//Set SendGridEmailSenderOption
builder.Services.Configure<SendGridEmailSenderOption>(opt =>
{
    opt.ApiKey = builder.Configuration.GetValue<string>("SGKey");
    opt.SenderEmail = "logologi417@gmail.com";
    opt.SenderName = "Tima";
});

//Enable GoogleAuth
builder.Services.AddAuthentication().AddGoogle(opt =>
{
    opt.ClientId = builder.Configuration.GetValue<string>("GoogleAuthClientId");
    opt.ClientSecret = builder.Configuration.GetValue<string>("GoogleAuthClientSecret");
});

//Enable FacebookAuth
builder.Services.AddAuthentication().AddFacebook(opt =>
{
    opt.ClientId = builder.Configuration.GetValue<string>("FacebookAuthClientId");
    opt.ClientSecret = builder.Configuration.GetValue<string>("FacebookClientSecret");
});

builder.Host.UseSerilog((hostContext, asyncServiceScope, configuration) =>
{
    configuration.WriteTo.Console();
    configuration.WriteTo.File(builder.Configuration["Logging:LogPath"]);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure(builder.Configuration);

//Add Swagger
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Title = "SwaggerApi",
        Version = "v1"
    });
});
//ExceptionFilter
builder.Services.AddMvcCore(options =>
{
    options.Filters.Add<ExceptionFilter>();
});

builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAdsApi"));


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "defaultArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//area:exists

app.MapAreaControllerRoute(
    name: "Identity",
    areaName: "Identity",
    pattern: "Identity/{controller=Account}/{action=Register}");

app.MapAreaControllerRoute(
    name: "Manager",
    areaName: "Manager",
    pattern: "Manager/{controller=Home}/{action=Index}");

app.Run();