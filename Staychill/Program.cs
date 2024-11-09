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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cart}/{action=CartIndex}/{id?}");

app.Run();
