using UnityEngine;
using Valve.VR;
using VRUtils.InputModule;

namespace VRUtils.InputModule
{
public class ViveInputDevice : MonoBehaviour, IInputDevice
{
    [SerializeField] private SteamVR_Action_Boolean click;
    [SerializeField] private SteamVR_Action_Boolean subClick;
    [SerializeField] private SteamVR_Action_Boolean pad;
    [SerializeField] private SteamVR_Action_Boolean menu;
    [SerializeField] private SteamVR_Action_Pose pose;
    [SerializeField] private SteamVR_Action_Vibration haptic;
    [SerializeField] private SteamVR_Action_Vector2 padAxis;
    
    private SteamVR_Input_Sources handType;
    private SteamVR_Behaviour_Pose trackedObject;

    private float TouchPositionX => padAxis.GetAxis(handType).x;
    private float TouchPositionY => padAxis.GetAxis(handType).y;

    private void Awake()
    {
        trackedObject = GetComponent<SteamVR_Behaviour_Pose>();
        handType = trackedObject.inputSource;
    }

    //Trigger
    public bool ClickDown()
    {
        if(click.GetStateDown(handType))
        {
            haptic.Execute(0f, 0.1f, 1f, 0.5f, handType);
            return true;
        }

        return false;
    }

    public bool Clicking()
    {
        return click.GetState(handType);
    }

    public bool ClickUp()
    {
        return click.GetStateUp(handType);
    }

    //Touchpad
    public bool PadDown()
    {
        return pad.GetStateDown(handType);
    }

    public bool PadUp()
    {
        return pad.GetStateUp(handType);
    }

    public bool PadPressing()
    {
        return pad.GetState(handType);
    }

    public bool UpSidePadPressing()
    {
        if (PadPressing())
            return (TouchPositionY / TouchPositionX > 1 || TouchPositionY / TouchPositionX < -1) && TouchPositionY > 0;

        return false;
    }

    public bool DownSidePadPressing()
    {
        if (PadPressing())
            return (TouchPositionY / TouchPositionX > 1 || TouchPositionY / TouchPositionX < -1) && TouchPositionY < 0;

        return false;
    }

    public bool RightSidePadPressing()
    {
        if (PadPressing())
            return (TouchPositionX / TouchPositionY > 1 || TouchPositionX / TouchPositionY < -1) && TouchPositionX > 0;

        return false;
    }

    public bool LeftSidePadPressing()
    {
        if (PadPressing())
            return (TouchPositionX / TouchPositionY > 1 || TouchPositionX / TouchPositionY < -1) && TouchPositionX < 0;

        return false;
    }
    
    public bool UpSidePadUp()
    {
        if (PadUp())
            return (TouchPositionY / TouchPositionX > 1 || TouchPositionY / TouchPositionX < -1) && TouchPositionY > 0;

        return false;
    }

    public bool DownSidePadUp()
    {
        if (PadUp())
            return (TouchPositionY / TouchPositionX > 1 || TouchPositionY / TouchPositionX < -1) && TouchPositionY < 0;

        return false;
    }

    public bool RightSidePadUp()
    {
        if (PadUp())
            return (TouchPositionX / TouchPositionY > 1 || TouchPositionX / TouchPositionY < -1) && TouchPositionX > 0;

        return false;
    }

    public bool LeftSidePadUp()
    {
        if (PadUp())
            return (TouchPositionX / TouchPositionY > 1 || TouchPositionX / TouchPositionY < -1) && TouchPositionX < 0;

        return false;
    }

    public bool SubClickDown()
    {
        return subClick.GetStateDown(handType);
    }

    public bool SubClicking()
    {
        return subClick.GetState(handType);
    }

    public bool SubClickUp()
    {
        return subClick.GetStateUp(handType);
    }

    public bool MenuDown()
    {
        return menu.GetStateDown(handType);
    }

    public bool MenuPressing()
    {
        return menu.GetState(handType);
    }

    public bool MenuUp()
    {
        return menu.GetStateUp(handType);
    }

    public Vector3 DifferenceLocalPosition()
    {
        return pose.GetLocalPosition(handType) - pose.GetLastLocalPosition(handType);
    }

    public Quaternion DifferenceLocalRotation()
    {
        return pose.GetLocalRotation(handType) * Quaternion.Inverse(pose.GetLastLocalRotation(handType));
    }

    public Vector3 ControllerVelocity()
    {
        return trackedObject.GetVelocity();
    }

    public Vector3 ControllerAngularVelocity()
    {
        return trackedObject.GetAngularVelocity();
    }

    public void HapticPulse()
    {
        haptic.Execute(0f, 0.1f, 1f, 1000f, handType);
    }

    public bool PressingAnyButton()
    {
        return Clicking() || SubClicking() || PadPressing() || MenuPressing();
    }
}
}