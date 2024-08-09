using Agora.Rtc;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct UserCallbackEventArgs
{
    public RtcConnection connection { get; set; }
    public uint uid { get; set; }
    public int elapsed {  get; set; }
    public USER_OFFLINE_REASON_TYPE reason { get; set; }   

}

public class AgoraCallbackHandler : IRtcEngineEventHandler
{

    public static event Action<UserCallbackEventArgs> OnRemoteUserJoined;
    public static event Action<UserCallbackEventArgs> OnRemoteUserLeft;

    public static event Action<UserCallbackEventArgs> OnLocalUserJoined;

    public static event Action<LocalVideoStats> OnLocalVideoStatsEvent;
    public static event Action<RemoteVideoStats> OnRemoteVideoStatsEvent;

    public static event Action localUserLeft;

    public override void OnUserJoined(RtcConnection connection, uint uid, int elapsed)
    {
        Debug.Log($"OnUserJoined callback remote user id {uid}");
        //todo check params 

        UserCallbackEventArgs userArgs = new UserCallbackEventArgs
        {
            connection = connection,
            uid = uid,
            elapsed = elapsed,
        };

        OnRemoteUserJoined?.Invoke(userArgs); 

       
    }

    public override void OnJoinChannelSuccess(RtcConnection connection, int elapsed)
    {
        Debug.Log($"OnJoinChannel local user callback");

        UserCallbackEventArgs userArgs = new UserCallbackEventArgs
        {
            connection = connection,
            uid = 0,
            elapsed = elapsed,
        };

        OnLocalUserJoined?.Invoke(userArgs);
        
    }

    // This callback is triggered when a remote user leaves the current channel
    public override void OnUserOffline(RtcConnection connection, uint uid, USER_OFFLINE_REASON_TYPE reason)
    {
        Debug.Log("OnUserOffline callback");
        //todo check params 

        UserCallbackEventArgs userArgs = new UserCallbackEventArgs
        {
            connection = connection,
            uid = uid,
            reason = reason,
        };

        OnRemoteUserLeft?.Invoke(userArgs);
    }

    public override void OnRtcStats(RtcConnection connection, RtcStats stats)
    {

    }

    public override void OnNetworkQuality(RtcConnection connection, uint remoteUid, int txQuality, int rxQuality)
    {
    }

    public override void OnLocalVideoStats(RtcConnection connection, LocalVideoStats stats)
    {
        Debug.Log("Local video stats callback for user " + connection.localUid);
        OnLocalVideoStatsEvent?.Invoke(stats);
    }

    public override void OnRemoteVideoStats(RtcConnection connection, RemoteVideoStats stats)
    {
        Debug.Log("Remote video stats callback for user " + connection.localUid);

        OnRemoteVideoStatsEvent?.Invoke(stats);

    }
}
