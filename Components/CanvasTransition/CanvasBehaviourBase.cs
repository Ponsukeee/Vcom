using Components.Controller;
using VRUtils.Components;
using UnityEngine;
using UnityEngine.Events;

namespace VRUtils.Components
{
public abstract class CanvasBehaviourBase : GrabbableBase, IInputModule
{
    private Renderer rend;
    private RectTransform rect;
    protected Transform tf;
    private CanvasScroll parentScroll;
    public static CanvasBehaviourBase SelectedCanvas { get; private set; }

    private void Awake()
    {
        rend = GetComponentInChildren<Renderer>();
        rect = GetComponent<RectTransform>();
        tf = transform;
        OnAwake();
    }

    protected virtual void OnAwake()
    {
    }

    /// <summary>
    /// スクロールさせる軸の仮想の座標を返す
    /// </summary>
    /// <param name="indexOfTarget"></param>
    /// <param name="centerPosition"></param>
    /// <param name="moveAxis"></param>
    public abstract Vector3 PositionOfMoveAxis(int indexOfTarget, Vector3 centerPosition, int moveAxis);

    /// <summary>
    /// アクションが発生する軸の仮想の座標を返す
    /// </summary>
    /// <param name="centerPosition"></param>
    /// <param name="actionAxis"></param>
    /// <returns></returns>
    public abstract Vector3 PositionOfActionAxis(Vector3 centerPosition, int actionAxis);

    public Vector3 CanvasSize()
    {
//        return Vector3.Scale(rend.bounds.size, transform.localScale);
//        return rend.bounds.size;
        return Vector3.Scale(new Vector3(rect.rect.width, rect.rect.height, 0f), rect.localScale);
//        return new Vector3(rect.rect.width, rect.rect.height, 0f);
    }

    public void SetParentScroll(CanvasScroll scroll)
    {
        parentScroll = scroll;
    }

    public IInputModule HandleInput(InputType input, DeviceInfo deviceInfo)
    {
        switch (input)
        {
            case InputType.Click:
                parentScroll.Grab();
                break;
            case InputType.Drag:
                parentScroll.Drag(deviceInfo.GetDiffPosition(), deviceInfo.GetDeviceObject());
                break;
            case InputType.Release:
                parentScroll.Release(deviceInfo.GetVelocity() + deviceInfo.GetAngularVelocity());
                return null;
            case InputType.DoubleClick:
                parentScroll.OnPlusAction.Invoke(deviceInfo.GetDeviceObject());
                return null;
            case InputType.SubClick:
                Grab(deviceInfo);
                break;
            case InputType.SubRelease:
                Release(deviceInfo);
                return null;
            case InputType.SubDrag:
                Drag(deviceInfo);
                break;
            case InputType.SubDoubleClick:
                parentScroll.OnMinusAction.Invoke(deviceInfo.GetDeviceObject());
                break;
        }
        
        return this;
    }

    public void OnSet()
    {
        if (parentScroll == null) return;
        parentScroll.SelectCanvas(this);
        SelectedCanvas = this;
        GetComponent<IGlowable>()?.Glow();
    }

    public void OnUnset()
    {
        if (parentScroll == null) return;
        parentScroll.SelectCanvas(null);
        GetComponent<IGlowable>()?.Darken();
    }
}
}