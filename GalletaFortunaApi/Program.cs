using GalletaFortunaApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllHeaders",
    builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAllHeaders");

app.MapGet("/getFortune", () =>
{
    var file = File.ReadAllText(Directory.GetCurrentDirectory() + "/fortune.json");
    var galletas = (List<galleta>)JsonConvert.DeserializeObject(file, typeof(List<galleta>));
    var randon = Random.Shared.Next(0, galletas.Count);
    var galleta = galletas.SingleOrDefault(g => g.id == randon);
    return galleta;
})
.WithName("getFortune");

app.Run();

