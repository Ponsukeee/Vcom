using UnityEngine;

namespace VRSNS.Core
{
public class ClientTest : MonoBehaviour
{
    private async void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Client.Signin("test", "test");
        }
        
        if (Input.GetKeyDown(KeyCode.B))
        {
            Client.LeaveRoom();
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Start");
            var roomInfos = await Client.GetRooms();
            foreach (var roomInfo in roomInfos)
            {
                Debug.Log($"オーナー = {roomInfo.OwnerName}, ID = {roomInfo.RoomID}");
            }
            Debug.Log(roomInfos.Length);
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            var roomInfos = await Client.GetRooms();
            Client.JoinRoom(roomInfos[0].RoomID);
        }
    }
}
}
