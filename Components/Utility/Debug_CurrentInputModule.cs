using TMPro;
using UnityEngine;
using VRUtils.Components;

namespace VRUtils.Components
{
public class Debug_CurrentInputModule : MonoBehaviour
{
    TextMeshProUGUI text;
    [SerializeField] private InputController inputController;

    private void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    
    private void Update()
    {
        text.text = inputController.GetNameOfCurrentInputModule();
    }
}
}