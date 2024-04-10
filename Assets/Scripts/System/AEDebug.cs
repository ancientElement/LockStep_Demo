#define DEBUGMODE
namespace LockStep_Demo
{
    public static class AEDebug
    {
        public static void Log(object msg)
        {
            Log(msg.ToString());
        }

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