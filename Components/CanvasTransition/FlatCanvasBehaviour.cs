using UnityEngine;
using UnityEngine.UI;

namespace VRUtils.Components
{
public class FlatCanvasBehaviour : CanvasBehaviourBase
{
    [SerializeField] private float margin;

    private static readonly float Moving_Time = 0.03f;

    public override Vector3 PositionOfMoveAxis(int indexOfTarget, Vector3 centerPosition, int moveAxis)
    {
        var sizeOfCanvas = Vector3.zero;
        sizeOfCanvas[moveAxis] = (CanvasSize()[moveAxis] + margin);
//        sizeOfCanvas[moveAxis] = (tf.root.localScale[moveAxis] + margin) * tf.localScale[moveAxis];

        var targetPosition = Vector3.zero;
        targetPosition[moveAxis] = sizeOfCanvas[moveAxis] * (indexOfTarget - centerPosition[moveAxis]);
        tf.localRotation = Quaternion.identity;
        tf.parent.localRotation = Quaternion.identity;

        return targetPosition;
    }

    public override Vector3 PositionOfActionAxis(Vector3 centerPosition, int actionAxis)
    {
        var targetPosition = Vector3.zero;
        targetPosition[actionAxis] = centerPosition[actionAxis] * transform.localScale[actionAxis];

        return targetPosition;
    }
}
}