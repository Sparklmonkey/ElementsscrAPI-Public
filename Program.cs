using System.Text;
using ElementscrAPI.Data;
using ElementscrAPI.Filters;
using ElementscrAPI.Hubs;
using ElementscrAPI.Identity;
using ElementscrAPI.Repositories;
using ElementscrAPI.Services;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
// Add services to the container.

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.AllowSynchronousIO = true;
});

builder.Services.AddCors();
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x => {
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidIssuer = config["JwtSettings:Issuer"],
        ValidAudience = config["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!)),
        ClockSkew = TimeSpan.FromSeconds(15)
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(IdentityHolder.AdminUserPolicyName, builder =>
    {
        builder.RequireClaim(IdentityHolder.AdminUserClaimName, "yes");
    });
});
builder.Services.AddControllers();

builder.Services.AddDbContext<PlayerDataContext>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IJwtAuth, JwtAuth>();
builder.Services.AddScoped<IPvpRoomService, PvpRoomService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddFastEndpoints();
builder.Services.AddSwaggerDoc();
builder.Services.AddSignalR();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseFileServer();
var provider = new FileExtensionContentTypeProvider();
provider.Mappings.Remove(".unityweb");
provider.Mappings.Add(".unityweb", "application/octet-stream");
provider.Mappings.Remove(".mem");
provider.Mappings.Add(".mem", "application/octet-stream");
provider.Mappings.Remove(".unity3d");
provider.Mappings.Add(".unity3d", "application/vnd.unity");
provider.Mappings.Remove(".data");
provider.Mappings.Add(".data", "application/octet-stream");
provider.Mappings.Remove(".atlas");
provider.Mappings.Add(".atlas", "text/plain");
provider.Mappings.Remove(".wasm");
provider.Mappings.Add(".wasm", "application/wasm");
provider.Mappings.Remove(".symbols.json");
provider.Mappings.Add(".symbols.json", "application/octet-stream");
provider.Mappings.Remove(".js");
provider.Mappings.Add(".js", "application/javascript");
app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider
});

app.UseRouting();

app.UseCors(builder => builder
        .WithOrigins("https://v6p9d9t4.ssl.hwcdn.net", "https://elementstherevival.com/", "https://www.elementstherevival.com/")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
);

app.MapHub<PvpHub>("/pvphub");   
app.UseFastEndpoints();

app.UseOpenApi();
app.UseSwaggerUi3(s => s.ConfigureDefaults());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();