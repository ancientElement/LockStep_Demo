#define DEBUGMODE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AE_ServerNet
{
    public static class AEDebug
    {
        public static void Log(string msg)
        {
#if DEBUGMODE
#if UNITY_EDITOR
            UnityEngine.Debug.Log(msg);
#else
            Console.WriteLine(msg);
#endif
#endif
        }
    }
}