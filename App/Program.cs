using System.Threading.Tasks;
using FoodIntegration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace App
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Task quartzStartup = QuartzRegister.Run();
			CreateWebHostBuilder(args).Build().Run();
			quartzStartup.GetAwaiter().GetResult();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();
	}
}