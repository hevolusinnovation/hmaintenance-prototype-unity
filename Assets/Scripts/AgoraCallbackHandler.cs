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

}

public class AgoraCallbackHandler : IRtcEngineEventHandler
{

    public static event Action<UserCallbackEventArgs> RemoteUserJoined;
    public static event Action<UserCallbackEventArgs> remoteUserLeft;

    public static event Action<UserCallbackEventArgs> LocalUserJoined;
    public static event Action localUserLeft;

    public override void OnUserJoined(RtcConnection connection, uint uid, int elapsed)
    {
        Debug.Log("OnUserJoined callback");
        //todo check params 

        UserCallbackEventArgs userArgs = new UserCallbackEventArgs
        {
            connection = connection,
            uid = uid,
            elapsed = elapsed,
        };

        RemoteUserJoined?.Invoke(userArgs); 

       
    }

    public override void OnJoinChannelSuccess(RtcConnection connection, int elapsed)
    {
        Debug.Log("OnJoinChannel callback");

        UserCallbackEventArgs userArgs = new UserCallbackEventArgs
        {
            connection = connection,
            uid = 0,
            elapsed = elapsed,
        };

        LocalUserJoined?.Invoke(userArgs);
        
    }

}
