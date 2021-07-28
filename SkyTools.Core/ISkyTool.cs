using System;
using System.Collections.Generic;

namespace SkyTools.Core
{
    public interface  ISkyTool
    {
        //delegate void DoSome(string cmdText);
        //delegate bool FilterText(string cmdText);
        public string Name { get;  init; }
        public string Info { get; init; }

        public void DoSome(string cmdText, SkyToolMain toolMain);
        public string  Register();
        public void Init(SkyToolMain toolMain);
        public void Stop(SkyToolMain toolMain);
    }
}
