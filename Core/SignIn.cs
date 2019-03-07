using Michsky.UI.ModernUIPack;
using UnityEngine;

namespace VRSNS.Core
{
public class SignIn : MonoBehaviour
{
    [SerializeField] private CustomInputField userName;
    [SerializeField] private CustomInputField password;
    [SerializeField] private CustomInputField passwordConfirm;

    public void SendInfo()
    {
        if (!Client.InRoom)
            Client.Signin(userName.inputText.text, password.inputText.text);
    }

    public void SendNewInfo()
    {
        if (!Client.InRoom)
            Client.CreateUser(userName.inputText.text, userName.inputText.text, password.inputText.text, passwordConfirm.inputText.text);
    }
}
}