public static class Log
{
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Debug(string message, params object[] args)
    {
        WriteLog(message, "white", args);
    }

    public static void Info(string message, params object[] args)
    {
        WriteLog(message, "green", args);
    }

    public static void Warning(string message, params object[] args)
    {
        WriteLog(message, "yellow", args);
    }

    public static void Error(string message, params object[] args)
    {
        WriteLog(message, "red", args);
    }

    public static void WriteLog(string message, string color, params object[] args)
    {
#if UNITY_EDITOR
        UnityEngine.Debug.LogFormat($"<color={color}>[STARTVR] {message}</color>", args);
#else
        UnityEngine.Debug.LogFormat($"[STARTVR] {message}", args);
#endif
    }
}
