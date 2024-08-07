using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace UnityLibrary.Runtime.Network
{

    public static class NetworkController
    {
        public static IEnumerator NetworkRequest<T>(NetworkRequest<T> request, Action<UnityWebRequest> responseCallback, bool jsonConvert = true)
        {
            if (request == null)
                yield return null;

            UnityWebRequest unityRequest = CreateWebRequest(request.Uri, request.Type, request.Content, request.Header, jsonConvert);
            yield return unityRequest.SendWebRequest();

            responseCallback?.Invoke(unityRequest);
        }

        public static UnityWebRequest CreateWebRequest(Uri uri, NetworkRequestType type, object data, bool jsonConvert = true)
        {
            UnityWebRequest request = new UnityWebRequest(uri.AbsoluteUri, type.ToString());

            if (data != null)
            {
                Debug.Log($"Content: {JsonConvert.SerializeObject(data)}");
                byte[] bodyRaw = jsonConvert ? Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)) : Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            }

            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader(APIHeader.CONTENT_TYPE, APIHeader.APPLICATION_JSON);

            return request;
        }

        public static UnityWebRequest CreateWebRequest(Uri uri, NetworkRequestType type, object data, KeyValuePair<string, string> headers, bool jsonConvert = true)
        {
            UnityWebRequest request = new UnityWebRequest(uri.AbsoluteUri, type.ToString());

            if (data != null)
            {
                Debug.Log($"Content: {JsonConvert.SerializeObject(data)}");
                byte[] bodyRaw = jsonConvert ? Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)) : Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            }

            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader(APIHeader.CONTENT_TYPE, APIHeader.APPLICATION_JSON);
            request.SetRequestHeader(headers.Key, headers.Value);

            return request;
        }

        public static UnityWebRequest CreateWebRequest(Uri uri, NetworkRequestType type, object data, Dictionary<string, string> headers, bool jsonConvert = true)
        {
            UnityWebRequest request = new UnityWebRequest(uri.AbsoluteUri, type.ToString());

            if (data != null)
            {
                Debug.Log($"Content: {JsonConvert.SerializeObject(data)}");
                byte[] bodyRaw = jsonConvert ? Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)) : Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            }

            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader(APIHeader.CONTENT_TYPE, APIHeader.APPLICATION_JSON);

            foreach (KeyValuePair<string, string> pair in headers)
            {
                request.SetRequestHeader(pair.Key, pair.Value);
            }

            return request;
        }

        public static NetworkRequest<T> CreateNetworkRequest<T>(Uri uri, NetworkRequestType type, T content, KeyValuePair<string, string> header)
        {
            NetworkRequest<T> request = new NetworkRequest<T>();

            Debug.Log($"Uri: {uri} - Type: {type} - Content: {content} - Header: {header}");

            request.Uri = uri;
            request.Type = type;
            request.Content = content;
            request.Header = header;

            return request;
        }

        public static NetworkResponse<T> CreateNetworkResponse<T>(UnityWebRequest request, bool jsonConvert = true)
        {
            NetworkResponse<T> response = new NetworkResponse<T>();

            if (string.IsNullOrEmpty(request.downloadHandler.text))
            {
                response.Type = NetworkResponseType.ERROR;
                return response;
            }

            byte[] data = request.downloadHandler.data;
            string result = Encoding.UTF8.GetString(data);

            T content = jsonConvert ? JsonConvert.DeserializeObject<T>(result)
             : JsonUtility.FromJson<T>(request.downloadHandler.text);

            if (content == null)
            {
                response.Type = NetworkResponseType.ERROR;
                return response;
            }

            response.Content = content;
            response.Type = NetworkResponseType.SUCCESS;

            return response;
        }

        public static IEnumerator SendWebRequest(Uri uri, NetworkRequestType type, object data, Action<UnityWebRequest> responseCallback)
        {
            UnityWebRequest request = default;

            switch (type)
            {

                case NetworkRequestType.GET:
                    request = UnityWebRequest.Get(uri);
                    break;
                case NetworkRequestType.POST:
                    request = UnityWebRequest.Post(uri, JsonUtility.ToJson(data), "application/json");
                    break;
                case NetworkRequestType.PUT:
                    request = UnityWebRequest.Put(uri, JsonUtility.ToJson(data));
                    break;

            }

            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log($"[NetworkController] - [SendWebRequest] ~ UnityWebRequest Error: {request.error}");
                yield break;
            }

            if(request.isDone)
            {
                Debug.Log($"[NetworkController] - [SendWebRequest] ~ UnityWebRequest Response: {request.responseCode} - {GetNetworkServiceStatus(request.result)} - {GetNetworkServiceStatusAdvice(GetNetworkServiceStatus(request.result))}");
                responseCallback?.Invoke(request);
            }
        }

        public static NetworkServiceStatus GetNetworkServiceStatus(UnityWebRequest.Result result)
        {
            switch (result)
            {
                case UnityWebRequest.Result.InProgress:
                    return NetworkServiceStatus.PENDING;
                case UnityWebRequest.Result.Success:
                    return NetworkServiceStatus.COMPLETE_SUCCESSFUL;
                case UnityWebRequest.Result.ConnectionError:
                    return NetworkServiceStatus.CONNECTION_ERROR;
                case UnityWebRequest.Result.ProtocolError:
                    return NetworkServiceStatus.PROTOCOL_ERROR;
                case UnityWebRequest.Result.DataProcessingError:
                    return NetworkServiceStatus.DATA_PROCESSING_ERROR;
                default:
                    return NetworkServiceStatus.DEFAULT;
            }
        }

        public static string GetNetworkServiceStatusAdvice(NetworkServiceStatus status)
        {
            switch (status)
            {
                case NetworkServiceStatus.NONE:
                    return "EXCEPTION - It's not possible to determine the cause of this error.";
                case NetworkServiceStatus.DEFAULT:
                    return "EXCEPTION - It's not possible to determine the cause of this error.";
                case NetworkServiceStatus.PENDING:
                    return "PENDING OPERATION - Trying to reach out the server.";
                case NetworkServiceStatus.CONNECTION_ERROR:
                    return "CONNECTION ERROR - It was not possible to reach out the server.";
                case NetworkServiceStatus.PROTOCOL_ERROR:
                    return "DATA PROCESSING ERROR - Reached out the server, but received an error defined by connection protocol.";
                case NetworkServiceStatus.DATA_PROCESSING_ERROR:
                    return "DATA PROCESSING ERROR - Encountered an error when processing data get from server.";
                case NetworkServiceStatus.COMPLETE_UNSUCCESSFUL:
                    return "COMPLETE UNSUCCESSFUL - Server responses but something goes wrong.";
                case NetworkServiceStatus.COMPLETE_SUCCESSFUL:
                    return "COMPLETE SUCCESSFUL - It was possible to reach out the server.";
                default:
                    return "DEFAULT - The error encountered is not included in any foreseen case.";
            }
        }
    }

}