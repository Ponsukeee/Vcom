using Components.Controller;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using VRUtils.Components;
using VRUtils.InputModule;

namespace VRSNS.Core
{
public class VirtualKey : MonoBehaviour
{
    private TextMeshProUGUI tmp;

    private void Awake()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SendInput()
    {
        VirtualKeyBoard.Input(tmp.text);
    }
}
}