using Components.Controller;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using VRUtils.Components;
using VRUtils.InputModule;

namespace VRSNS.Core
{
public class InputFieldModuleBehaviour : MonoBehaviour, IInputModule
{
    private CustomInputField customInputField;
    private TMP_InputField inputField;

    private void Awake()
    {
        customInputField = GetComponent<CustomInputField>();
        inputField = customInputField.inputText;
    }

    public IInputModule HandleInput(InputType input, DeviceInfo deviceInfo)
    {
        if (input == InputType.Click)
        {
            customInputField.Animate();
            inputField.ActivateInputField();
            VirtualKeyBoard.EnableInput(inputField);
            VirtualKeyBoard.SetPosition(transform.position + Vector3.up * 1f);
        }

        return null;
    }

    public void OnSet()
    {
//        if (inputField)
//            inputField.placeholder.color = Color.cyan;
    }

    public void OnUnset()
    {
//        if (inputField)
//            inputField.placeholder.color = Color.gray;
    }
}
}