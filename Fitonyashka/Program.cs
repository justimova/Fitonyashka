using System.Text;
using Fitonyashka.InfrastructureLayer.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEnitityServices();
builder.Services.AddRepositories();
builder.Services.AddUserContext();

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
