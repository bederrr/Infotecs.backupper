using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;
using Microsoft.Extensions.Configuration;


namespace FirstTask
{
    class Program
    {
        private static IConfiguration configuration;

        private static IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();
        }

        static void Main(string[] args)
        {
            try
            {
                configuration = GetConfiguration();
            }

            catch (Exception e)
            {
                Console.WriteLine("appsettings.json is not exists. ", e.Message);
                Console.ReadKey();
                return;
            }

            IFileManager fileManager = new FileManager(logger: config.logger);

            if (string.IsNullOrEmpty(configuration["targetDir"].ToString()))
            {
                config.logger.Fatal("string of target dir is empty");
                return;
            }

            configuration.GetSection("sourceDir").Get<List<string>>().ForEach(action: sourceDir =>
                 {
                     try
                     {
                         if (string.IsNullOrEmpty(sourceDir))
                         {
                             throw new ArgumentNullException(nameof(sourceDir));
                         }
                         fileManager.CreateCurrentDirectory(sourceDir, configuration["targetDir"].ToString());
                     }

                     catch (ArgumentNullException)
                     {
                         config.logger.Error("String {0} is null or empty", sourceDir);
                     }
                     catch (Exception e)
                     {
                         config.logger.Fatal("The process failed: {0}", e.Message);
                     }
                 });
        }
    }
}