using Microsoft.AspNetCore.Mvc;
using Web.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDatabase(builder.Configuration)
    .AddApplicationServices()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .ConfigureApiOptions()
    .AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection()
    .UseAnyCors()
    .UseRouting();
    
app.MapControllers();
app.Run();
