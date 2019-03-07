using MagicOnionClient;
using TMPro;
using VRSNS.Core;
using VRUtils.Components;

public class RoomInfoCanvas : FlatCanvasBehaviour
{
    private TextMeshProUGUI text;
    private string roomID;
    private VRSNS.Core.Client client;

    public void Initialize(RoomInfo roomInfo, VRSNS.Core.Client client)
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = $"ホスト名 : {roomInfo.OwnerName}";
        roomID = roomInfo.RoomID;
        this.client = client;
    }

    public void JoinRoom()
    {
        Client.JoinRoom(roomID);
    }
}
