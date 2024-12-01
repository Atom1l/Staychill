using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Staychill.Data;
using Staychill.Email;

var builder = WebApplication.CreateBuilder(args);

// Add EmailSender services //
builder.Services.AddTransient<Staychill.Email.IEmailSender, EmailSender>();


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<StaychillDbContext>( options=>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StaychillConnectionString")));

// Cookie for account //
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login"; // Redirect to login page
        options.LogoutPath = "/User/Logout"; // Redirect to logout action
        options.ExpireTimeSpan = TimeSpan.FromDays(7); // Set cookie expiration
    });

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
    pattern: "{controller=User}/{action=Home}/{id?}");


app.Run();
