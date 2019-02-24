using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace VRUtils.Components
{
public class VideoViewer : MonoBehaviour, IViewer
{
    public void Display(string filePath, GameObject handlerObject)
    {
        StartCoroutine(VideoPlayStart(filePath, handlerObject));
    }
    
    private IEnumerator VideoPlayStart(string filePath, GameObject handlerObject)
    {
        var canvas = GetComponentInChildren<Canvas>();
        var videoPlayer = canvas.gameObject.AddComponent<VideoPlayer>();
        videoPlayer.url = filePath;
        videoPlayer.source = VideoSource.Url;
        videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
            yield return null;

        var audioSource = canvas.gameObject.AddComponent<AudioSource>();
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, audioSource);

        var rawImage = canvas.gameObject.AddComponent<RawImage>();
        rawImage.texture = videoPlayer.texture;

        AdjustCanvasSize(rawImage);

        videoPlayer.Play();
        while (videoPlayer.isPlaying)
        {
            yield return null;
        }
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