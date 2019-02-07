using System;
using System.IO;
using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace FirstTask
{
    class Program
    {
        private static IServiceCollection _services { get; set; }
        private static IServiceProvider _provider { get; set; }
        private static IConfiguration _configuration { get; set; }

        private static IConfiguration ReadConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();
        }

        private static LoggerConfiguration CreateLoggerConfiguration()
        {
            return new LoggerConfiguration()
                .ReadFrom.Configuration(_configuration)
                .WriteTo.File("logs\\" + DateTime.Now.ToString("[yyyy-M-dd-H-mm]") + ".txt", rollingInterval: RollingInterval.Infinite);
        }

        private static void ConfigureServices(IServiceCollection services, LoggerConfiguration loggerConfiguration)
        {
            Log.Logger = loggerConfiguration.CreateLogger();
            AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();
            services.AddSingleton(Log.Logger);

            services.AddTransient<IFileManager, FileManager>();
        }

        static void Main(string[] args)
        {
            try
            {
                _configuration = ReadConfiguration();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to read configuration. " + ex.Message);
                Console.ReadKey();
            }

            var loggerConfiguration = CreateLoggerConfiguration();

            _services = new ServiceCollection();
            ConfigureServices(_services, loggerConfiguration);
            _provider = _services.BuildServiceProvider();

            IFileManager fileManager = _provider.GetRequiredService<IFileManager>();

            _configuration.GetSection("sourceDir").Get<List<string>>().ForEach(action: sourceDir =>
                 {
                     try
                     {
                         if (string.IsNullOrEmpty(sourceDir))
                         {
                             throw new ArgumentNullException(nameof(sourceDir));
                         }
                         fileManager.CreateCurrentDirectory(sourceDir, _configuration["targetDir"].ToString());
                     }

                     catch (ArgumentNullException)
                     {
                         Log.Logger.Error("String {0} is null or empty", sourceDir);
                     }
                     catch (Exception ex)
                     {

                         Log.Logger.Fatal("The process failed: {0}", ex.Message);
                     }
                 });
        }
    }
}