using System.Collections.Generic;
using System.IO;
using VRUtils.Components;
using TMPro;
using UnityEngine;

namespace VRUtils.Components
{
public class Directory : MonoBehaviour
{
    private string parentPath;
    private string directoryName;
    public string DirectoryPath => parentPath + @"\" + directoryName; 
    
    public static bool Open(CanvasScroll scroll, CanvasBehaviourBase directoryCanvas, CanvasBehaviourBase fileCanvas, string path)
    {
        var directoryList = System.IO.Directory.GetDirectories(path);
        var fileList = System.IO.Directory.GetFiles(path);

        if (directoryList.Length == 0 && fileList.Length == 0)
            return false;
        
        foreach (var directoryPath in directoryList)
        {
            var directoryName = new DirectoryInfo(directoryPath).Name;
            var instantiatedCanvas = scroll.AddCanvas(directoryCanvas.gameObject, directoryName);
            instantiatedCanvas.GetComponent<CanvasBehaviourBase>().SetParentScroll(scroll);
            var directory = instantiatedCanvas.AddComponent<Directory>();
            directory.Initialize(path, directoryName);
        }

        foreach (var filePath in fileList)
        {
            var fileName = new FileInfo(filePath).Name;
            var array = fileName.Split('.');
            var extension = array[array.Length - 1];
            if (!File.CanOpen(extension))
                continue;
            
            var instantiatedCanvas = scroll.AddCanvas(fileCanvas.gameObject, fileName);
            instantiatedCanvas.GetComponent<CanvasBehaviourBase>().SetParentScroll(scroll);
            var file = instantiatedCanvas.AddComponent<File>();
            file.Initialize(path, fileName);
        }

        return true;
    }
    
    public bool Open(CanvasScroll scroll, CanvasBehaviourBase directoryCanvas, CanvasBehaviourBase fileCanvas)
    {
        return Open(scroll, directoryCanvas, fileCanvas, DirectoryPath);
    }

    public void Initialize(string path, string name)
    {
        parentPath = path;
        directoryName = name;

        var text = GetComponentInChildren<TextMeshProUGUI>();
        if (text == null) return;
        
        GetComponentInChildren<TextMeshProUGUI>().text = name;
    }
}
}