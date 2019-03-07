using TMPro;
using UnityEngine;

namespace VRSNS.Core
{
public class VirtualKey : MonoBehaviour
{
    private TextMeshProUGUI tmp;

    private void Awake()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SendInput()
    {
        VirtualKeyBoard.Input(tmp.text);
    }
}
}