using System.Text;
using Fytonyashka.DataAccessLayer.Repositories;
using Fytonyashka.Services;
using Fytonyashka.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IWeightService, WeightService>();
builder.Services.AddSingleton<IFileService, FileService>();
builder.Services.AddSingleton<IStaticFilePublisher, StaticFilePublisher>();
builder.Services.AddSingleton<IWeightDateRangeService, WeightDateRangeService>();
builder.Services.AddSingleton<IUserGoalService, UserGoalService>();
builder.Services.AddSingleton<ISleepService, SleepService>();
builder.Services.AddTransient<IWeightRepository, WeightRepository>();
builder.Services.AddTransient<IUserGoalRepository, UserGoalRepository>();
builder.Services.AddTransient<IWeightDateRangeRepository, WeightDateRangeRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ISleepRepository, SleepRepository>();

builder.Services.AddControllers();
var jwt = builder.Configuration.GetSection("Jwt");
var keyBytes = Encoding.UTF8.GetBytes(jwt["Key"]!);

builder.Services
  .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options => {
      options.TokenValidationParameters = new TokenValidationParameters {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = jwt["Issuer"],
          ValidAudience = jwt["Audience"],
          IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
          ClockSkew = TimeSpan.Zero
      };
  });
builder.Services.AddAuthorization();

builder.Services.AddSpaStaticFiles(configuration => {
	configuration.RootPath = "ClientApp/dist";
});
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

if (app.Environment.IsDevelopment()) {
	app.UseDeveloperExceptionPage();
} else {
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}
app.UseStaticFiles();
app.MapControllers();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => {
	endpoints.MapControllers();
});
if (builder.Environment.IsProduction()) {
	app.UseSpaStaticFiles();
}
app.UseSpa(spa => {
	spa.Options.SourcePath = "ClientApp";
	if (builder.Environment.IsDevelopment()) {
		spa.UseAngularCliServer(npmScript: "start");
	}
});
app.Run();
