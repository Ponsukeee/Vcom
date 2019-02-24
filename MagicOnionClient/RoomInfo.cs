using MessagePack;
using Valve.Newtonsoft.Json;

namespace MagicOnionClient
{
[MessagePackObject]
[JsonObject("RoomInfo")]
public class RoomInfo
{
    [JsonProperty("RoomID")]
    [Key(0)]
    public string RoomID { get; set; }

    [JsonProperty("RoomName")]
    [Key(1)]
    public string RoomName { get; set; }

    [JsonProperty("OwnerName")]
    [Key(2)]
    public string OwnerName { get; set; }

    [JsonProperty("IsPublic")]
    [Key(3)]
    public bool IsPublic { get; set; }
}
}
