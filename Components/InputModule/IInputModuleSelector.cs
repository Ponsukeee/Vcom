namespace VRUtils.InputModule
{
public interface IInputModuleSelector
{
    IInputModule GetCurrentModule();
    void NotifyHandlingModule(IInputModule handlingModule);
}
}
