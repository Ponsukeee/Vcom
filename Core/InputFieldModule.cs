using Components.Controller;
using TMPro;
using UnityEngine;
using VRUtils.Components;

namespace VRSNS.Core
{
public class InputFieldModule : MonoBehaviour, IInputModule
{
    private TextMeshPro text;
    private IGlowable glowable;
        
    private void Awake()
    {
        text = GetComponentInChildren<TextMeshPro>();
        glowable = GetComponent<IGlowable>();
    }
    
    public IInputModule HandleInput(InputType input, DeviceInfo deviceInfo)
    {
        if (input == InputType.Click)
        {
            var keyBoardManager = deviceInfo.transform.root.GetComponent<KeyBoardManager>();
            keyBoardManager.ShowKeyBoard(text);
        }

        return null;
    }

    private void ShowKeyboard()
    {
        
    }

    public void OnSet()
    {
        glowable?.Glow();
    }

    public void OnUnset()
    {
        glowable?.Glow();
    }
}
}
