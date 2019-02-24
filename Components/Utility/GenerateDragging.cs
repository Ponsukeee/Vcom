using Components.Controller;
using VRUtils.Components;

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
            case InputType.Drag:
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