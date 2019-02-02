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
            Config config = new ConfigReader().ReadConfig();

            IFileManager fileManager = new FileManager(logger: config.logger);

            config.properties.sourceDir.ForEach(action: f =>
             {
                 try
                 {
                     fileManager.Process(f, config.properties.targetDir);
                 }
                 catch (Exception e)
                 {

                 }
             });
            Log.CloseAndFlush();
        }
    }
}