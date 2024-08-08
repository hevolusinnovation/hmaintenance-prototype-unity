using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[Serializable]
public class UserModel
{
    public string roomName;
    public string participantName;

}
public class OpenViduClientTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {


        StartCoroutine(SendRequest());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    IEnumerator SendRequest()
    {
        var user = new UserModel();
        user.roomName = "Salotto";
        user.participantName = "Caloggero";
        string json = JsonUtility.ToJson(user);

        using (UnityWebRequest www = UnityWebRequest.Post("localhost:6080/token", json, "application/json"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("OK!");
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
}
