using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix
{
    public class NewDebug
    {
        public async void DebugNew()
        {
            await Task.Delay(1000);
            Debug.LogFormat("hello, HybridCLR. {0}", "新增类");

        }
    }
}
