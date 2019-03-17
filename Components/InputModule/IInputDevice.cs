using UnityEngine;

namespace VRUtils.InputModule
{
    public interface IInputDevice
    {
        //Trigger
        bool ClickDown();
        bool Clicking();
        bool ClickUp();

        //Touchpad
        bool PadDown();
        bool PadPressing();
        bool PadUp();
        
        bool UpSidePadPressing();
        bool DownSidePadPressing();
        bool RightSidePadPressing();
        bool LeftSidePadPressing();
        
        bool UpSidePadUp();
        bool DownSidePadUp();
        bool RightSidePadUp();
        bool LeftSidePadUp();

        //Grip
        bool SubClickDown();
        bool SubClicking();
        bool SubClickUp();

        //ApplicationMenu
        bool MenuDown();
        bool MenuPressing();
        bool MenuUp();
        
        //Movement
        Vector3 DifferenceLocalPosition();
        Quaternion DifferenceLocalRotation();
        Vector3 ControllerVelocity();
        Vector3 ControllerAngularVelocity();

        void HapticPulse();

        bool PressingAnyButton();
    }
}