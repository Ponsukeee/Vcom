using UnityEngine;

namespace VRUtils.Components
{
    public interface IInputDevice
    {
        //Trigger
        bool PressDownTrigger();
        bool PressTrigger();
        bool PressUpTrigger();

        //Touchpad
        bool PressDownTouchpad();
        bool PressTouchpad();
        bool PressUpTouchpad();
        
        bool PressUpSideTouchpad();
        bool PressDownSideTouchpad();
        bool PressRightSideTouchpad();
        bool PressLeftSideTouchpad();
        
        bool PressUpUpSideTouchpad();
        bool PressUpDownSideTouchpad();
        bool PressUpRightSideTouchpad();
        bool PressUpLeftSideTouchpad();

        //Grip
        bool PressDownGrip();
        bool PressGrip();
        bool PressUpGrip();

        //ApplicationMenu
        bool PressDownApplicationMenu();
        bool PressApplicationMenu();
        bool PressUpApplicationMenu();
        
        //Movement
        Vector3 DifferenceLocalPosition();
        Quaternion DifferenceLocalRotation();
        Vector3 ControllerVelocity();
        Vector3 ControllerAngularVelocity();

        void HapticPulse();

        bool PressingAnyButton();
    }
}