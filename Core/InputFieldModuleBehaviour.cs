using System;
using System.Threading;
using Michsky.UI.ModernUIPack;
using TMPro;
using UniRx.Async;
using UnityEngine;
using VRUtils.InputModule;

namespace VRSNS.Core
{
public class InputFieldModuleBehaviour : MonoBehaviour, IInputModule
{
    private CustomInputField customInputField;
    private TMP_InputField inputField;
    
    private Vector3 defaultScale;
    private Vector3 targetScale;
    private CancellationTokenSource cts = new CancellationTokenSource();

    private void Awake()
    {
        customInputField = GetComponent<CustomInputField>();
        inputField = customInputField.inputText;
        
        defaultScale = transform.localScale;
        targetScale = transform.localScale;
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
            customInputField.Animate();
            VirtualKeyBoard.EnableInput(inputField);
        }

        return null;
    }

    public void OnSet()
    {
        var value = 1.05f;
        targetScale = Vector3.Scale(defaultScale, new Vector3(value, value, value));
        UpdateAnimation().Forget(e => { });
    }

    public void OnUnset()
    {
        targetScale = defaultScale;
        UpdateAnimation().Forget(e => { });
    }
}
}