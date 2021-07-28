using SkyTools.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
namespace SkyTools
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"当前工作目录:{Directory.GetCurrentDirectory()}");
            SkyToolMain toolMain = new SkyToolMain();
            toolMain.Start(args);
            toolMain.StopTools();
        }
    }
}
