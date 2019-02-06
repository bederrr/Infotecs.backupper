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
using Microsoft.Extensions.DependencyInjection;

namespace FirstTask
{
    class Program
    {
        //private static IConfiguration _configuration;
        //IServiceCollection services;

        //public IServiceProvider ConfigureServices(IServiceCollection services)
        //{
        //    services.AddTransient<IFileManager, FileManager>();

        //    var containerBuilder = new ContainerBuilder();
        //    containerBuilder.RegisterModule<DefaultModule>();
        //    containerBuilder.Populate(services);
        //    var container = containerBuilder.Build();
        //    return new AutofacServiceProvider(container);
        //}

        //private static IConfiguration GetConfiguration()
        //{
        //    return new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
        //        .AddJsonFile("appsettings.json", false)
        //        .Build();
        //}

        //static void Main(string[] args)
        //{
        //    try
        //    {
        //        _configuration = GetConfiguration();
        //    }

        //    catch (Exception e)
        //    {
        //        Console.WriteLine("appsettings.json is not exists. ", e.Message);
        //        Console.ReadKey();
        //        return;
        //    }

        //    IFileManager fileManager = new FileManager(logger: config.logger);

        //    if (string.IsNullOrEmpty(_configuration["targetDir"].ToString()))
        //    {
        //        config.logger.Fatal("string of target dir is empty");
        //        return;
        //    }

        //    _configuration.GetSection("sourceDir").Get<List<string>>().ForEach(action: sourceDir =>
        //         {
        //             try
        //             {
        //                 if (string.IsNullOrEmpty(sourceDir))
        //                 {
        //                     throw new ArgumentNullException(nameof(sourceDir));
        //                 }
        //                 fileManager.CreateCurrentDirectory(sourceDir, _configuration["targetDir"].ToString());
        //             }

        //             catch (ArgumentNullException)
        //             {
        //                 config.logger.Error("String {0} is null or empty", sourceDir);
        //             }
        //             catch (Exception e)
        //             {
        //                 config.logger.Fatal("The process failed: {0}", e.Message);
        //             }
        //         });
        //}

        private static readonly IFileManager _filemanager;

        static void Main(string[] args)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            IFileManager application = new FileManager(null, serviceCollection);
        }

        static private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IFileManager, FileManager>();
        }


    }
}