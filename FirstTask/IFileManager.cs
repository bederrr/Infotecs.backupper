using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstTask
{
    public interface IFileManager
    {
        void Process(string a, string b);
        void CopyTo(string a, string b);
    }
}
