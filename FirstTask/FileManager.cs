using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Serilog;
using Microsoft.Extensions.DependencyInjection;

namespace FirstTask
{
    public class FileManager : IFileManager
    {
        private readonly ILogger _logger;
        public IServiceProvider services { get; set; }

        public FileManager(ILogger logger, IServiceCollection serviceCollection)
        {
            _logger = logger;
            ConfigureServices(serviceCollection);
        }

        public void Copy(string sDir, string tDir)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(tDir);
            if (!dirInfo.Exists)
            {
                    dirInfo.Create();
                    _logger.Debug("directory " + tDir + " created successfully");
            }

            dirInfo = new DirectoryInfo(sDir);
            foreach (FileInfo file in dirInfo.GetFiles("*.*"))
            {
                try
                {
                    File.Copy(file.FullName, Path.Combine(tDir, file.Name), true);
                    _logger.Debug("File copy " + file.FullName + " successfully completed");
                }
/*
                catch(Exception ex) when (
                ex is UnauthorizedAccessException ||
                ex is ArgumentException ||
                ex is ArgumentNullException ||
                ex is PathTooLongException ||
                ex is DirectoryNotFoundException ||
                ex is FileNotFoundException ||
                ex is IOException ||
                ex is NotSupportedException)
                {
                    _logger.Error("File copy " + file.FullName + " failed. " + ex.Message);
                }
                дичь
                */
                catch(Exception ex)
                {
                    throw ex;
                }
            }

            foreach (var dir in Directory.GetDirectories(sDir))
            {
                Copy(dir, tDir + "\\" + Path.GetFileName(dir));
            }
        }

        public void CreateCurrentDirectory(string sourceDirectoy, string targetDirectory)
        {
            var dirInfo = new DirectoryInfo(targetDirectory);

            if (!dirInfo.Exists)
            {
                _logger.Information(targetDirectory + " is not exists");
                try
                {
                    dirInfo.Create();
                    _logger.Information("directory " + targetDirectory + " created successfully");
                }
                catch(Exception e)
                {
                    throw e;
                }
            }

            string currentFolder = Path.GetFileName(sourceDirectoy) + DateTime.Now.ToString(" [yyyy-M-dd-H-mm]");
            _logger.Debug("Start backup. Directory name: " + currentFolder);


            dirInfo.CreateSubdirectory(currentFolder);
            _logger.Information("subbdirectory " + currentFolder + " created successfully");


            try
            {
                Copy(sourceDirectoy, targetDirectory + "\\" + currentFolder);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IPaymentService, PaymentService>();
        }
    }
}
