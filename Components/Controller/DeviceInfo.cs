using UnityEngine;
using VRUtils.Components;

namespace VRUtils.Components
{
public class DeviceInfo
{
    private readonly GameObject deviceObject;
    private readonly IInputDevice inputDevice;
    public Transform transform { get; private set; }

    public DeviceInfo(GameObject deviceObject, IInputDevice inputDevice)
    {
        this.deviceObject = deviceObject;
        this.inputDevice = inputDevice;
        transform = deviceObject.transform;
    }

    public GameObject GetDeviceObject()
    {
        return deviceObject;
    }

    public Vector3 GetDiffPosition()
    {
        return inputDevice.DifferenceLocalPosition();
    }

    public Quaternion GetDiffRotation()
    {
        return inputDevice.DifferenceLocalRotation();
    }

    public Vector3 GetVelocity()
    {
        return inputDevice.ControllerVelocity();
    }

    public Vector3 GetAngularVelocity()
    {
        return inputDevice.ControllerAngularVelocity();
    }
}
}