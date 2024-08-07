using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
using UnityEngine.Networking;
using UnityLibrary.Runtime.Network;

public class TokenRequester : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField][TextArea] private string _token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE3MjI5NzY3NjYsImlzcyI6ImRldmtleSIsIm5iZiI6MCwic3ViIjoiUGFydGljaXBhbnQ2IiwibmFtZSI6IlBhcnRpY2lwYW50NiIsInZpZGVvIjp7InJvb20iOiJUZXN0IFJvb20iLCJyb29tSm9pbiI6dHJ1ZX19.3vE-N8wtPiHOAAV1gxZxzd-93AhyU9hwyijASO2wzxs&auto_subscribe=1&sdk=js&version=2.1.5&protocol=13";
    [SerializeField] private bool _local = default;

    [Header("References")]
    [SerializeField] private RoomUser _roomUser = default;

    [ContextMenu(nameof(RequestToken))]
    private async void RequestToken()
    {
        RoomUserModel user = new RoomUserModel();
        user.RoomName = "HevolusRoom";
        user.ParticipantName = _roomUser.GetParticipantName();

        Debug.Log($"[TokenRequester] - [RequestToken] ~ Contacting Server By {user.ParticipantName} In Room {user.RoomName}.");

        StringContent stringContent = APIUtility.CreateStringContent(user);
        HttpResponseMessage response = await APIUtility.PostToEndpoint(stringContent, _local ? APIEndpoint.LOCAL_TOKEN_URL : APIEndpoint.LAN_TOKEN_URL);

        if (response == null)
        {
            Debug.Log($"[TokenRequester] - [RequestToken] ~ Response is not valid: {response.StatusCode}-{response.ReasonPhrase}.");
            return;
        }

        if (!response.IsSuccessStatusCode)
        {
            Debug.Log($"[TokenRequester] - [RequestToken] ~ Response is not successful: {response.StatusCode}-{response.ReasonPhrase}.");
            return;
        }

        string content = await response.Content.ReadAsStringAsync();
        Debug.Log($"[TokenRequester] - [RequestToken] ~ String Content Received: {content}.");

        if (string.IsNullOrEmpty(content))
        {
            Debug.Log($"[TokenRequester] - [RequestToken] ~ Received Content Is Null Or Empty: {content}.");
            return;
        }

        RoomUserToken token = JsonConvert.DeserializeObject<RoomUserToken>(content);
        Debug.Log($"[TokenRequester] - [RequestToken] ~ Received Token Identity: {token.Identity}.");
    }

    [ContextMenu(nameof(CO_RequestToken))]
    private void CO_RequestToken()
    {
        RoomUserModel user = new RoomUserModel();
        user.RoomName = "HevolusRoom";
        user.ParticipantName = _roomUser.GetParticipantName();

        Debug.Log($"[TokenRequester] - [RequestToken] ~ Contacting Server By {user.ParticipantName} In Room {user.RoomName}.");

        string stringContent = APIUtility.CreateJsonContent(user);
        NetworkRequest<string> request = NetworkController.CreateNetworkRequest(new Uri(_local ? APIEndpoint.LOCAL_TOKEN_URL : APIEndpoint.LAN_TOKEN_URL), NetworkRequestType.POST, stringContent, new KeyValuePair<string, string>(APIHeader.CONTENT_TYPE, APIHeader.APPLICATION_JSON));

        StartCoroutine(NetworkController.NetworkRequest(request, ResponseToken));
    }

    private void ResponseToken(UnityWebRequest request)
    {
        NetworkResponse<UserToken> response = NetworkController.CreateNetworkResponse<UserToken>(request, false);

        if (response == null)
        {
            Debug.Log($"[TokenRequester] - [RequestToken] ~ Response is not valid.");
            return;
        }

        if (response.Type != NetworkResponseType.SUCCESS)
        {
            NetworkServiceStatus status = NetworkController.GetNetworkServiceStatus(request.result);
            Debug.Log($"[TokenRequester] - [RequestToken] ~ Response is not successful: {NetworkController.GetNetworkServiceStatusAdvice(status)}.");
            return;
        }

        Debug.Log($"[TokenRequester] - [RequestToken] ~ Response is successful.");

        if (response.Content == null)
        {
            Debug.Log($"[TokenRequester] - [RequestToken] ~ Received Content Is Null Or Empty.");
            return;
        }

        if (string.IsNullOrEmpty(response.Content.Token))
        {
            Debug.Log($"[TokenRequester] - [RequestToken] ~ Received Token Is Null Or Empty.");
            return;
        }

        Debug.Log($"[TokenRequester] - [RequestToken] ~ Received Token Identity: {response.Content.Token}.");
    }

}