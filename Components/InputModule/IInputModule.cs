namespace VRUtils.InputModule
{
public interface IInputModule
{
    IInputModule HandleInput(InputType input, DeviceInfo deviceInfo);
    void OnSet();
    void OnUnset();
}
}