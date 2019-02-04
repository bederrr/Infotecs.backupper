using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstTask
{
    public class Properties
    {
        public List<string> sourceDir { get; set; }
        public string targetDir { get; set; }
        public string debugLevel { get; set; }

        public Properties(List<string> sourceDir, string targetDir, string debugLevel)
        {
            this.sourceDir = sourceDir;
            this.targetDir = targetDir;
            this.debugLevel = debugLevel;
        }
    }
}
