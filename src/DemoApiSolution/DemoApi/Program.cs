// Hey, I want to be a pretty standard straight forward API, give it to me off the standard menu.
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args); 

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// above this line is registering services - what functionality do we want to include in this API
var app = builder.Build();
// this is all about the request/response model of HTTP - how should requests be turned into responses.

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // add a endpoint called /openapi/v1.json 
    app.MapScalarApiReference();
}

app.MapGet("/status", () => "Looks Good!"); // "Minimal APIs" 

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers(); // when you get here during startup, use reflection to find all the controller classes in this project
// and create the phone book (route table) based on it's metadata (attributes)

app.Run(); // go in an while(true) { ...} and listen for HTTP requests.
