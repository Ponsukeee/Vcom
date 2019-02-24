using Components.Controller;
using TMPro;
using UnityEngine;
using VRUtils.Components;

namespace VRSNS.Core
{
public class SignIn : MonoBehaviour
{
    [SerializeField] private Client client;

    private async void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            client.Signin("test", "test");
        }
        
        if (Input.GetKeyDown(KeyCode.B))
        {
            client.LeaveRoom();
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Start");
            var roomInfos = await client.GetRooms();
            foreach (var roomInfo in roomInfos)
            {
                Debug.Log($"オーナー = {roomInfo.OwnerName}, ID = {roomInfo.RoomID}");
            }
            Debug.Log(roomInfos.Length);
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            var roomInfos = await client.GetRooms();
            client.JoinRoom(roomInfos[0].RoomID);
        }
    }

    public void Signin()
    {
        client.Signin("test", "test");
    }
}
}
