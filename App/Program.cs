using FoodIntegration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
			QuartzRegister.Run().GetAwaiter().GetResult();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
