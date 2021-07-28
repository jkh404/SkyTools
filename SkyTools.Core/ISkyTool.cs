using System;
using System.Collections.Generic;

namespace SkyTools.Core
{
    /// <summary>
    /// 插件工具接口
    /// </summary>
    public interface  ISkyTool
    {
        /// <summary>
        /// 插件工具名
        /// </summary>
        public string Name { get;  init; }
        /// <summary>
        /// 插件工具简介
        /// </summary>
        public string Info { get; init; }
        /// <summary>
        /// 插件执行主方法
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="toolMain"></param>
        public void DoSome(string cmdText, SkyToolMain toolMain);
        /// <summary>
        /// 注册关键字
        /// </summary>
        /// <returns></returns>
        public string  Register();
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="toolMain"></param>
        public void Init(SkyToolMain toolMain);
        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="toolMain"></param>
        public void Stop(SkyToolMain toolMain);
    }
}
