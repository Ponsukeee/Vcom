using UnityEngine;
using UnityEngine.Events;
using VRUtils.InputModule;

namespace VRUtils.Components
{
[RequireComponent(typeof(Collider))]
public class Grabbable : GrabbableBase, IInputModule
{
    [HideInInspector] public UnityEvent onDrop;
    private IGlowable glowable;

    private void Awake()
    {
        glowable = GetComponent<IGlowable>();
    }

    public IInputModule HandleInput(InputType input, DeviceInfo deviceInfo)
    {
        switch (input)
        {
            case InputType.Click:
            case InputType.DoubleClick:
            case InputType.SubClick:
                Grab(deviceInfo);
                break;
            case InputType.Release:
            case InputType.SubRelease:
                onDrop?.Invoke();
                Release(deviceInfo);
                return null;
            case InputType.Clicking:
            case InputType.SubClicking:
                Drag(deviceInfo);
                break;
            case InputType.Up:
                MoveForward();
                break;
            case InputType.Down:
                MoveBack();
                break;
        }

        return this;
    }

    public void OnSet()
    {
        glowable?.Glow();
    }

    public void OnUnset()
    {
        glowable?.Darken();
    }
}
}