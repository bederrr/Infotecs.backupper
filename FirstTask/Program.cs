using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;

namespace FirstTask
{
    class Program
    {
        static void Main(string[] args)
        {
            Config config;
            try
            {
                config = new ConfigReader().ReadConfig();
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