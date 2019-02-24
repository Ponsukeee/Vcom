using System.Threading.Tasks;
using VRUtils.Components;

namespace Components.Controller
{
public interface IInputModule
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <param name="deviceInfo"></param>
    /// <returns>next module</returns>
    IInputModule HandleInput(InputType input, DeviceInfo deviceInfo);
    void OnSet();
    void OnUnset();
}
}