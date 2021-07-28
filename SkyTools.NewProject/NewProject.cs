using Masuit.Tools.Files;
using SkyTools.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SkyTools.NewProject
{
    public class NewProject : ISkyTool
    {

        public string Name { get; init; } = "获取本地项目模板";
        public string Info { get; init; } = "可通过命令行的快捷方式获取本地的项目模板";
        public List<KeyValuePair<string, string>> keyValues =null;
        private string BasePath= $"{AppContext.BaseDirectory}Tools\\NewProject";
        public void DoSome(string cmdText, SkyToolMain toolMain)
        {
            List<string> args=cmdText.Split(" ").ToList();
            if (args.Count <= 0) return;
            if (args[0].IsMatch("New|new") && args.Count==2)
            {
                var data = keyValues.Find(u => u.Key == args[1]);
                string path= data.Value;
                
                if (path != null && path.Length>0) {
                    Console.WriteLine($"匹配到的压缩包{path}");
                    if (!File.Exists(path))
                    {
                        Console.WriteLine($"压缩包文件{path}不存在");
                        bool isOk= keyValues.Remove(data);
                        Console.WriteLine($"移除无效数据{(isOk?"成功":"失败")}");
                    }
                    else
                    {
                        SevenZipCompressor zipCompressor = new SevenZipCompressor(null);
                        Console.WriteLine($"正在解压");
                        zipCompressor.Decompress(path, $"{Directory.GetCurrentDirectory()}\\");
                        Console.WriteLine("解压成功");
                    }
                }
            }
            else if(args[0].IsMatch("Push|push") &&args.Count == 3)
            {
                try
                {
                    string copyFile = $"{BasePath}\\{Path.GetFileName(args[1])}";
                    if (!Directory.Exists(BasePath)) Directory.CreateDirectory(BasePath);
                    if (File.Exists(copyFile))
                    {
                        copyFile = $"{BasePath}\\{DateTime.Now.ToString("yyyyMMdd_HHmmss")}_{DateTime.Now.Millisecond}_{Path.GetFileName(args[1])}";
                        File.Copy(args[1], copyFile);
                    }
                    else
                    {
                        File.Copy(args[1], copyFile);
                    }
                    keyValues.Add(new(args[2], copyFile));
                    Console.WriteLine("添加成功");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"添加失败,err{ex.Message}");
                }

            }
            else if(args[0].IsMatch("Set|set") && args.Count == 2)
            {
                if (args[1].IsDir())
                {
                    toolMain.AddData($"{Name}.BasePath", args[1]);
                    BasePath = args[1];
                }
                else
                {
                    Console.WriteLine($"{args[1]}不是一个文件夹");
                }
                Console.WriteLine(toolMain.AddData(Name, keyValues));
            }
            else if(args[0].IsMatch("newlist") && args.Count == 1)
            {
                keyValues.ForEach(data=> {
                    Console.WriteLine($"名字:{data.Key}\t\t路径:{data.Value}");
                });
            }
        }
        public string Register()=> "New|new|Push|push|Set|set|newlist";

        public void Init(SkyToolMain toolMain)
        {
            keyValues =toolMain.GetData<List<KeyValuePair<string, string>>>(Name);
            string path = toolMain.GetData<string>($"{Name}.BasePath");
            if (path!=null)
            {
                BasePath = path;
            }
            if (keyValues==null)
            {
                keyValues = new List<KeyValuePair<string, string>>();
            }
        }

        public void Stop(SkyToolMain toolMain)
        {
            Console.WriteLine(toolMain.AddData(Name, keyValues));
        }
    }
}
