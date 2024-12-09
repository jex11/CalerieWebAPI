using AdobeESignWebAPI.Model;
using AdobeESignWebAPI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Headers;
using System.Runtime;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<WebAPISetting>(builder.Configuration.GetSection("WebAPISetting"));
builder.Services.AddScoped<WebAPIClient>();gerhe
// Register the HttpClient and the WeatherApiClient
//builder.Services.AddHttpClient<WebAPIClient>((serviceProvider, client) =>
//{
//    var apiSetting = serviceProvider.GetRequiredService<IOptions<WebAPISetting>>().Value;
//    // Base address of the API
//    client.BaseAddress = new Uri(apiSetting.ApiUrl);
//    // Optionally, add default headers, timeouts, etc.
//    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSetting.AccessToken);
//});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
