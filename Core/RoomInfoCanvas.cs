using MagicOnionClient;
using TMPro;
using VRSNS.Core;
using VRUtils.Components;

public class RoomInfoCanvas : FlatCanvasBehaviour
{
    private TextMeshProUGUI text;
    private string roomID;

    public void Initialize(RoomInfo roomInfo)
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = $"ホスト名 : {roomInfo.OwnerName}";
        roomID = roomInfo.RoomID;
    }

    public void JoinRoom()
    {
        Client.JoinRoom(roomID);
    }
}
