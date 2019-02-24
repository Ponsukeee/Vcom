using UnityEngine;

namespace VRUtils.Components
{
public class ViewCanvasBehaviour : CanvasBehaviourBase
{
    [SerializeField] private Vector3 offsetFromEye = new Vector3(0f, -0.3f, 1.5f);
    private Transform transformOfEye;

    protected override void OnAwake()
    {
        var mainCamera = Camera.main;
        if (mainCamera != null) 
            transformOfEye = mainCamera.transform;
    }

    private void FixedUpdate()
    {
        transform.LookAt(transformOfEye);
        transform.Rotate(new Vector3(0f,-180f,0f));
    }

    public override Vector3 PositionOfMoveAxis(int indexOfTarget, Vector3 centerPosition, int moveAxis)
    {
        var sizeOfCanvas = Vector3.zero;
        sizeOfCanvas[moveAxis] = CanvasSize()[moveAxis];

        var targetPosition = Vector3.zero;
        targetPosition[moveAxis] = sizeOfCanvas[moveAxis] * (indexOfTarget - centerPosition[moveAxis]);
        targetPosition += transform.parent.InverseTransformPoint(transformOfEye.position + offsetFromEye);

        return targetPosition;
    }

    public override Vector3 PositionOfActionAxis(Vector3 centerPosition, int actionAxis)
    {
        var targetPosition = Vector3.zero;
        targetPosition[actionAxis] = centerPosition[actionAxis];

        return targetPosition;
    }
}
}