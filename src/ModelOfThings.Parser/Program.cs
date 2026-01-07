using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ModelOfThings.Parser
{
    public class Program
    {
         // This method starts the parser
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
