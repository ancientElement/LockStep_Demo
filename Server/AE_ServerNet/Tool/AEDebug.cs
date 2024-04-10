#define DEBUGMODE
#define SERVER
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
#if SERVER
            Console.WriteLine(msg);
#else
            UnityEngine.Debug.Log(msg);
#endif
#endif
        }
    }
}