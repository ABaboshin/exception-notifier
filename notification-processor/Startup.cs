using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NotifyProcessor.Config;
using NotifyProcessor.Logic;

namespace NotifyProcessor
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }
        RedisListener _listener;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(env.ContentRootPath)
                // .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                ;

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            
            var cfg = new FullConfiguration();
            Configuration.Bind(cfg);

            _listener = new RedisListener();
            _listener.Config = cfg;
            _listener.Run();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.Run(context =>
            {
                return context.Response.WriteAsync("Hello from ASP.NET Core!");
            });
        }
    }
}