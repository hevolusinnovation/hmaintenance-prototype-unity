using Newtonsoft.Json;
using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public static class APIUtility
{
#pragma warning disable
    private static readonly string jsonMediaType = "application/json";
#pragma warning enable

    public static async Task<HttpResponseMessage> GetFromEndpoint(string uri)
    {
        using (HttpClient client = new HttpClient())
        {
            Uri endpoint = GetEndpoint(uri);
            return await client.GetAsync(endpoint);
        }
    }

    public static async Task<HttpResponseMessage> PostToEndpoint(StringContent content, string uri)
    {
        using (HttpClient client = new HttpClient())
        {
            Uri endpoint = GetEndpoint(uri);
            return await client.PostAsync(endpoint, content);
        }
    }

    public static Uri GetEndpoint(string uri)
    {
        return new Uri(uri);
    }

    public static async Task<string> GetJsonContent(HttpResponseMessage response)
    {
        return await response.Content.ReadAsStringAsync();
    }

    public static string CreateJsonContent<T>(T serializableObject)
    {
        // JSON CONVERT
        string jsonConvert = JsonConvert.SerializeObject(serializableObject);
        Debug.Log($"JsonConvert result: {jsonConvert}");

        return jsonConvert;

        // JSON UTILITY
        //string jsonUtility = JsonUtility.ToJson(serializableObject);
        //Debug.Log($"JsonUtility result: {jsonUtility}");

        //return jsonUtility;
    }

    public static StringContent CreateStringContent(string jsonContent)
    {
        return new StringContent(jsonContent, Encoding.UTF8); //StringContent(jsonContent, Encoding.UTF8, jsonMediaType);
    }

    public static StringContent CreateStringContent<T>(T serializableObject)
    {
        string json = CreateJsonContent(serializableObject);
        return CreateStringContent(json);
    }
}
