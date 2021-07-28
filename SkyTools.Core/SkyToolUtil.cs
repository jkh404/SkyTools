using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SkyTools.Core
{
    public  static class SkyToolUtil
    {
        public  static bool IsMatch(this string dataText, string regexText)
            =>new Regex(regexText).IsMatch(dataText);
        public static bool IsDir(this string dataText)
        {
            return File.GetAttributes(dataText).CompareTo(FileAttributes.Directory)==0;
        }

        
    }
}
