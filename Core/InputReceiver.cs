using UnityEngine;
using UnityEngine.Events;
using VRUtils.Components;
using VRUtils.InputModule;

namespace Components.Controller
{
public class InputReceiver : MonoBehaviour, IInputModule
{
    [SerializeField] private InputType inputType;
    [SerializeField] private UnityEvent onReceiveInput;
    
    public IInputModule HandleInput(InputType input, DeviceInfo deviceInfo)
    {
        if (inputType == input)
        {
            onReceiveInput.Invoke();
        }

        return null;
    }

    public void OnSet()
    {
        GetComponent<IGlowable>()?.Glow();
    }

    public void OnUnset()
    {
        GetComponent<IGlowable>()?.Darken();
    }
}
}