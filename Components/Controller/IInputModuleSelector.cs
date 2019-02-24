namespace Components.Controller
{
public interface IInputModuleSelector
{
    IInputModule GetCurrentModule();
    void NotifyHandlingModule(IInputModule handlingModule);
}
}
