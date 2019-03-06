using TMPro;
using UniRx;
using UnityEngine;

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
            Debug.LogError("There are two objects with VirtualKeyBoard.");
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
    }

    public static void HideKeyBoard()
    {
        KeyBoard.SetActive(false);
    }

    public static void SetPosition(Vector3 position)
    {
        KeyBoard.transform.position = position;
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