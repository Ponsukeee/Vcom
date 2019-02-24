using System;
using UnityEngine;
using VRUtils.Components;
using Client = VRSNS.Core.Client;

public class RoomInfoScroll : MonoBehaviour
{
    [SerializeField] private Client client;
    [SerializeField] private RoomInfoCanvas roomInfoCanvas;
    private CanvasScroll scroll;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            DisplayRoomInfos();
        }
    }

    public async void DisplayRoomInfos()
    {
        try
        {
            var roomInfos = await client.GetRooms();
            var scrollObject = new GameObject();
            scroll = scrollObject.AddComponent<CanvasScroll>();
            Vector3 offset = Vector3.zero;
            foreach (var roomInfo in roomInfos)
            {
                var instantiatedCanvas = scroll.AddCanvas(roomInfoCanvas.gameObject, $"{roomInfo.OwnerName}'s Room");
                var canvas = instantiatedCanvas.GetComponent<RoomInfoCanvas>();
                canvas.SetParentScroll(scroll);
                canvas.Initialize(roomInfo, client);
                offset = Vector3.Scale(canvas.CanvasSize(), Vector3.right) / 2;
            }

            scrollObject.transform.localPosition += new Vector3(GetComponent<RectTransform>().rect.width * transform.localScale.x / 2 + 3, 0f, 0f) + offset ;
            scrollObject.transform.SetParent(transform.root, false);
        }
        catch (Exception e)
        {
            Notification.Notify("ルーム情報を取得できませんでした");
            Debug.LogError(e);
            throw;
        }
    }
}
