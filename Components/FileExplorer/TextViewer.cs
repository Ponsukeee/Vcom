using System;
using TMPro;
using UnityEngine;

namespace VRUtils.Components
{
public class TextViewer : MonoBehaviour, IViewer
{
    public void Display(String filePath, GameObject handlerObject)
    {
        var streamReader = System.IO.File.OpenText(filePath);
        var text = streamReader.ReadToEnd();
        streamReader.Close();

        GetComponentInChildren<TextMeshProUGUI>().text = text;

//        var tmp = transform.GetChild(0).gameObject.AddComponent<TextMeshPro>();
//        
//        tmp.text = text;
//        tmp.fontSize = 20;
//        tmp.enableAutoSizing = true;
//        tmp.fontSizeMin = 12;
//        tmp.fontSizeMax = 20;
//        tmp.alignment = TextAlignmentOptions.Center;
//        tmp.enableWordWrapping = true;
//        tmp.overflowMode = TextOverflowModes.Ellipsis;
    }
}
}