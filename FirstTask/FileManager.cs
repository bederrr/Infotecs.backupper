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
        private ILogger _logger;
        public IServiceProvider services { get; set; }

        public FileManager(ILogger logger)
        {
            _logger = logger;
        }

        public void Copy(string sDir, string tDir)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(tDir);
            if (!dirInfo.Exists)
            {
                try
                {
                    dirInfo.Create();
                    _logger.Debug("directory " + tDir + " created successfully");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            dirInfo = new DirectoryInfo(sDir);
            foreach (FileInfo file in dirInfo.GetFiles("*.*"))
            {
                try
                {
                    File.Copy(file.FullName, Path.Combine(tDir, file.Name), true);
                    _logger.Debug("File copy " + file.FullName + " successfully completed");
                }
                catch(Exception ex)
                {
                    _logger.Error("File {0} copying error {1}", file.FullName, ex.Message);
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
            _logger.Information("Start backup. Directory name: " + currentFolder);


            dirInfo.CreateSubdirectory(currentFolder);
            _logger.Information("subbdirectory " + currentFolder + " created successfully");


            try
            {
                Copy(sourceDirectoy, targetDirectory + "\\" + currentFolder);
            }
            catch(Exception ex)
            {
                _logger.Error("Folder copying error " + ex.Message);
            }
        }
    }
}
