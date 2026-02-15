using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GameLibrary.Data;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using GameLibrary;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization(options =>
    {
        // Aceasta linie spune: "Pentru orice validare din Model, caută textul în SharedResource"
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(SharedResource));
    });

builder.Services.AddDbContext<GameLibraryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GameLibraryContext") ?? throw new InvalidOperationException("Connection string 'GameLibraryContext' not found.")));

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en-US"),
        new CultureInfo("ro-RO")
    };

    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRequestLocalization();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.MapRazorPages();

app.Run();
