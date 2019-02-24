using System;
using System.Threading;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace VRUtils.Components
{
public class PanelAnimation : MonoBehaviour
{
    [SerializeField] private GameObject head;
    [SerializeField] private float animationFrame;
    [SerializeField] private GestureInputModule[] gestureInputModules;

    private Vector3 defaultScale;
    private Transform uiTransform;
    private CancellationTokenSource cts;
    private float startFrame;

    private void Awake()
    {
        defaultScale = transform.localScale;
        uiTransform = transform;
        cts = new CancellationTokenSource();

        var verticalMinScale = new Vector3(defaultScale.x, 0f, defaultScale.z);
        var horizontalMinScale = new Vector3(0f, defaultScale.y, defaultScale.z);
        var verticalShiftValue = uiTransform.up * defaultScale.y / 2;
        var horizontalShiftValue = uiTransform.right * defaultScale.x / 2;
        foreach (var module in gestureInputModules)
        {
            module.OnUpDirection.Subscribe(source => PlayAnimation(source, verticalMinScale, -verticalShiftValue, defaultScale.y / animationFrame));
            module.OnDownDirection.Subscribe(source => PlayAnimation(source, verticalMinScale, verticalShiftValue, defaultScale.y / animationFrame));
            module.OnRightDirection.Subscribe(source => PlayAnimation(source, horizontalMinScale, -horizontalShiftValue, defaultScale.x / animationFrame));
            module.OnLeftDirection.Subscribe(source => PlayAnimation(source, horizontalMinScale, horizontalShiftValue, defaultScale.x / animationFrame));
        }

        gameObject.SetActive(false);
    }

    private async void PlayAnimation(GameObject sourceObject, Vector3 minScale, Vector3 initialShift, float maxDistanceDelta)
    {
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
            startFrame = Time.frameCount;

            var targetPosition = sourceObject.transform.position;
            LookAtHead(targetPosition);
            uiTransform.localScale = minScale;
            uiTransform.position = targetPosition + initialShift;

            await UpdateAnimation(defaultScale, targetPosition, maxDistanceDelta);
        }
        else
        {
            startFrame = Time.frameCount;
            var targetScale = minScale;
            var targetPosition = uiTransform.position - initialShift;

            await UpdateAnimation(targetScale, targetPosition, maxDistanceDelta);
            gameObject.SetActive(false);
        }
    }

    private async UniTask UpdateAnimation(Vector3 targetScale, Vector3 targetPosition, float maxDistanceDelta)
    {
        try
        {
            while (animationFrame - (Time.frameCount - startFrame) > 0f)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, cts.Token);
                uiTransform.localScale = Vector3.MoveTowards(uiTransform.localScale, targetScale, maxDistanceDelta);
                uiTransform.position = Vector3.MoveTowards(uiTransform.position, targetPosition, maxDistanceDelta / 2);
            }
        }
        catch (OperationCanceledException)
        {
            Debug.Log("UniTask.Yield is cancelled");
            throw;
        }
    }

    private void LookAtHead(Vector3 targetPosition)
    {
        uiTransform.localRotation = Quaternion.identity;
        var toUiVector = targetPosition - head.transform.position;
        var planeVector = new Vector3(1f, 0f, 1f);
        var angle = Vector3.Angle(Vector3.Scale(planeVector, toUiVector), Vector3.forward);
        if (!(Vector3.Dot(toUiVector, Vector3.right) > 0))
            angle = -angle;
        var rot = Quaternion.AngleAxis(angle, Vector3.up);
        uiTransform.localRotation *= rot;
    }
}
}