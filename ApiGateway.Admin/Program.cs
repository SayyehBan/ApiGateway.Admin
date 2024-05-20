using ApiGateway.Admin.Models.Links;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
.AddJsonFile("ocelot.json")
    .AddOcelot(builder.Environment)
    .AddEnvironmentVariables();

var AuthenticationSchemeKey = "ApiGatewayAdminAuthenticationScheme";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(AuthenticationSchemeKey, option =>
    {
        option.Authority = LinkServices.IdentityServer;
        option.Audience = "apigatewayadmin";
    });


builder.Services.AddOcelot(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapControllers();
app.UseRouting();
app.UseOcelot().Wait();
app.Run();
