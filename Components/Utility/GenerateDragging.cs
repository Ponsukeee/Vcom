using Components.Controller;
using VRUtils.Components;
using VRUtils.InputModule;

namespace VRUtils.Components
{
public class GenerateDragging : GrabbableBase, IInputModule
{
    public IInputModule HandleInput(InputType input, DeviceInfo deviceInfo)
    {
        switch (input)
        {
            case InputType.Click:
                Grab(deviceInfo);
                break;
            case InputType.Clicking:
                Drag(deviceInfo);
                break;
            case InputType.Release:
                Release(deviceInfo);
                Destroy(this, 0.5f);
                return null;
        }

        return this;
    }

    public void OnSet()
    {
    }

    public void OnUnset()
    {
    }
}
}