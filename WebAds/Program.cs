using BLL.Services;
using BLL.Infrastructure;
using Domain.Models;
using Azure.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

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
    opt.SenderEmail = builder.Configuration["SenderGrid:SenderEmail"];
    opt.SenderName = builder.Configuration["SenderGrid:SenderName"];
});


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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "Identity",
    areaName: "Identity",
    pattern: "Identity/{controller=Account}/{action=Register}");
app.Run();
