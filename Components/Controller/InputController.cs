using System.Collections;
using Components.Controller;
using UnityEngine;

namespace VRUtils.Components
{
public enum InputType
{
    Click,
    DoubleClick,
    Drag,
    Release,

    SubClick,
    SubDoubleClick,
    SubDrag,
    SubRelease,

    Up,
    Down,
    Right,
    Left,

    ReleasePad,
    ReleaseUp,
    ReleaseDown,
    ReleaseRight,
    ReleaseLeft,
}

public class InputController : MonoBehaviour
{
    private IInputDevice inputDevice;
    public DeviceInfo DeviceInfo { get; private set; }
    private bool isPressingAnyButton;
    private bool hasClicked;
    private bool hasSubClicked;

    private IInputModule handlingModule;
    private InputModuleSelectorManager selectorManager;
    [SerializeField] private GestureInputModule gestureInputModule;
    
    [SerializeField, Range(10, 50)] private int intervalOfDoubleClick = 30;
    [SerializeField] private InputDeviceAssignor.DeviceType deviceType;

    private bool IsHandling => handlingModule != null;

    private void Awake()
    {
        inputDevice = gameObject.AddComponent<InputDeviceAssignor>().AssignInputDevice(deviceType);
        DeviceInfo = new DeviceInfo(gameObject, inputDevice);
        selectorManager = new InputModuleSelectorManager(gameObject, inputDevice);
    }

    private void Update()
    {
        ReceiveInput();
    }

    private void LateUpdate()
    {
        isPressingAnyButton = inputDevice.PressingAnyButton();
    }

    private IEnumerator JudgeDoubleClick(InputType inputType)
    {
        for (int i = 0; i < intervalOfDoubleClick; i++)
        {
            if (hasClicked && inputDevice.PressDownTrigger())
            {
                hasClicked = false;
                SendInput(inputType);
                yield break;
            }

            yield return null;
        }

        hasClicked = false;
    }

    private IEnumerator JudgeSubDoubleClick(InputType inputType)
    {
        for (int i = 0; i < intervalOfDoubleClick; i++)
        {
            if (hasSubClicked && inputDevice.PressDownGrip())
            {
                hasSubClicked = false;
                SendInput(inputType);
                yield break;
            }

            yield return null;
        }

        hasSubClicked = false;
    }

    private void SendInput(InputType inputType)
    {
        if (!IsHandling && isPressingAnyButton) return;
        
        if (IsHandling)
        {
            handlingModule = handlingModule.HandleInput(inputType, DeviceInfo);
            selectorManager.CurrentSelector.NotifyHandlingModule(handlingModule);
        }
        else
        {
            var currentModule = selectorManager.CurrentSelector.GetCurrentModule();
            if (currentModule == null)
            {
                handlingModule = gestureInputModule.HandleInput(inputType, DeviceInfo);
                return;
            }
            handlingModule = currentModule.HandleInput(inputType, DeviceInfo);
            selectorManager.CurrentSelector.NotifyHandlingModule(handlingModule);
        }
    }

    private void ReceiveInput()
    {
        if (inputDevice.PressDownApplicationMenu())
        {
            selectorManager.SwitchSelector();
            handlingModule = null;
        }

        if (inputDevice.PressDownTrigger())
        {
            if (hasClicked == false)
            {
                SendInput(InputType.Click);
                StartCoroutine(JudgeDoubleClick(InputType.DoubleClick));
                hasClicked = true;
            }
        }

        if (inputDevice.PressTrigger())
        {
            if (inputDevice.PressDownTrigger()) return;

            SendInput(InputType.Drag);
        }

        if (inputDevice.PressUpTrigger())
        {
            SendInput(InputType.Release);
        }

        if (inputDevice.PressDownGrip())
        {
            if (hasSubClicked == false)
            {
                SendInput(InputType.SubClick);
                StartCoroutine(JudgeSubDoubleClick(InputType.SubDoubleClick));
                hasSubClicked = true;
            }
        }

        if (inputDevice.PressGrip())
        {
            SendInput(InputType.SubDrag);
        }

        if (inputDevice.PressUpGrip())
        {
            SendInput(InputType.SubRelease);
        }

        if (inputDevice.PressUpSideTouchpad())
        {
            SendInput(InputType.Up);
        }

        if (inputDevice.PressDownSideTouchpad())
        {
            SendInput(InputType.Down);
        }

        if (inputDevice.PressRightSideTouchpad())
        {
            SendInput(InputType.Right);
        }

        if (inputDevice.PressLeftSideTouchpad())
        {
            SendInput(InputType.Left);
        }

        if (inputDevice.PressUpTouchpad())
        {
            SendInput(InputType.ReleasePad);
        }

        if (inputDevice.PressUpUpSideTouchpad())
        {
            SendInput(InputType.ReleaseUp);
        }

        if (inputDevice.PressUpDownSideTouchpad())
        {
            SendInput(InputType.ReleaseDown);
        }

        if (inputDevice.PressUpRightSideTouchpad())
        {
            SendInput(InputType.ReleaseRight);
        }

        if (inputDevice.PressUpLeftSideTouchpad())
        {
            SendInput(InputType.ReleaseLeft);
        }
    }

    //for debug
    public string GetNameOfCurrentInputModule()
    {
        return selectorManager?.CurrentSelector?.GetCurrentModule()?.ToString();
    }
}
}