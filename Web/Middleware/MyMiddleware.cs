using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Web.Middleware
{
    public class MyMiddleware
    {
        private readonly RequestDelegate _next;
        public MyMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            await context.Response.WriteAsync("From the USE!\n");
            await _next(context);
        }
    }
}


public class MyAuth
{
    private readonly RequestDelegate _next;
    public MyAuth(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        var username = context.Request.Query["username"];
        var password = context.Request.Query["password"];

        // Check if the provided credentials are valid
        if (!IsValidCredentials(username, password))
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Not authorized.");
            return;
        }

        // Continue to the next middleware if credentials are valid
        await context.Response.WriteAsync($"Username: {username}\n");
        await _next(context);
    }

    private bool IsValidCredentials(string username, string password)
    {
        // Validate against hardcoded credentials
        return username == "user1" && password == "password1";
    }
}