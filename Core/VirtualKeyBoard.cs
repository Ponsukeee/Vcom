using TMPro;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace VRSNS.Core
{
public class VirtualKeyBoard : MonoBehaviour
{
    private static GameObject KeyBoard { get; set; }
    private static TMP_InputField handlingField;
    public static bool IsActive => KeyBoard.activeSelf;

    private void Awake()
    {
        if (KeyBoard != null)
        {
            Debug.LogWarning("There are two objects with VirtualKeyBoard.");
        }

        KeyBoard = gameObject;
        KeyBoard.SetActive(false);
    }

    private void Update()
    {
        if (handlingField == null)
        {
            HideKeyBoard();
        }
    }

    public static void EnableInput(TMP_InputField inputField)
    {
        if (!IsActive)
        {
            KeyBoard.SetActive(true);
        }

        if (handlingField != null)
        {
            handlingField.DeactivateInputField();
        }

        handlingField = inputField;
        handlingField.ActivateInputField();
        handlingField.caretPosition = handlingField.text.Length;
        handlingField.stringPosition = handlingField.text.Length;

        KeyBoard.transform.position = Player.instance.hmdTransform.position + Player.instance.transform.forward * 0.5f + Vector3.up * -0.3f;
        KeyBoard.transform.LookAt(Player.instance.hmdTransform);
        KeyBoard.transform.Rotate(0f, 180f, 0f);
    }

    public void HideKeyBoard()
    {
        KeyBoard.SetActive(false);
    }

    public static void Input(string value)
    {
        if (handlingField == null) return;

        if (value == "back")
        {
            if (handlingField.stringPosition > 0)
                handlingField.text = handlingField.text.Remove(handlingField.stringPosition - 1);
        }
        else if (value == "space")
        {
            handlingField.text = handlingField.text.Insert(handlingField.stringPosition, " ");
            handlingField.stringPosition++;
        }
        else
        {
            handlingField.text = handlingField.text.Insert(handlingField.stringPosition, value.ToLower());
            handlingField.stringPosition++;
        }
    }
}
}