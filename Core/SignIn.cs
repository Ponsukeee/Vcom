using Components.Controller;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using VRUtils.Components;

namespace VRSNS.Core
{
public class SignIn : MonoBehaviour
{
    [SerializeField] private Client client;
    [SerializeField] private CustomInputField userName;
    [SerializeField] private CustomInputField password;

    public void SendInfo()
    {
        client.Signin(userName.inputText.text, password.inputText.text);
    }
}
}
