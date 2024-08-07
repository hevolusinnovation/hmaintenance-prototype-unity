using Agora.Rtc;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CallFrameController : MonoBehaviour
{
    public uint id { get; private set; }
    public string channelId { get; private set; }

    private VIDEO_SOURCE_TYPE currentSource { get; set; }

    [SerializeField] private VideoSurface videoSurface;

    private void OnValidate()
    {
        if (videoSurface == null)
            videoSurface = this.GetComponent<VideoSurface>();
    }


    public void SetFrame(uint id, string channelId, VIDEO_SOURCE_TYPE sourceType = VIDEO_SOURCE_TYPE.VIDEO_SOURCE_CAMERA)
    {
        this.id = id;
        this.channelId = channelId;
        this.currentSource = sourceType;
        videoSurface.SetForUser(id, channelId, sourceType);

    }

    public void SetEnable(bool value)
    {
        videoSurface.SetEnable(value);
        this.gameObject.SetActive(value);
    }

    public void Destroy()
    {
        videoSurface.SetEnable(false);
        Destroy(this.gameObject);
        
    }

    

}
