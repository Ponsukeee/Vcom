using Components.Controller;
using TMPro;
using UnityEngine;
using VRUtils.Components;

namespace VRSNS.Core
{
public class SignUp : MonoBehaviour, IInputModule
{
    [SerializeField] private TextMeshPro name;
    [SerializeField] private TextMeshPro email;
    [SerializeField] private TextMeshPro password;
    [SerializeField] private TextMeshPro passwordConfirmation;
    
    public IInputModule HandleInput(InputType input, DeviceInfo deviceInfo)
    {
        if (input == InputType.DoubleClick)
        {
            var client = deviceInfo.GetDeviceObject().transform.root.GetComponentInChildren<Client>();
            client.CreateUser(name.text, email.text, password.text, passwordConfirmation.text);
        }

        return null;
    }
    
    public void OnSet()
    {
    }

    public void OnUnset()
    {
    }
}
}
