using DNTCaptcha.Core;
using ERecruitmentSystem02.Common.HelperClasses;
using ERecruitmentSystem02.Common.Utility;
using ERecruitmentSystem02.Repository;
using ERecruitmentSystem02.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IDataAccess, DataAccess>();
builder.Services.AddScoped<IDataInsert, DataInsert>();
builder.Services.AddScoped<IPasswordHasher<object>, PasswordHasher<object>>();
builder.Services.AddScoped<IPasswordHandler, PasswordHandler>();
builder.Services.AddScoped<IAccountService, AccountService>();


builder.Services.AddDNTCaptcha(options =>
{
    options.UseCookieStorageProvider(SameSiteMode.Strict)
    .AbsoluteExpiration(minutes: 20)
    .ShowThousandsSeparators(false)
    .WithNoise(0.3f, 0.3f, 1, 0.0f)
    .WithEncryptionKey("This is my secure key!")
    .InputNames(
        new DNTCaptchaComponent
        {
            CaptchaHiddenInputName = "DNT_CaptchaText",
            CaptchaHiddenTokenName = "DNT_CaptchaToken",
            CaptchaInputName = "DNT_CaptchaInputText"
        })
    .Identifier("dnt_Captcha")
    ;
});


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
     .AddCookie(options =>
     {
         options.LoginPath = "/Account/Login";
         options.LogoutPath = "/Account/Logout";
     });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
