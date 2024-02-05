
namespace Middleware.Tests;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;


public class MyAuthTests : IAsyncLifetime
{
    IHost? host;
    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
    public async Task InitializeAsync()
    {
        host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .ConfigureServices(services =>
                        {
                        })
                        .Configure(app =>
                        {
                            app.UseMiddleware<MyAuth>();
                            app.Run(async context =>
                            {
                                await context.Response.WriteAsync("Authenticated!");
                            });
                        });
                })
                .StartAsync();
            
    }
    [Fact]
    public async Task MiddlewareTest_NotAuthenticated()
    {
        var response = await host.GetTestClient().GetAsync("/");
        Assert.NotEqual(HttpStatusCode.NotFound, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("Not authorized.", result);
    }
    [Fact]
    public async Task MiddlewareTest_Authenticated()
    {
        var response = await host.GetTestClient().GetAsync("/?username=user1&password=password1");
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("Authenticated!", result);
    }
    
    [Fact]
    public async Task MiddlewareTest_WrongPassword_NotAuthenticated()
    {
        var response = await host.GetTestClient().GetAsync("/?username=user1&password=password3");
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("Not authorized.", result);
    }
    
    [Fact]
    public async Task MiddlewareTest_WrongUsername_NotAuthenticated()
    {
        var response = await host.GetTestClient().GetAsync("/?username=user3&password=password3");
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("Not authorized.", result);
    }
}