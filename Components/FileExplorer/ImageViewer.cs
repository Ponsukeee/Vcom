using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace VRUtils.Components
{
public class ImageViewer : MonoBehaviour, IViewer
{
    public void Display(string filePath, GameObject handlerObject)
    {
        StartCoroutine(ViewImage(filePath, handlerObject));
    }
    
    private IEnumerator ViewImage(string filePath, GameObject handelerObject)
    {
        var canvas = GetComponentInChildren<Canvas>();
        var rawImage = canvas.gameObject.AddComponent<RawImage>();
        
        WWW www = new WWW(filePath);
        yield return www;

        rawImage.texture = www.textureNonReadable;
        AdjustCanvasSize(rawImage);
    }


    private void AdjustCanvasSize(RawImage rawImage)
    {
        var rectTransform = GetComponentInChildren<RectTransform>();
        var prevSize = rectTransform.sizeDelta;
        rawImage.SetNativeSize();
        var afterSize = rectTransform.sizeDelta;
        var scalingVector = new Vector3(afterSize.x / prevSize.x, afterSize.y / prevSize.y, 1f);

        var backgroundTransform = transform;
        backgroundTransform.localScale = Vector3.Scale(backgroundTransform.localScale, scalingVector * 0.5f);
        rectTransform.sizeDelta = prevSize;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
}