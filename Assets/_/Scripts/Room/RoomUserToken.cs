using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserToken
{

    [JsonProperty("token")]
    [field:SerializeField] public string Token { get; set; }

}

[Serializable]
public class RoomUserToken
{

    [JsonProperty("nbf")]
    public DateTime StartTime { get; set; }

    [JsonProperty("exp")]
    public DateTime ExpirationTime { get; set; }

    [JsonProperty("iss")]
    public string APIKey { get; set; }

    [JsonProperty("sub")]
    public string Identity { get; set; }

    [JsonProperty("video")]
    public VideoGrant Video { get; set; }

    [JsonProperty("metadata")]
    public string Metadata { get; set; }

    [JsonProperty("sip")]
    public Sip Sip { get; set; }

    [JsonProperty("attributes")]
    public KeyValuePair<string, string> Attributes { get; set; }

}

[Serializable]
public class VideoGrant
{

    //Permission to create or delete rooms
    [JsonProperty("roomCreate")]
    public bool RoomCreate {  get; set; }

    //Permission to list available rooms
    [JsonProperty("roomList")]
    public bool RoomList { get; set; }

    //Permission to join a room
    [JsonProperty("roomJoin")]
    public bool RoomJoin { get; set; }

    //Permission to moderate a room
    [JsonProperty("roomAdmin")]
    public bool RoomAdmin { get; set; }

    //Permissions to use Egress service
    [JsonProperty("roomRecord")]
    public bool RoomRecord { get; set; }

    //Permissions to use Ingress service
    [JsonProperty("ingressAdmin")]
    public bool IngressAdmin { get; set; }

    //Name of the room, required if join or admin is set
    [JsonProperty("room")]
    public string Room { get; set; }

    //Allow participant to publish data to the room
    [JsonProperty("canPublish")]
    public bool CanPublish { get; set; }

    //Allow participant to publish tracks
    [JsonProperty("canPublishData")]
    public bool CanPublishData { get; set; }

    //When set, only listed source can be published. (camera, microphone, screen_share, screen_share_audio)
    [JsonProperty("canPublicSources")]
    public string[] CannPublicSources { get; set; }

    //Allow participant to subscribe to tracks
    [JsonProperty("canSubscribe")]
    public bool CanSubscribe { get; set; }

    //Allow participant to update its own metadata
    [JsonProperty("canUpdateOwnMetadata")]
    public bool CanUpdateOwnMetadata { get; set; }

    //Hide participant from others in the room
    [JsonProperty("hidden")]
    public bool Hidden { get; set; }

    [JsonProperty("kind")]
    public string Kind { get; set; }

}

[Serializable]
public class ParticipantMetadata
{

}

[Serializable]
public class Sip
{

    //Permission to manage SIP trunks and dispatch rules
    [JsonProperty("admin")]
    public bool Admin { get; set; }

    //Permission to make SIP calls via CreateSIPParticipant
    [JsonProperty("call")]
    public bool Call { get; set; }

}