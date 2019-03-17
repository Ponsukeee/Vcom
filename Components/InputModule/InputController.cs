using System;
using System.Collections;
using UnityEngine;

namespace VRUtils.InputModule
{
public enum InputType
{
    Click,
    DoubleClick,
    Clicking,
    Release,

    SubClick,
    SubDoubleClick,
    SubClicking,
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

[RequireComponent(typeof(Collider))]
public class InputController : MonoBehaviour
{
    private delegate bool Predicate();
    private IInputDevice inputDevice;
    public DeviceInfo DeviceInfo { get; private set; }
    private bool isPressingAnyButton;
    private bool hasClicked;
    private bool hasSubClicked;

    private IInputModule handlingModule;
    private InputModuleSelectorManager selectorManager;
    private IInputModule defaultInputModule;

    [SerializeField] private GameObject defaultInputModuleObject;
    [SerializeField, Range(10, 50)] private int intervalOfDoubleClick = 30;

    private bool IsHandling => handlingModule != null;

    private void Awake()
    {
        inputDevice = GetComponent<IInputDevice>();
        DeviceInfo = new DeviceInfo(gameObject, inputDevice);
        selectorManager = new InputModuleSelectorManager(gameObject, inputDevice);
        
        if (defaultInputModuleObject != null)
            defaultInputModule = defaultInputModuleObject.GetComponent<IInputModule>();
    }

    private void Update()
    {
        ReceiveInput();
    }

    private void LateUpdate()
    {
        isPressingAnyButton = inputDevice.PressingAnyButton();
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
                handlingModule = defaultInputModule?.HandleInput(inputType, DeviceInfo);
                return;
            }

            handlingModule = currentModule.HandleInput(inputType, DeviceInfo);
            selectorManager.CurrentSelector.NotifyHandlingModule(handlingModule);
        }
    }

    private void ReceiveInput()
    {
        if (inputDevice.MenuDown())
        {
            selectorManager.SwitchSelector();
            handlingModule = null;
        }

        if (inputDevice.ClickDown())
        {
            if (hasClicked == false)
            {
                SendInput(InputType.Click);
                StartCoroutine(JudgeDoubleClick(InputType.DoubleClick, inputDevice.ClickDown));
                hasClicked = true;
            }
        }

        if (inputDevice.Clicking())
        {
            if (inputDevice.ClickDown()) return;

            SendInput(InputType.Clicking);
        }

        if (inputDevice.ClickUp())
        {
            SendInput(InputType.Release);
        }

        if (inputDevice.SubClickDown())
        {
            if (hasClicked == false)
            {
                SendInput(InputType.SubClick);
                StartCoroutine(JudgeDoubleClick(InputType.DoubleClick, inputDevice.SubClickDown));
                hasClicked = true;
            }
        }

        if (inputDevice.SubClicking())
        {
            SendInput(InputType.SubClicking);
        }

        if (inputDevice.SubClickUp())
        {
            SendInput(InputType.SubRelease);
        }

        if (inputDevice.UpSidePadPressing())
        {
            SendInput(InputType.Up);
        }

        if (inputDevice.DownSidePadPressing())
        {
            SendInput(InputType.Down);
        }

        if (inputDevice.RightSidePadPressing())
        {
            SendInput(InputType.Right);
        }

        if (inputDevice.LeftSidePadPressing())
        {
            SendInput(InputType.Left);
        }

        if (inputDevice.PadUp())
        {
            SendInput(InputType.ReleasePad);
        }

        if (inputDevice.UpSidePadUp())
        {
            SendInput(InputType.ReleaseUp);
        }

        if (inputDevice.DownSidePadUp())
        {
            SendInput(InputType.ReleaseDown);
        }

        if (inputDevice.RightSidePadUp())
        {
            SendInput(InputType.ReleaseRight);
        }

        if (inputDevice.LeftSidePadUp())
        {
            SendInput(InputType.ReleaseLeft);
        }
    }

    private IEnumerator JudgeDoubleClick(InputType inputType, Predicate pred)
    {
        for (int i = 0; i < intervalOfDoubleClick; i++)
        {
            if (hasClicked && pred())
            {
                hasClicked = false;
                SendInput(inputType);
                yield break;
            }

            yield return null;
        }

        hasClicked = false;
    }

    //for debug
    public string GetNameOfCurrentInputModule()
    {
        return selectorManager?.CurrentSelector?.GetCurrentModule()?.ToString();
    }
}
}