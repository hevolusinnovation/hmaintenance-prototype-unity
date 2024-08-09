using Agora.Rtc;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonitoringTextPresenter : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _textMeshPro;


    private string title = "Monitoring";
    private string videocallStatsTitle = "VideocallStats";



    private void OnValidate()
    {
        if (_textMeshPro == null)
            _textMeshPro = GetComponent<TextMeshProUGUI>();
    }


    public void UpdateVideoStats(RemoteVideoStats remoteStats, LocalVideoStats localStats)
    {
        string statsText = string.Empty;

        if (localStats != null)
        {
            statsText += $"LOCAL VIDEO STATS\n";
            statsText += $"Resolution: {localStats.encodedFrameWidth}x{localStats.encodedFrameHeight}\n";
            statsText += $"Bitrate: {localStats.sentBitrate} Kbps\n";
            statsText += $"Frame Rate: {localStats.sentFrameRate} fps\n";
            statsText += $"Packet Loss Rate: {localStats.txPacketLossRate}%\n";
        }

        if (remoteStats != null)
        {
            statsText += $"REMOTE VIDEO STATS\n";
            statsText += $"Resolution: {remoteStats.width}x{remoteStats.height}\n";
            statsText += $"UID: {remoteStats.uid}\n";
            statsText += $"Bitrate: {remoteStats.receivedBitrate} Kbps\n";
            statsText += $"Frame Rate Decoder: {remoteStats.decoderOutputFrameRate} fps\n";
            statsText += $"Frame Rate Render: {remoteStats.rendererOutputFrameRate} fps\n";
            statsText += $"Packet Loss Rate: {remoteStats.packetLossRate}%\n";
        }

        _textMeshPro.text = statsText;

    }



}