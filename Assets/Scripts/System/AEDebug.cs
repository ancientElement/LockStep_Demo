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
#if UNITY_EDITOR
            UnityEngine.Debug.Log(msg);
#else
            Console.WriteLine(msg);
#endif
#endif
        }
    }
}