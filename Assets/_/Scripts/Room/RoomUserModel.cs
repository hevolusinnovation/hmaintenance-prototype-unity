using Newtonsoft.Json;

[System.Serializable]       
public class RoomUserModel
{
    [JsonProperty("roomName")]
    public string RoomName { get; set; }

    [JsonProperty("participantName")]
    public string ParticipantName { get; set; }
}
