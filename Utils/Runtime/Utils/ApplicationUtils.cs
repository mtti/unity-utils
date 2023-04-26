using System;
using UnityEngine;

namespace mtti.Funcs
{
    public static class ApplicationUtils
    {
        public static void Crash(Exception reason, int exitCode = 1)
        {
            Debug.LogException(reason);
            Debug.LogError($"Application crashing with exit code {exitCode}");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit(exitCode);
#endif
        }
    }
}
