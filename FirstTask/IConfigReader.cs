﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace FirstTask
{
    public interface IConfigReader
    {
        Config ReadConfig();
    }
}