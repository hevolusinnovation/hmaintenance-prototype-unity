using Agora.Rtc;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class VideocallsGridView : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;

    [SerializeField] private CallFrameController _localFrame;
    [SerializeField] private GameObject _framePrefab;

    private void OnValidate()
    {
        if (_gridLayoutGroup == null)
            _gridLayoutGroup = this.GetComponent<GridLayoutGroup>();
    }

    private void Awake()
    {
        if (_framePrefab == null)
        {
            Debug.LogError("frame prefab is null, please assign it in inspector", this);           
        }
    }

    public CallFrameController GetLocalFrame()
    {
        if (_localFrame == null)
        {
            Debug.LogError("Local frame not found, please assign it in inspector", this);
            return null;
        }

        return _localFrame;
    }
    
    public CallFrameController CreateFrame()
    {
        if (_framePrefab == null)
        {
            return null;
        }

        var frame = Instantiate(_framePrefab, this.transform);

        if (frame.TryGetComponent(out CallFrameController frameController))
        {
            return frameController;
        }
        else
        {
            Debug.LogError("frame controller component not found", this);
            return null;
        }
    }
    public void RemoveFrame(uint id)
    {


    }

}
