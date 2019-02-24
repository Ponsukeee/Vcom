using UnityEngine;
using VRUtils.Components;

namespace Components.Controller
{
public class KeyboardInput : IInputDevice
{
    public bool PressDownTrigger()
    {
        return Input.GetKeyDown(KeyCode.T);
    }

    public bool PressTrigger()
    {
        return Input.GetKey(KeyCode.T);
    }

    public bool PressUpTrigger()
    {
        return Input.GetKeyUp(KeyCode.T);
    }

    public bool PressDownTouchpad()
    {
        throw new System.NotImplementedException();
    }

    public bool PressTouchpad()
    {
        throw new System.NotImplementedException();
    }

    public bool PressUpTouchpad()
    {
        throw new System.NotImplementedException();
    }

    public bool PressUpSideTouchpad()
    {
        return Input.GetKeyDown(KeyCode.UpArrow);
    }

    public bool PressDownSideTouchpad()
    {
        return Input.GetKeyDown(KeyCode.DownArrow);
    }

    public bool PressRightSideTouchpad()
    {
        return Input.GetKeyDown(KeyCode.RightArrow);
    }

    public bool PressLeftSideTouchpad()
    {
        return Input.GetKeyDown(KeyCode.LeftArrow);
    }

    public bool PressUpUpSideTouchpad()
    {
        throw new System.NotImplementedException();
    }

    public bool PressUpDownSideTouchpad()
    {
        throw new System.NotImplementedException();
    }

    public bool PressUpRightSideTouchpad()
    {
        throw new System.NotImplementedException();
    }

    public bool PressUpLeftSideTouchpad()
    {
        throw new System.NotImplementedException();
    }

    public bool PressDownGrip()
    {
        return Input.GetKeyDown(KeyCode.G);
    }

    public bool PressGrip()
    {
        throw new System.NotImplementedException();
    }

    public bool PressUpGrip()
    {
        throw new System.NotImplementedException();
    }

    public bool PressDownApplicationMenu()
    {
        return Input.GetKeyDown(KeyCode.A);
    }

    public bool PressApplicationMenu()
    {
        throw new System.NotImplementedException();
    }

    public bool PressUpApplicationMenu()
    {
        throw new System.NotImplementedException();
    }

    public Vector3 DifferenceLocalPosition()
    {
        if (Input.GetKey(KeyCode.L))
            return Vector3.right * 0.01f;
        else if (Input.GetKey(KeyCode.K))
            return -Vector3.right * 0.01f;
        else if (Input.GetKey(KeyCode.I))
            return Vector3.up * 0.01f;
        else if (Input.GetKey(KeyCode.M))
            return -Vector3.up * 0.01f;

        return Vector3.zero;
    }

    public Quaternion DifferenceLocalRotation()
    {
        throw new System.NotImplementedException();
    }

    public Vector3 ControllerVelocity()
    {
        if (Input.GetKey(KeyCode.L))
            return Vector3.right;
        else if (Input.GetKey(KeyCode.K))
            return -Vector3.right;
        else if (Input.GetKey(KeyCode.I))
            return Vector3.up;
        else if (Input.GetKey(KeyCode.M))
            return -Vector3.up;

        return Vector3.zero;
    }

    public Vector3 ControllerAngularVelocity()
    {
        throw new System.NotImplementedException();
    }

    public void HapticPulse()
    {
        throw new System.NotImplementedException();
    }

    public bool PressingAnyButton()
    {
        throw new System.NotImplementedException();
    }
}
}