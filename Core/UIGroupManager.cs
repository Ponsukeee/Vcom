using UnityEngine;
using VRSNS.Core;

public class UIGroupManager : MonoBehaviour
{
    [SerializeField] private RoomInfoScroll roomInfo;

    private void Awake()
    {
        var buttons = GetComponentsInChildren<VRButton>();
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.AddListener(roomInfo.Hide);
        }
    }
}
