using System;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
using UnityEngine.Networking;
using UnityLibrary.Runtime.Network;

public class TokenRequester : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private bool _local = default;

    [Header("References")]
    [SerializeField] private ClientAccessPanel _clientAccessPanel = default;

    public Action<UserToken> OnUserTokenReceived = default;

    private void Awake()
    {
        if(_clientAccessPanel != null)
        {
            _clientAccessPanel.OnJoinRoomAuthentication += RequestToken;
        }
    }
    private void OnDestroy()
    {
        if (_clientAccessPanel != null)
        {
            _clientAccessPanel.OnJoinRoomAuthentication -= RequestToken;
        }
    }

    [ContextMenu(nameof(RequestToken))]
    public async void RequestToken(RoomUserAuthentication roomUser)
    {
        Debug.Log($"[TokenRequester] - [RequestToken] ~ Contacting Server By {roomUser.ParticipantName} In Room {roomUser.RoomName}.");

        StringContent stringContent = APIUtility.CreateStringContent(roomUser);
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

        UserToken userToken = new UserToken() { Token = content };
        Debug.Log($"[TokenRequester] - [RequestToken] ~ Received Token: {userToken.Token}.");

        OnUserTokenReceived?.Invoke(userToken);
    }

    [ContextMenu(nameof(CO_RequestToken))]
    public void CO_RequestToken(RoomUserAuthentication roomUser)
    {
        Debug.Log($"[TokenRequester] - [RequestToken] ~ Contacting Server By {roomUser.ParticipantName} In Room {roomUser.RoomName}.");

        string stringContent = APIUtility.CreateJsonContent(roomUser);
        NetworkRequest<string> request = NetworkController.CreateNetworkRequest(new Uri(_local ? APIEndpoint.LOCAL_TOKEN_URL : APIEndpoint.LAN_TOKEN_URL), NetworkRequestType.POST, stringContent, new KeyValuePair<string, string>(APIHeader.CONTENT_TYPE, APIHeader.APPLICATION_JSON));

        StartCoroutine(NetworkController.NetworkRequest(request, ResponseToken));
    }

    [ContextMenu(nameof(CO_Simple_RequestToken))]
    public void CO_Simple_RequestToken(RoomUserAuthentication roomUser)
    {
        Debug.Log($"[TokenRequester] - [RequestToken] ~ Contacting Server By {roomUser.ParticipantName} In Room {roomUser.RoomName}.");

        StartCoroutine(NetworkController.SendWebRequest(new Uri(_local ? APIEndpoint.LOCAL_TOKEN_URL : APIEndpoint.LAN_TOKEN_URL), NetworkRequestType.POST, roomUser, ResponseToken));
    }

    private void ResponseToken(UnityWebRequest request)
    {
        NetworkResponse<UserToken> response = NetworkController.CreateNetworkResponse<UserToken>(request, true);

        if (response == null)
        {
            Debug.Log($"[TokenRequester] - [ResponseToken] ~ Response is not valid.");
            return;
        }

        if (response.Type != NetworkResponseType.SUCCESS)
        {
            NetworkServiceStatus status = NetworkController.GetNetworkServiceStatus(request.result);
            Debug.Log($"[TokenRequester] - [ResponseToken] ~ Response is not successful: {NetworkController.GetNetworkServiceStatusAdvice(status)}.");
            return;
        }

        Debug.Log($"[TokenRequester] - [ResponseToken] ~ Response is successful.");

        if (response.Content == null)
        {
            Debug.Log($"[TokenRequester] - [ResponseToken] ~ Received Content Is Null Or Empty.");
            return;
        }

        if (string.IsNullOrEmpty(response.Content.Token))
        {
            Debug.Log($"[TokenRequester] - [ResponseToken] ~ Received Token Is Null Or Empty.");
            return;
        }

        Debug.Log($"[TokenRequester] - [ResponseToken] ~ Received Token Identity: {response.Content.Token}.");
    }

}