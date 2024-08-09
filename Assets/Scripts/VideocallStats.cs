using Agora.Rtc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideocallStats : MonoBehaviour
{

    private string _statsText = "";

    private LocalVideoStats _localStats;
    private RemoteVideoStats _remoteStats;

    [SerializeField] MonitoringTextPresenter monitorPresenter;

    private void OnEnable()
    {
        AgoraCallbackHandler.OnLocalVideoStatsEvent += UpdateLocalVideoStats;
        AgoraCallbackHandler.OnRemoteVideoStatsEvent += UpdateRemoteVideoStats;
    }

    private void OnDisable()
    {
        AgoraCallbackHandler.OnLocalVideoStatsEvent -= UpdateLocalVideoStats;
        AgoraCallbackHandler.OnRemoteVideoStatsEvent -= UpdateRemoteVideoStats;
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;
        //w*2/100, h*2/100, w, h * 2 / 100
        GUI.Label(new Rect(10, 10, 500, 500), _statsText);
    }

    public void UpdateLocalVideoStats(LocalVideoStats stats)
    {
        Debug.Log("Update local video stats");

        if (stats != null)
        {
            _localStats = stats;
        }
    }

    public void UpdateRemoteVideoStats(RemoteVideoStats stats)
    {
        Debug.Log("Update remote video stats");

        if (stats != null)
        {
            _remoteStats = stats;
            monitorPresenter.UpdateVideoStats(_remoteStats, _localStats);
        }
    }


}
