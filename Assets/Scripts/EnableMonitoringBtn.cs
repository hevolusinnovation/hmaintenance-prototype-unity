using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnableMonitoringBtn : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI buttonText;

    [SerializeField] private GameObject monitoringServices;
    [SerializeField] private GameObject monitoringView;

    private bool current = false;

    private void OnValidate()
    {
        if (buttonText == null)
            buttonText = this.GetComponentInChildren<TextMeshProUGUI>();

    }
    

    public void EnableMonitoring()
    {

        if (!current)
        {
            buttonText.text = "Disable Monitoring";
            
        }
        else
        {
            buttonText.text = "Enable Monitoring";
        }

        monitoringServices.SetActive(!current);
        monitoringView.SetActive(!current);
        current = !current;
    }
}
