using System;
using System.Collections;
using VRUtils.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace VRUtils.Components
{
public class File : MonoBehaviour
{
    private static readonly string[] READABLE_EXTENSIONS = {"txt", "mp4", "png", "jpg", "vrm"};
    
    private string fileName;
    private string parentPath;
    private string FilePath => parentPath + @"\" + fileName;

    public static bool CanOpen(string extension)
    {
        foreach (var t in READABLE_EXTENSIONS)
        {
            if (extension == t)
                return true;
        }

        return false;
    }

    public void Initialize(string path, string name)
    {
        parentPath = path;
        fileName = name;

        GetComponentInChildren<TextMeshProUGUI>().text = name;
    }

    public void Open(GameObject canvasBehaviour, GameObject handlerObject)
    {
        var array = fileName.Split('.');
        var extension = array[array.Length - 1];
        GameObject instantiatedCanvas;

        try
        {
            IViewer viewer = null;
            switch (extension)
            {
                case "mp4":
                    instantiatedCanvas = InitializeCanvas(canvasBehaviour);
                    viewer = instantiatedCanvas.AddComponent<VideoViewer>();
                    break;
                case "txt":
                    instantiatedCanvas = InitializeCanvas(canvasBehaviour);
                    viewer = instantiatedCanvas.AddComponent<TextViewer>();
                    break;
                case "png":
                case "jpg":
                    instantiatedCanvas = InitializeCanvas(canvasBehaviour);
                    viewer = instantiatedCanvas.AddComponent<ImageViewer>();
                    break;
                case "vrm":
                    var go = new GameObject();
                    viewer = go.AddComponent<VrmViewer>();
                    break;
            }

            viewer?.Display(FilePath, handlerObject);
        }
        catch (System.IO.IOException e)
        {
            Debug.LogError(e);
            throw;
        }
    }

    private GameObject InitializeCanvas(GameObject canvasBehaviour)
    {
        var instantiatedCanvas = Instantiate(canvasBehaviour);
        var tf = transform;
        instantiatedCanvas.transform.position = tf.position;
        instantiatedCanvas.transform.localRotation = tf.rotation;

        return instantiatedCanvas;
    }
}
}