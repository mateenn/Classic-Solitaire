using UnityEngine;

namespace TheSyedMateen.ClassicSolitaire
{
    public static class Helper
    {

        #region Logs Reigon

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void Log(string value)
        {
            Debug.Log(value);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void Log(string value, GameObject obj)
        {
            Debug.Log(value, obj);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogError(string value)
        {
            Debug.LogError(value);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogError(string value, GameObject obj)
        {
            Debug.LogError(value, obj);
        }

        #endregion

    }
}