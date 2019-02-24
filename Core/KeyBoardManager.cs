using TMPro;
using UniRx;
using UnityEngine;

namespace VRSNS.Core
{
public class KeyBoardManager : MonoBehaviour
{
    [SerializeField] private GameObject keyBoardPrefab;

    private TextMeshPro handlingField;
    private GameObject showingKeyBoard;
    private bool IsShowingKeyBoard => showingKeyBoard != null;

    public void ShowKeyBoard(TextMeshPro text)
    {
        if (!IsShowingKeyBoard)
        {
            showingKeyBoard = Instantiate(keyBoardPrefab);
            InitializeKeyBoard(showingKeyBoard);
        }

        handlingField = text;
    }

    private void InitializeKeyBoard(GameObject keyBoard)
    {
        var keys = keyBoard.GetComponentsInChildren<InputKey>();
        foreach (var key in keys)
        {
            key.OnPressed.Subscribe(Input);
        }
    }

    public void HideKeyBoard()
    {
        DestroyImmediate(showingKeyBoard);
        showingKeyBoard = null;
        handlingField = null;
    }

    private void Input(string value)
    {
        if (value == "backspace")
        {
            var text = handlingField.text;
            handlingField.text = text.Remove(text.Length - 1);
        }
        else if(value == "space")
        {
            handlingField.text += " ";
        }
        else
        {
            handlingField.text += value.ToLower();
        }
    }
}
}