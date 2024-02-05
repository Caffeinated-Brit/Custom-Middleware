
using Web.Middleware;
using Microsoft.AspNetCore.Http;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


app.MapGet("/", () => "Hello World!");

app.UseMiddleware<MyAuth>();
app.Run(async context =>
{
    await context.Response.WriteAsync("Authenticated!");
});

app.Run();
