using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FirstTask
{
    public class FileManager : IFileManager
    {
        private readonly ILogger _logger;

        public FileManager(ILogger<FileManager> logger)
        {
            _logger = logger;
        }

        public void CopyTo(string sDir, string tDir)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(tDir);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            dirInfo = new DirectoryInfo(sDir);
            foreach (FileInfo file in dirInfo.GetFiles("*.*"))
            {
                File.Copy(file.FullName, Path.Combine(tDir, file.Name), true);
            }

            foreach (var dir in Directory.GetDirectories(sDir))
            {
                CopyTo(dir, tDir + "\\" + Path.GetFileName(dir));
            }
        }

        public void Process(string sourceDirectoy, string targetDirectory)
        {
            var dirInfo = new DirectoryInfo(targetDirectory);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            string currentFolder = Path.GetFileName(sourceDirectoy) + DateTime.Now.ToString(" [yyyy-M-dd-H-mm]");

            dirInfo.CreateSubdirectory(currentFolder);

            CopyTo(sourceDirectoy, targetDirectory + "\\" + currentFolder);
        }
    }
}
