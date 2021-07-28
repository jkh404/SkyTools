using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SkyTools.Core
{
    public class SkyToolMain
    {
        private  List<ISkyTool> skyTools = new List<ISkyTool>();
        private  List<(string filter, Action<string, SkyToolMain> doSome)> Regs
            = new List<(string filter, Action<string, SkyToolMain> doSome)>();
        private  List<(string text, Action doSome)> Menus = new List<(string, Action)>();
        public bool IsRun = true;
        public string ToolsPath { get; set; }
            = $"{AppContext.BaseDirectory}Tools\\";
        /// <summary>
        /// 通过名字获取数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="name">数据名</param>
        /// <returns>数据</returns>
        public T GetData<T>(string name)
        {
            if(!File.Exists($"{AppContext.BaseDirectory}Data\\{name}.json")) return default(T);
            StreamReader streamReader = new StreamReader($"{AppContext.BaseDirectory}Data\\{name}.json");
            if (streamReader != null)
            {
                string json = streamReader.ReadToEnd();
                streamReader.Dispose();
                return JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                return default(T);
            }
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="name">数据名</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public string AddData(string name,object data)
        {
            try
            {
                if (!Directory.Exists($"{AppContext.BaseDirectory}Data\\")) 
                    Directory.CreateDirectory($"{AppContext.BaseDirectory}Data\\");
                File.WriteAllText($"{AppContext.BaseDirectory}Data\\{name}.json", JsonConvert.SerializeObject(data));
                return "数据保存成功";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// 启动插件主程序
        /// </summary>
        /// <param name="args">命令行参数</param>
        public void Start(string[] args)
        {

            Console.WriteLine($"当前程序目录:{AppContext.BaseDirectory}");
            if (args.Length>0)
            {
                LoadTools();
                DoTool(string.Join(" ",args));
            }
            else
            {
                LoadTools();
                LoadMenus();
                ShowMenus();
                ListeningInput();
            }
        }
        /// <summary>
        /// 启动插件主程序
        /// </summary>
        public void Start( )
        {
            
            Console.WriteLine($"当前程序目录:{AppContext.BaseDirectory}");
            if (Environment.GetCommandLineArgs().Length>0)
            {
                LoadTools();
                DoTool(string.Join(" ", Environment.GetCommandLineArgs()));
            }
            else
            {
                LoadTools();
                LoadMenus();
                ShowMenus();
                ListeningInput();
            }


        }
        /// <summary>
        /// 执行插件操作
        /// </summary>
        /// <param name="inputText">命令行输入</param>
        /// <returns></returns>
        private int DoTool(string inputText)
        {
            int num = 0;
            List<string> args = inputText.Split(" ").ToList();
            if (args.Count > 0)
            {
                
                Regs.ForEach((obj) => {
                    Regex regex = new Regex(obj.filter);
                    if (regex.IsMatch(args[0]))
                    {
                        num++;
                        Console.WriteLine("正在执行。。。");
                        string cmdText = string.Join(" ", args);
                        Console.WriteLine($"参数:{cmdText}");
                        obj.doSome(cmdText, this);
                        Console.WriteLine("执行结束");
                    }
                });
            }
            return num;
        }
        /// <summary>
        /// 监听控制台输入
        /// </summary>
        private  void ListeningInput()
        {
            do
            {
                Console.Write("请输入关键字或菜单编号:");
                string inputText = Console.ReadLine();
                int index = -1;
                int.TryParse(inputText, out index);
                if (index >0 && index <= Menus.Count)
                {
                    Menus[index-1].doSome();
                    Console.WriteLine();
                }
                else 
                {
                    int count=DoTool(inputText);
                    if (count == 0) Console.WriteLine("没有匹配的关键字");
                }
            } while (IsRun);
        }
        /// <summary>
        /// 加载菜单
        /// </summary>
        private  void LoadMenus()
        {
            Menus.Add(("查看插件列表", () => {
                Console.WriteLine("插件列表：\n");
                int index = 0;
                skyTools.ForEach(tool => {
                    Console.WriteLine($"{++index}——{tool.Name}\t\t关键字：{tool.Register()}");
                });
            }
            ));
            Menus.Add(("查看插件详细信息", () => {
                Console.WriteLine("插件详细信息：\n");
                skyTools.ForEach(tool => {
                    Console.WriteLine($"插件名：{tool.Name}\n插件简介：{tool.Info}\r\n");
                });
            }
            ));
            Menus.Add(("退出", () => {
                IsRun = false;
            }));
        }
        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="menu">text:菜单文本，doSome 输入菜单编号后执行的操作</param>
        public void AddMenu((string text, Action doSome) menu)
        {
            Menus.Add(menu);
        }
        /// <summary>
        /// 加载插件
        /// </summary>
        private  void LoadTools()
        {
            int ErrCount = 0;
            if (!Directory.Exists(ToolsPath)) Directory.CreateDirectory(ToolsPath);
            List<string> dllFiles = Directory.GetFiles(ToolsPath, "*.dll").ToList();
            Console.WriteLine($"扫描到插件{dllFiles.Count}个");
            Console.WriteLine("加载插件中。。。");
            dllFiles.ForEach(file => {
                Assembly assem = Assembly.LoadFile(file);
                Type[] types = null;
                try
                {
                    types = assem.GetTypes();
                    foreach (Type type in types)
                    {
                        foreach (var item in type.GetInterfaces())
                        {
                            if (item == typeof(ISkyTool))
                            {
                                ISkyTool tool = (ISkyTool)type.GetConstructors().Single().Invoke(new object[0]);
                                tool.Init(this);
                                Regs.Add((tool.Register(), tool.DoSome));
                                skyTools.Add(tool);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ++ErrCount;
                    Console.WriteLine($"err:{ex.Message}");
                }

            });
            Console.WriteLine($"加载完成!成功{skyTools.Count}个，失败{ErrCount}个");
        }
        /// <summary>
        /// 显示菜单
        /// </summary>
        private  void ShowMenus()
        {
            Console.WriteLine("\r\n");
            Console.WriteLine("菜单界面");
            for (int i = 0; i < Menus.Count; i += 2)
            {
                if (Menus.Count - i > 1)
                {
                    Console.Write($"{i+1}、{Menus[i].text}\t\t{i + 2}、{Menus[i + 1].text}\n");
                }
                else
                {
                    Console.Write($"{i+1}、{Menus[i].text}\n");
                }
            }
            Console.WriteLine("\r\n");
        }
        /// <summary>
        /// 停止插件
        /// </summary>
        public void StopTools()
        {
            skyTools.ForEach(tool=> {
                tool.Stop(this);
            });
        }
    }
}
