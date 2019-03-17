using UnityEngine;

namespace VRUtils.InputModule
{
public class LaserAppearanceManager : MonoBehaviour
{
    private LineRenderer line;
    private LaserSelector laserSelector;

    private RaycastHit handlingHitInfo;
    private Vector3 hitPointFromCenter;
    private Quaternion lastRotation;

    private static readonly int MIDDLE_COUNTS = 30;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        laserSelector = GetComponent<LaserSelector>();
        laserSelector.onChangedHandlingModule.AddListener(Initialize);

        line.SetPosition(1, Vector3.forward * laserSelector.MaxDistance);
        line.enabled = false;
    }

    private void FixedUpdate()
    {
        line.enabled = laserSelector.enabled;
        if (!line.enabled) return;

        if (laserSelector.IsHandling)
        {
            Bend();
        }
        else
        {
            AdjustLineLength();
        }
    }

    private void AdjustLineLength()
    {
        line.positionCount = 2;
        if (laserSelector.IsHit)
            line.SetPosition(1, Vector3.forward * laserSelector.HitInfo.distance);
        else
            line.SetPosition(1, Vector3.forward * laserSelector.MaxDistance);
    }

    private void Bend()
    {
        if (handlingHitInfo.transform == null) return;
        var totalPoints = MIDDLE_COUNTS + 2;
        var start = Vector3.zero;
        var control = Vector3.forward * (handlingHitInfo.transform.position - transform.position).magnitude / 2;

        var diffRotation = handlingHitInfo.transform.rotation * Quaternion.Inverse(lastRotation);
        var end = transform.InverseTransformPoint(handlingHitInfo.transform.position + diffRotation * hitPointFromCenter);

        line.SetPosition(0, start);
        for (int i = 1; i <= MIDDLE_COUNTS; i++)
        {
            var t = (float) i / (float) (totalPoints - 1);
            var pos = GetVezierCurvePoint(start, end, control, t);
            line.SetPosition(i, pos);
        }

        line.SetPosition(totalPoints - 1, end);
    }

    private void Initialize()
    {
        var isHit = Physics.Raycast(transform.position, transform.forward, out handlingHitInfo, laserSelector.MaxDistance);
        if (!isHit) return;

        hitPointFromCenter = handlingHitInfo.point - handlingHitInfo.transform.position;
        lastRotation = handlingHitInfo.transform.rotation;
        line.positionCount = MIDDLE_COUNTS + 2;
    }

    public static Vector3 GetVezierCurvePoint(Vector3 start, Vector3 end, Vector3 control, float t)
    {
        var p1 = Vector3.Lerp(start, control, t);
        var p2 = Vector3.Lerp(control, end, t);
        var p3 = Vector3.Lerp(p1, p2, t);

        return p3;
    }
}
}