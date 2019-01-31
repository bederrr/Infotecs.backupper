using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace FirstTask
{
    class Program
    {
        public static void Process(string sDir, string tDir)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(tDir);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }

                dirInfo = new DirectoryInfo(sDir);
                foreach (FileInfo file in dirInfo.GetFiles("*.*"))
                {
                    try
                    {
                        File.Copy(file.FullName, Path.Combine(tDir, file.Name), true);
                        int a = 5;
                        a = a / 0;
                        Console.Write("read");
                    }
                    catch (Exception ex) when (ex is PathTooLongException)
                    {
                        Console.WriteLine("file " + file.FullName + " has a long name");
                        //Logger.addlog
                    }

                }

                foreach (var dir in Directory.GetDirectories(sDir))
                {
                    Process(dir, tDir + "\\" + Path.GetFileName(dir));
                }
            }
            catch (Exception)
            {
                //Console.WriteLine(ex.Message);
                throw;
            }
        }     

        static void Mai2n(string[] args)
        {
            if (File.Exists("appsettings.json"))
            {
                var settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText("appsettings.json"));
                ////
                try
                {
                    foreach (var dir in settings.sourceDir)
                    {
                        string dirName = dir;
                        if (dir.EndsWith("\\"))
                        {
                            dirName = dir.Trim('\\');
                        }

                        dirName = Path.GetFileName(dirName);
                        dirName += DateTime.Now.ToString(" [yyyy-M-dd-H-mm]");

                        DirectoryInfo dirInfo = new DirectoryInfo(settings.targetDir);
                        if (!dirInfo.Exists)
                        {
                            dirInfo.Create();
                        }
                        dirInfo.CreateSubdirectory(dirName); 

                        Process(dir, settings.targetDir + "\\" + dirName);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Console.WriteLine("task complete");
                ////
                Console.ReadKey();
            }
            else Console.WriteLine("File appsettings.json is not exists");
        }

        static void Main(string[] args)
        {
            IFileManager fileManager = new FileManager(new ILogger<FileManager>());
        }
    }
}