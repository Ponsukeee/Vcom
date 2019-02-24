using System.Threading.Tasks;
using Components.Controller;
using UniRx;
using UnityEngine;
using VRUtils.Components;

namespace VRUtils.Components
{
public class GestureInputModule : MonoBehaviour, IInputModule
{
    [SerializeField] private GameObject head;
    [SerializeField] private float maxGestureTime;
    [SerializeField] private float gestureMovementValue;

    public Subject<GameObject> OnRightDirection { get; } = new Subject<GameObject>();
    public Subject<GameObject> OnLeftDirection { get; } = new Subject<GameObject>();
    public Subject<GameObject> OnUpDirection { get; } = new Subject<GameObject>();
    public Subject<GameObject> OnDownDirection { get; } = new Subject<GameObject>();
    private Vector3 startPoint;
    private float startTime;

    public IInputModule HandleInput(InputType input, DeviceInfo deviceInfo)
    {
        switch (input)
        {
            case InputType.SubClick:
                StartGesture();
                return this;
            case InputType.SubDrag:
                return this;
            case InputType.SubRelease:
                StopGesture();
                return null;
        }

        return null;
    }

    private void StartGesture()
    {
        startPoint = transform.position;
        startTime = Time.time;
    }

    private void StopGesture()
    {
        var gestureTime = Time.time - startTime;
        if (gestureTime > maxGestureTime) return;

        var headTransform = head.transform;
        var gestureDir = transform.position - startPoint;
        var upDir = Vector3.Scale(gestureDir, Vector3.up);
        var rightDir = Vector3.Scale(gestureDir, headTransform.right);
        var upDirValue = upDir.magnitude;
        var rightDirValue = rightDir.magnitude;

        if (upDirValue < gestureMovementValue && rightDirValue < gestureMovementValue) return;

        if (upDirValue > rightDirValue)
        {
            if (Vector3.Dot(Vector3.up, upDir) > 0)
                OnUpDirection.OnNext(gameObject);
            else
                OnDownDirection.OnNext(gameObject);
        }
        else
        {
            if (Vector3.Dot(headTransform.right, rightDir) > 0)
                OnRightDirection.OnNext(gameObject);
            else
                OnLeftDirection.OnNext(gameObject);
        }
    }

    public void OnSet()
    {
    }

    public void OnUnset()
    {
    }
}
}