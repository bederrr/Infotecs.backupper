using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Core;
using System.IO;
using Newtonsoft.Json;

namespace FirstTask
{
    class ConfigReader : IConfigReader
    {
        public Config ReadConfig()
        {
            var config = new Config();

            var logLevelSwitch = new LoggingLevelSwitch();

            config.logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(logLevelSwitch)
                .WriteTo.File("logs\\" + DateTime.Now.ToString("[yyyy-M-dd-H-mm]") + ".txt", rollingInterval: RollingInterval.Infinite)
                .CreateLogger();

            if (!File.Exists("appsettings.json"))
            {
                logLevelSwitch.MinimumLevel = Serilog.Events.LogEventLevel.Warning;
                config.logger.Warning("appsettings.json is not exist");
                Log.CloseAndFlush();
                throw new Exception();
            }

            config.properties = JsonConvert.DeserializeObject<jsonProperties>(File.ReadAllText("appsettings.json"));

            switch (config.properties.debugLevel)
            {
                case "Debug":
                    logLevelSwitch.MinimumLevel = Serilog.Events.LogEventLevel.Debug;
                    break;

                case "Error":
                    logLevelSwitch.MinimumLevel = Serilog.Events.LogEventLevel.Error;
                    break;

                case "Information":
                    logLevelSwitch.MinimumLevel = Serilog.Events.LogEventLevel.Information;
                    break;
            }
            config.logger.Information("appsettings.json read successfully");
            return config;
        }
    }
}
