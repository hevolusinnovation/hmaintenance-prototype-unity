using UnityEngine;
using System.Diagnostics;

public class SystemMonitor : MonoBehaviour
{
    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;
        GUIStyle style = new GUIStyle();
        Rect rect = new Rect(w*2/100, h * 4 / 100, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = Color.white;

        string text = string.Format("CPU: {0:0.0} %\nMemory: {1:0.0} MB", GetCPUUsage(), GetMemoryUsage());
        GUI.Label(rect, text, style);
    }

    float GetCPUUsage()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using (AndroidJavaObject activity = new AndroidJavaObject("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject context = activity.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (AndroidJavaObject activityManager = context.Call<AndroidJavaObject>("getSystemService", "activity"))
                    {
                        using (AndroidJavaObject cpuInfo = activityManager.Call<AndroidJavaObject>("getProcessCpuInfo"))
                        {
                            int totalCpuUsage = cpuInfo.Call<int>("getTotalCpuUsage");
                            return totalCpuUsage;
                        }
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError("Failed to get CPU usage: " + e.Message);
            return 0.0f;
        }
#else
        return 0.0f;
#endif
    }

    float GetMemoryUsage()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using (AndroidJavaObject activity = new AndroidJavaObject("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject context = activity.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (AndroidJavaObject activityManager = context.Call<AndroidJavaObject>("getSystemService", "activity"))
                    {
                        using (AndroidJavaObject memoryInfo = new AndroidJavaObject("android.app.ActivityManager$MemoryInfo"))
                        {
                            activityManager.Call("getMemoryInfo", memoryInfo);
                            long totalMemory = memoryInfo.Get<long>("totalMem");
                            long availableMemory = memoryInfo.Get<long>("availMem");
                            long usedMemory = totalMemory - availableMemory;
                            return usedMemory / (1024 * 1024); // Convert to MB
                        }
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError("Failed to get memory usage: " + e.Message);
            return 0.0f;
        }
#else
        return 0.0f;
#endif
    }
}
