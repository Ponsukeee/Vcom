using UnityEngine;
using UnityEngine.UI;

namespace VRUtils.Components
{
public class DepthCanvasBehaviour : CanvasBehaviourBase
{
    [SerializeField] private float depthRate = 0.1f;
    [SerializeField] private float scaleRate = 0.2f;

    private static readonly float Moving_Time = 0.03f;

    private Vector3 originalScale;

    protected override void OnAwake()
    {
        originalScale = transform.localScale;
    }

    public override Vector3 PositionOfMoveAxis(int indexOfTarget, Vector3 centerPosition, int moveAxis)
    {
        var velocity = Vector3.zero;
        var diffFromCenter = Mathf.Abs(indexOfTarget - centerPosition[moveAxis]);
        
        var diffScale = diffFromCenter * scaleRate;
        var targetScale = originalScale - Vector3.one * diffScale;
        transform.localScale = Vector3.SmoothDamp(transform.localScale, targetScale, ref velocity, Moving_Time);

        var targetPosition = Vector3.zero;
        targetPosition[moveAxis] = (indexOfTarget - centerPosition[moveAxis]) * CanvasSize()[moveAxis] / 2;
        targetPosition[2] = diffFromCenter * depthRate;

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