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
        private static Properties ReadAppsettings()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            return new Properties(sourceDir: configuration.GetSection("sourceDir").Get<List<string>>(),
                                            targetDir: configuration["targetDir"].ToString(),
                                            debugLevel: configuration["debugLevel"].ToString());
        }

        static void Main(string[] args)
        {
            try
            {
                var properties = ReadAppsettings();
            }
            catch (Exception)
            {
                Console.WriteLine("appsettings.json is not exists");
                Console.ReadKey();
                return;
            }
            

            IFileManager fileManager = new FileManager(logger: config.logger);

            if (string.IsNullOrEmpty(config.properties.targetDir))
            {
                config.logger.Fatal("string of target dir is empty");
                return;
            }
                config.properties.sourceDir.ForEach(action: sourceDir =>
                 {
                     try
                     {
                         if (string.IsNullOrEmpty(sourceDir))
                         {
                             throw new ArgumentNullException(nameof(sourceDir));
                         }
                         fileManager.Process(sourceDir, config.properties.targetDir);
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