using SkyTools.Core;
using System;

namespace SkyTools.Template
{
    public class MyTool : ISkyTool
    {
        public string Name { get; init; } = "插件名";
        public string Info { get; init; } = "插件介绍";
        /// <summary>
        /// 执行关键字触发事件
        /// </summary>
        /// <param name="cmdText">命令行参数</param>
        /// <param name="toolMain">插件执行主对象</param>
        public void DoSome(string cmdText, SkyToolMain toolMain)
        {
            Console.WriteLine($"{Name}插件正在执行...");
        }
        /// <summary>
        /// 初始化插件
        /// </summary>
        /// <param name="toolMain">插件执行主对象</param>
        public void Init(SkyToolMain toolMain)
        {
            Console.WriteLine($"{Name}插件初始化...");
        }
        /// <summary>
        /// 注册关键字
        /// </summary>
        /// <returns>用于匹配关键字的正则表达式</returns>
        public string Register()
        {
            return "Test";
        }
        /// <summary>
        /// 停止插件
        /// </summary>
        /// <param name="toolMain">插件执行主对象</param>
        public void Stop(SkyToolMain toolMain)
        {
            Console.WriteLine($"{Name}插件停止...");
        }
    }
}
