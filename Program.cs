using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace NameDirectoryService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
//#if DOCKER
                // dirty hack for Docker only!
                .UseUrls("http://0.0.0.0:5000") // Take that, Docker port forwarding!!!
//#endif                
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
