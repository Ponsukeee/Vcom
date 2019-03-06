using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRUtils.Components;
using VRUtils.InputModule;

namespace VRUtils.Components
{
public class GrabbableBase : MonoBehaviour
{
    private float velocity;
    private bool hasKinematic;
    private Rigidbody rb;
//    private Vector3 lastForward;
    private Vector3 hitPointFromCenter;
    private readonly List<DeviceInfo> grabbingDevices = new List<DeviceInfo>();
    
    private static readonly float MAX_VELOCITY = 6f;
    private static readonly float ACCELERATION = 0.3f;
    private static readonly float MIN_DISTANCE = 0.1f;
    private static readonly float MAX_DISTANCE = 3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
            hasKinematic = rb.isKinematic;
    }

    private void OnTriggerExit(Collider other)
    {
        var inputController = other.GetComponent<InputController>();
        if (inputController == null) return;

        grabbingDevices.Remove(inputController.DeviceInfo);

        if (hasKinematic == false && rb != null && !grabbingDevices.Any())
        {
            rb.isKinematic = false;
        }
    }

    protected void Grab(DeviceInfo deviceInfo)
    {
        velocity = 0;

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        }

        RaycastHit hitInfo;
        Physics.Raycast(deviceInfo.transform.position, deviceInfo.transform.forward, out hitInfo, MAX_DISTANCE + 1f);
        hitPointFromCenter = hitInfo.point - transform.position;
//        lastForward = deviceInfo.transform.forward;

        grabbingDevices.Add(deviceInfo);
    }

    protected void Drag(DeviceInfo deviceInfo)
    {
        if (!grabbingDevices.Any() ||
            grabbingDevices[grabbingDevices.Count - 1] != deviceInfo) return;

        float angle;
        Vector3 axis;
        deviceInfo.GetDiffRotation().ToAngleAxis(out angle, out axis);

//        var diffDirection = Distance(deviceInfo) * deviceInfo.transform.forward - Distance(deviceInfo) * lastForward;
//        lastForward = deviceInfo.transform.forward;
        
        transform.root.position += deviceInfo.GetDiffPosition() + DiffDistance(deviceInfo);
        transform.root.RotateAround(deviceInfo.transform.position, axis, angle);
    }

    protected void Release(DeviceInfo deviceInfo)
    {
        grabbingDevices.Remove(deviceInfo);
        velocity = 0;

        if (rb != null && !grabbingDevices.Any())
        {
            if (hasKinematic == false)
                rb.isKinematic = false;

            if (rb.useGravity == true)
            {
                rb.velocity = deviceInfo.GetVelocity();
                rb.angularVelocity = deviceInfo.GetAngularVelocity();
            }
        }
    }

    private float Distance(DeviceInfo deviceInfo)
    {
        return ((transform.position + hitPointFromCenter) - deviceInfo.transform.position).magnitude;
    }

    private Vector3 DiffDistance(DeviceInfo deviceInfo)
    {
        var distance = Distance(deviceInfo);
        bool isClosest = velocity < 0 && distance < MIN_DISTANCE;
        bool isFarthest = velocity > 0 && distance > MAX_DISTANCE;
        if (isClosest || isFarthest)
            return Vector3.zero;

        return (velocity * Time.deltaTime) * deviceInfo.transform.forward;
    }
    
    protected void MoveForward()
    {
        if (!grabbingDevices.Any()) return;

        if (velocity < 0)
            velocity = 0;
        velocity += ACCELERATION;
        if (velocity > MAX_VELOCITY)
            velocity = MAX_VELOCITY;
    }

    protected void MoveBack()
    {
        if (!grabbingDevices.Any()) return;

        if (velocity > 0)
            velocity = 0;
        velocity -= ACCELERATION;
        if (velocity < -MAX_VELOCITY)
            velocity = -MAX_VELOCITY;
    }
}
}