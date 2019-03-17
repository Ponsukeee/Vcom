using UnityEngine;
using VRUtils.Components;

public class FileCanvas : FlatCanvasBehaviour
{
    [SerializeField] private GameObject viewerCanvas; 
    
    public void Open()
    {
        GetComponent<File>().Open(viewerCanvas);
    }

    public void Share()
    {
        GetComponent<File>().ReadAllBytes();
    }
}
