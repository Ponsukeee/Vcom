using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace VRUtils.Components
{
public class ViveInputDeviceSupport : IInputDevice
{
    private Hand hand;

    private float TouchPositionX => SteamVR_Input._default.inActions.TouchpadAxis.GetAxis(hand.handType).x;
    private float TouchPositionY => SteamVR_Input._default.inActions.TouchpadAxis.GetAxis(hand.handType).y;

    public ViveInputDeviceSupport(Hand hand)
    {
        this.hand = hand;
    }

    //Trigger
    public bool PressDownTrigger()
    {
        if(SteamVR_Input._default.inActions.Trigger.GetStateDown(hand.handType))
        {
            SteamVR_Input._default.outActions.Haptic.Execute(0f, 0.1f, 1f, 0.5f, hand.handType);
            return true;
        }

        return false;
    }

    public bool PressTrigger()
    {
        return SteamVR_Input._default.inActions.Trigger.GetState(hand.handType);
    }

    public bool PressUpTrigger()
    {
        return SteamVR_Input._default.inActions.Trigger.GetStateUp(hand.handType);
    }

    //Touchpad
    public bool PressDownTouchpad()
    {
        return SteamVR_Input._default.inActions.Touchpad.GetStateDown(hand.handType);
    }

    public bool PressUpTouchpad()
    {
        return SteamVR_Input._default.inActions.Touchpad.GetStateUp(hand.handType);
    }

    public bool PressTouchpad()
    {
        return SteamVR_Input._default.inActions.Touchpad.GetState(hand.handType);
    }

    public bool PressUpSideTouchpad()
    {
        if (PressTouchpad())
            return (TouchPositionY / TouchPositionX > 1 || TouchPositionY / TouchPositionX < -1) && TouchPositionY > 0;

        return false;
    }

    public bool PressDownSideTouchpad()
    {
        if (PressTouchpad())
            return (TouchPositionY / TouchPositionX > 1 || TouchPositionY / TouchPositionX < -1) && TouchPositionY < 0;

        return false;
    }

    public bool PressRightSideTouchpad()
    {
        if (PressTouchpad())
            return (TouchPositionX / TouchPositionY > 1 || TouchPositionX / TouchPositionY < -1) && TouchPositionX > 0;

        return false;
    }

    public bool PressLeftSideTouchpad()
    {
        if (PressTouchpad())
            return (TouchPositionX / TouchPositionY > 1 || TouchPositionX / TouchPositionY < -1) && TouchPositionX < 0;

        return false;
    }
    
    public bool PressUpUpSideTouchpad()
    {
        if (PressUpTouchpad())
            return (TouchPositionY / TouchPositionX > 1 || TouchPositionY / TouchPositionX < -1) && TouchPositionY > 0;

        return false;
    }

    public bool PressUpDownSideTouchpad()
    {
        if (PressUpTouchpad())
            return (TouchPositionY / TouchPositionX > 1 || TouchPositionY / TouchPositionX < -1) && TouchPositionY < 0;

        return false;
    }

    public bool PressUpRightSideTouchpad()
    {
        if (PressUpTouchpad())
            return (TouchPositionX / TouchPositionY > 1 || TouchPositionX / TouchPositionY < -1) && TouchPositionX > 0;

        return false;
    }

    public bool PressUpLeftSideTouchpad()
    {
        if (PressUpTouchpad())
            return (TouchPositionX / TouchPositionY > 1 || TouchPositionX / TouchPositionY < -1) && TouchPositionX < 0;

        return false;
    }

    public bool PressDownGrip()
    {
        return SteamVR_Input._default.inActions.GrabGrip.GetStateDown(hand.handType);
    }

    public bool PressGrip()
    {
        return SteamVR_Input._default.inActions.GrabGrip.GetState(hand.handType);
    }

    public bool PressUpGrip()
    {
        return SteamVR_Input._default.inActions.GrabGrip.GetStateUp(hand.handType);
    }

    public bool PressDownApplicationMenu()
    {
        return SteamVR_Input._default.inActions.ApplicationMenu.GetStateDown(hand.handType);
    }

    public bool PressApplicationMenu()
    {
        return SteamVR_Input._default.inActions.ApplicationMenu.GetState(hand.handType);
    }

    public bool PressUpApplicationMenu()
    {
        return SteamVR_Input._default.inActions.ApplicationMenu.GetStateUp(hand.handType);
    }

    public Vector3 DifferenceLocalPosition()
    {
        return SteamVR_Input._default.inActions.Pose.GetLocalPosition(hand.handType) -
               SteamVR_Input._default.inActions.Pose.GetLastLocalPosition(hand.handType);
    }

    public Quaternion DifferenceLocalRotation()
    {
        return SteamVR_Input._default.inActions.Pose.GetLocalRotation(hand.handType) *
               Quaternion.Inverse(SteamVR_Input._default.inActions.Pose.GetLastLocalRotation(hand.handType));
    }

    public Vector3 ControllerVelocity()
    {
        return hand.GetTrackedObjectVelocity();
    }

    public Vector3 ControllerAngularVelocity()
    {
        return hand.GetTrackedObjectAngularVelocity();
    }

    public void HapticPulse()
    {
        SteamVR_Input._default.outActions.Haptic.Execute(0f, 0.1f, 1f, 1000f, hand.handType);
    }

    public bool PressingAnyButton()
    {
        return PressTrigger() || PressGrip() || PressTouchpad() || PressApplicationMenu();
    }
}
}