using Newtonsoft.Json;
using Unity.VisualScripting;

[System.Serializable]       
public class RoomUserModel
{
    [JsonProperty("roomName")]
    [field:Serialize] public string RoomName { get; set; }

    [JsonProperty("participantName")]
    [field: Serialize] public string ParticipantName { get; set; }
}