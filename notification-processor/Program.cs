using Microsoft.AspNetCore.Hosting;

namespace NotifyProcessor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://*:4000/")
                .UseStartup<Startup>()
                .Build();

            host.Run();


        }
    }
}