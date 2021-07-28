using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SkyTools.Core
{
    /// <summary>
    /// SkyTool工具集
    /// </summary>
    public static class SkyToolUtil
    {
        /// <summary>
        /// 是否匹配
        /// </summary>
        /// <param name="dataText">参与匹配的字符串数据</param>
        /// <param name="regexText">正则表达式</param>
        /// <returns></returns>
        public  static bool IsMatch(this string dataText, string regexText)
            =>new Regex(regexText).IsMatch(dataText);
        /// <summary>
        /// 字符串是否是文件夹路径
        /// </summary>
        /// <param name="dataText">字符串</param>
        /// <returns></returns>
        public static bool IsDir(this string dataText)
        {
            return File.GetAttributes(dataText).CompareTo(FileAttributes.Directory)==0;
        }

        
    }
}
