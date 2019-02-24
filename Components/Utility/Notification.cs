using UnityEngine;

namespace VRUtils.Components
{
public class Notification : MonoBehaviour
{
    [SerializeField] private NotifyPanel panelPrefab;
    private static NotifyPanel panel;
    
    private void Awake()
    {
        panel = panelPrefab;
        if (panel == null)
        {
            Debug.LogWarning("表示させるパネルが存在しない");
            enabled = false;
        }
    }
    
    public static void Notify(string content)
    {
        panel.ShowNotifyPanel(content);
        Debug.Log(content);
    }
}
}
