using System;
using UnityEngine;
using VRSNS.Core;
using VRUtils.Components;
using Client = VRSNS.Core.Client;

namespace VRSNS.Core
{
public class RoomInfoScroll : MonoBehaviour
{
    [SerializeField] private Client client;
    [SerializeField] private RoomInfoCanvas roomInfoCanvas;
    private CanvasScroll scroll;

    public async void DisplayRoomInfos()
    {
        try
        {
            Hide();
            if (!Client.InRoom) return;
            
            var roomInfos = await Client.GetRooms();
            var scrollObject = new GameObject();
            scroll = scrollObject.AddComponent<CanvasScroll>();
            Vector3 halfCanvasSize = Vector3.zero;
            foreach (var roomInfo in roomInfos)
            {
                var instantiatedCanvas = scroll.AddCanvas(roomInfoCanvas.gameObject, $"{roomInfo.OwnerName}'s Room");
                var canvas = instantiatedCanvas.GetComponent<RoomInfoCanvas>();
                canvas.SetParentScroll(scroll);
                canvas.Initialize(roomInfo, client);
                halfCanvasSize = Vector3.Scale(canvas.CanvasSize(), Vector3.right) / 2;
            }

            var halfButtonSize = GetComponent<RectTransform>().rect.width * transform.localScale.x / 2;
            scrollObject.transform.localPosition += new Vector3(halfButtonSize + 3, 0f, 0f) + halfCanvasSize;
            scrollObject.transform.SetParent(transform.root, false);
        }
        catch (Exception e)
        {
            Notification.Notify("ルーム情報を取得できませんでした");
            Debug.LogError(e);
            throw;
        }
    }

    public void Hide()
    {
        if (scroll != null)
        {
            DestroyImmediate(scroll.gameObject);
            scroll = null;
        }
    }
}
}