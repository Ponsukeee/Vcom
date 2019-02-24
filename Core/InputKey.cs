using Components.Controller;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using VRUtils.Components;

namespace VRSNS.Core
{
public class InputKey : MonoBehaviour, IInputModule
{
    private TextMeshProUGUI text;
    public class UnityEventKeyBoard : UnityEvent<string>{}
//    public UnityEventKeyBoard OnPressed { get; } = new UnityEventKeyBoard();
    public Subject<string> OnPressed { get; } = new Subject<string>();

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    
    public IInputModule HandleInput(InputType input, DeviceInfo deviceInfo)
    {
        if (input == InputType.Click)
        {
            OnPressed.OnNext(text.text);
        }

        return null;
    }

    public void OnSet()
    {
//        throw new System.NotImplementedException();
    }

    public void OnUnset()
    {
//        throw new System.NotImplementedException();
    }
}
}