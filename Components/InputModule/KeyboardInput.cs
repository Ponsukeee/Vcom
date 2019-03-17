using UnityEngine;

namespace VRUtils.InputModule
{
public class KeyboardInput : MonoBehaviour, IInputDevice
{
    public bool ClickDown()
    {
        return Input.GetKeyDown(KeyCode.T);
    }

    public bool Clicking()
    {
        return Input.GetKey(KeyCode.T);
    }

    public bool ClickUp()
    {
        return Input.GetKeyUp(KeyCode.T);
    }

    public bool PadDown()
    {
        throw new System.NotImplementedException();
    }

    public bool PadPressing()
    {
        throw new System.NotImplementedException();
    }

    public bool PadUp()
    {
        throw new System.NotImplementedException();
    }

    public bool UpSidePadPressing()
    {
        return Input.GetKeyDown(KeyCode.UpArrow);
    }

    public bool DownSidePadPressing()
    {
        return Input.GetKeyDown(KeyCode.DownArrow);
    }

    public bool RightSidePadPressing()
    {
        return Input.GetKeyDown(KeyCode.RightArrow);
    }

    public bool LeftSidePadPressing()
    {
        return Input.GetKeyDown(KeyCode.LeftArrow);
    }

    public bool UpSidePadUp()
    {
        throw new System.NotImplementedException();
    }

    public bool DownSidePadUp()
    {
        throw new System.NotImplementedException();
    }

    public bool RightSidePadUp()
    {
        throw new System.NotImplementedException();
    }

    public bool LeftSidePadUp()
    {
        throw new System.NotImplementedException();
    }

    public bool SubClickDown()
    {
        return Input.GetKeyDown(KeyCode.G);
    }

    public bool SubClicking()
    {
        throw new System.NotImplementedException();
    }

    public bool SubClickUp()
    {
        throw new System.NotImplementedException();
    }

    public bool MenuDown()
    {
        return Input.GetKeyDown(KeyCode.A);
    }

    public bool MenuPressing()
    {
        throw new System.NotImplementedException();
    }

    public bool MenuUp()
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