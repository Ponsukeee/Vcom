using System;
using System.Threading;
using Components.Controller;
using UniRx.Async;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRUtils.Components;
using VRUtils.InputModule;

[RequireComponent(typeof(Image))]
public class VRButton : MonoBehaviour, IInputModule
{
    [SerializeField] private Color highlightColor;
    public UnityEvent onClick = new UnityEvent();
    private Image image;
    private Color defaultColor;
    private Color targetColor;
    private Vector3 defaultScale;
    private Vector3 targetScale;
    private CancellationTokenSource cts = new CancellationTokenSource();
    
    private void Awake()
    {
        image = GetComponent<Image>();
        defaultColor = image.color;
        targetColor = defaultColor;
        defaultScale = transform.localScale;
        targetScale = transform.localScale;
    }

    private void Update()
    {
//        image.color = Color.Lerp(image.color, targetColor, 0.12f);
//        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, 0.12f);
//        CurrentColor = targetColor;
//        if (CurrentColor.Equals(targetColor))
//        {
//            this.enabled = false;
//        }
    }

    private async UniTask UpdateAnimation()
    {
        var animationFrame = 12;
        try
        {
            var startFrame = Time.frameCount;
            while (animationFrame - (Time.frameCount - startFrame) > 0f)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, cts.Token);
                image.color = Color.Lerp(image.color, targetColor, 0.12f);
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, 0.12f);
            }
        }
        catch (OperationCanceledException)
        {
            Debug.Log("UniTask.Yield is cancelled");
            throw;
        }
    }

    public IInputModule HandleInput(InputType input, DeviceInfo deviceInfo)
    {
        if (input == InputType.Click)
        {
            onClick.Invoke();
//            targetColor = defaultColor + highlightColor;
//            targetScale = Vector3.Scale(defaultScale, new Vector3(1.1f, 1.1f, 1.1f));
//            await UpdateAnimation();
//            targetColor = defaultColor;
//            targetScale = defaultScale;
//            await UpdateAnimation();
        }

        return null;
    }

    public async void OnSet()
    {
        targetColor = defaultColor + highlightColor;
        targetScale = Vector3.Scale(defaultScale, new Vector3(1.1f, 1.1f, 1.1f));
        UpdateAnimation().Forget(e => { });
//        await UpdateAnimation();
    }

    public async void OnUnset()
    {
        targetColor = defaultColor;
        targetScale = defaultScale;
        UpdateAnimation().Forget(e => { });
//        await UpdateAnimation();
    }

    private void OnDestroy()
    {
    }
}
