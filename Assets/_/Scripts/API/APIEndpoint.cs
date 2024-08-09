public static class APIEndpoint
{

    //BASE
    public const string LOCAL_URL = "http://localhost:6080";
    public const string LAN_URL = "https://192-168-1-4.openvidu-local.dev:7443";

    //TOKEN
    public const string LOCAL_TOKEN_URL = LOCAL_URL + "/token";
    public const string LAN_TOKEN_URL = LAN_URL + "/token";

    //LIVEKIT
    public const string LIVEKIT_URL = /*"http://localhost:7880"*/ "ws://localhost:7880";

}