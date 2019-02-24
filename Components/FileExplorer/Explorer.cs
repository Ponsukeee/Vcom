using System.Collections.Generic;
using System.Linq;
using VRUtils.Components;
using UnityEngine;

namespace VRUtils.Components
{
public class Explorer : MonoBehaviour, IGeneratable
{
    [SerializeField] private CanvasBehaviourBase directoryCanvas;
    [SerializeField] private CanvasBehaviourBase fileCanvas;
    [SerializeField] private GameObject viewerCanvas;

    private CanvasScroll CurrentScroll
    {
        get
        {
            foreach (var scroll in scrollList)
            {
                if (scroll.Exist(CanvasBehaviourBase.SelectedCanvas))
                {
                    return scroll;
                }
            }

            return null;
        }
    }

    private readonly List<CanvasScroll> scrollList = new List<CanvasScroll>();
    private Directory rootDirectory;
    private static readonly string ROOT_PATH = @"C:";

    private void Awake()
    {
        rootDirectory = gameObject.AddComponent<Directory>();
        rootDirectory.Initialize(ROOT_PATH, "");
    }

    public void Open(GameObject handlerObject)
    {
        if (scrollList.Count == 0)
        {
            OpenDirectory(rootDirectory);
            return;
        }

        var directory = CurrentScroll.SelectedCanvas.GetComponent<Directory>();
        if (directory != null)
        {
            OpenDirectory(directory);
            return;
        }

        var file = CurrentScroll.SelectedCanvas.GetComponent<File>();
        if (file != null)
        {
            OpenFile(file, handlerObject);
        }
    }

    private void OpenDirectory(Directory directory)
    {
        var scrollObject = new GameObject();
        var newScroll = scrollObject.AddComponent<CanvasScroll>();

        if (directory.Open(newScroll, directoryCanvas, fileCanvas) == false)
            return;

        scrollObject.name = directory.DirectoryPath;

        newScroll.transform.SetParent(transform);
        newScroll.OnPlusAction.AddListener(Open);
        newScroll.OnMinusAction.AddListener(Return);
        scrollList.Add(newScroll);
    }

    private void OpenFile(File file, GameObject handlerObject)
    {
        file.Open(viewerCanvas, handlerObject);
    }

    public void Return(GameObject handlerObject)
    {
        var removeScroll = CurrentScroll;
        scrollList.Remove(removeScroll);
        DestroyImmediate(removeScroll.gameObject);
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < scrollList.Count(); i++)
        {
            if (scrollList[i].ListCount() == 0) continue;

            scrollList[i].UpdateLocalPosition(i);
        }
    }

    public void OnGenerate()
    {
        OpenDirectory(rootDirectory);
    }
}
}