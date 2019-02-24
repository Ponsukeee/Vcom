using System;
using System.IO;
using UnityEngine;

namespace VRUtils.Components
{
public class ScreenShot : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private string directoryName;
    
    public void SavePng()
    {
        var renderTexture = camera.activeTexture;
        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();

        try
        {
            var bytes = tex.EncodeToPNG();
            Destroy(tex);

            string fileName = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
            System.IO.File.WriteAllBytes($"/{directoryName}/{fileName}.png", bytes);
            Notification.Notify("写真を保存しました");
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            throw;
        }
    }
}
}