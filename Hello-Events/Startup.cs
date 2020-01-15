using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Hello_Events
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services){}

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync($"Hello World!\n");
                });

                endpoints.MapPost("/", async context =>
                {
                    string body;
                    using (var sr = new StreamReader(context.Request.Body))
                    {
                        body = await sr.ReadToEndAsync();
                    }

                    Console.WriteLine($"Received {body}");
                    await context.Response.WriteAsync("Processed");
                });
            });
        }
    }
}