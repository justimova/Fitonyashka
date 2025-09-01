using Fytonyashka.DataAccessLayer.Repositories;
using Fytonyashka.Services;
using Fytonyashka.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IWeightService, WeightService>();
builder.Services.AddSingleton<IFileService, FileService>();
builder.Services.AddSingleton<IStaticFilePublisher, StaticFilePublisher>();
builder.Services.AddSingleton<IWeightDateRangeService, WeightDateRangeService>();
builder.Services.AddSingleton<IUserGoalService, UserGoalService>();
builder.Services.AddTransient<IWeightRepository, WeightRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
