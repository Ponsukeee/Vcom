using System.Collections.Generic;
using VRUtils.Components;
using UnityEngine;
using UnityEngine.Events;

namespace VRUtils.Components
{
public class CanvasScroll : MonoBehaviour
{
    [SerializeField] private int numberToDisplay = 7;
    [SerializeField] private Direction direction = Direction.Vertical;

    private readonly List<CanvasBehaviourBase> canvasList = new List<CanvasBehaviourBase>();
    private CanvasBehaviourBase prevCenterCanvas;
    private Vector3 velocityOfCenter;
    private Vector3 moveDirection;
    private Vector3 actionDirection;
    private int moveAxis;
    private int actionAxis;
    private bool isGrabbed;

    private static readonly float MERGIN_OF_EDGE = 0.5f;
    private static readonly float ACTION_THRESHOLD = 0.2f;
    private static readonly float MOVING_TIME = 0.03f;
    private static readonly float DECELERATION_STEP = 0.005f;
    private static readonly float RELEASE_RATE = 0.0005f;
    private static readonly float DRAG_RATE = -0.1f;

    private enum Direction
    {
        Horizontal,
        Vertical,
    }

    public UnityEvent OnPlusAction { get; } = new UnityEvent();
    public UnityEvent OnMinusAction { get; } = new UnityEvent();

    private Vector3 centerPosition;
    private CanvasBehaviourBase CenterCanvas => canvasList[CenterIndex];
    private int CenterIndex => Mathf.FloorToInt(centerPosition[moveAxis] + MERGIN_OF_EDGE);

    private int CurrentIndex => canvasList.IndexOf(SelectedCanvas);
    public CanvasBehaviourBase SelectedCanvas { get; private set; }

    /// <summary>
    /// 表示できるキャンバスの中心から端までのキャンバスの枚数（中心を除く）
    /// </summary>
    private int CountFromCentralToEdge => numberToDisplay / 2;

    public int ListCount() => canvasList.Count;
    private void ShiftCenterPosition(int index) => centerPosition = moveDirection * index;

    private void Awake()
    {
        switch (direction)
        {
            case Direction.Horizontal:
                moveDirection = Vector3.right;
                moveAxis = 0;
                actionDirection = Vector3.up;
                actionAxis = 1;
                break;
            case Direction.Vertical:
                moveDirection = Vector3.up;
                moveAxis = 1;
                actionDirection = Vector3.right;
                actionAxis = 0;
                break;
        }
    }

    private void FixedUpdate()
    {
        if (canvasList.Count == 0) return;

        MoveCenterPosition(velocityOfCenter);
        DecelerateVelocity();
        if (isGrabbed == false)
            ReturnCenterPosition();

        UpdateEachCanvas();
        TransitCenterCanvas();
    }

    public void MoveCenterPosition(Vector3 movement)
    {
        var movementOnPlane = MovementOnPlane(movement);
        centerPosition += movementOnPlane;
        SuppressProtrusion();
    }

    private void DecelerateVelocity()
    {
        velocityOfCenter = Vector3.MoveTowards(velocityOfCenter, Vector3.zero, DECELERATION_STEP);
    }

    private void ReturnCenterPosition()
    {
        if (isGrabbed == true) return;
        if (Mathf.Approximately(centerPosition[moveAxis], CenterIndex) && Mathf.Approximately(centerPosition[actionAxis], 0f)) return;

        var velocity = Vector3.zero;
        var targetPosition = moveDirection * CenterIndex;
        centerPosition = Vector3.SmoothDamp(centerPosition, targetPosition, ref velocity, MOVING_TIME);
    }

    private void UpdateEachCanvas()
    {
        for (int i = 0; i < numberToDisplay; i++)
        {
            var velocity = Vector3.zero;
            var indexOfTarget = CenterIndex + (i - CountFromCentralToEdge);
            if (indexOfTarget < 0 || canvasList.Count - 1 < indexOfTarget)
                continue;

            var targetCanvas = canvasList[indexOfTarget];
            var targetPosition = Vector3.zero;
            targetPosition += targetCanvas.PositionOfMoveAxis(indexOfTarget, centerPosition, moveAxis);
            if (targetCanvas == SelectedCanvas)
                targetPosition += targetCanvas.PositionOfActionAxis(centerPosition, actionAxis);

            targetCanvas.transform.localPosition =
                Vector3.SmoothDamp(targetCanvas.transform.localPosition, targetPosition, ref velocity, MOVING_TIME);
        }
    }

    private void TransitCenterCanvas()
    {
        if (prevCenterCanvas == CenterCanvas) return;

        var afterCanvas = CenterCanvas;
        ActivateCanvas(prevCenterCanvas, afterCanvas);
        prevCenterCanvas = afterCanvas;
        centerPosition[actionAxis] = 0f;
    }

    private void ActivateCanvas(CanvasBehaviourBase prevCanvas, CanvasBehaviourBase afterCanvas)
    {
        var indexOfPrev = canvasList.IndexOf(prevCanvas);
        var indexOfAfter = canvasList.IndexOf(afterCanvas);
        int countFromCentralToEdge = numberToDisplay / 2;
        var loopTimes = Mathf.Min(numberToDisplay, Mathf.Abs(indexOfAfter - indexOfPrev));
        for (int i = 0; i < loopTimes; i++)
        {
            if (indexOfPrev < indexOfAfter)
            {
                if (indexOfPrev - countFromCentralToEdge >= 0)
                    canvasList[indexOfPrev - countFromCentralToEdge + i].gameObject.SetActive(false);
                if (indexOfAfter + countFromCentralToEdge < canvasList.Count)
                    canvasList[indexOfAfter + countFromCentralToEdge - i].gameObject.SetActive(true);
            }
            else if (indexOfPrev > indexOfAfter)
            {
                if (indexOfPrev + countFromCentralToEdge < canvasList.Count)
                    canvasList[indexOfPrev + countFromCentralToEdge - i].gameObject.SetActive(false);
                if (indexOfAfter - countFromCentralToEdge >= 0)
                    canvasList[indexOfAfter - countFromCentralToEdge + i].gameObject.SetActive(true);
            }
        }
    }

    private Vector3 MovementOnPlane(Vector3 movement)
    {
        var moveValue = Mathf.Abs(Vector3.Dot(movement.normalized, moveDirection));
        var actionValue = Mathf.Abs(Vector3.Dot(movement.normalized, actionDirection));

        if (moveValue > actionValue)
        {
            return moveValue * Vector3.Scale(movement, moveDirection);
        }
        else
            return -2f * moveValue * Vector3.Scale(movement, actionDirection);
    }

    private void SuppressProtrusion()
    {
        var maxEdge = (canvasList.Count - 1) + MERGIN_OF_EDGE - 0.01f;
        var minEdge = -1.0f + MERGIN_OF_EDGE;
        if (centerPosition[moveAxis] >= maxEdge)
        {
            centerPosition = moveDirection * maxEdge;
            velocityOfCenter = Vector3.zero;
        }
        else if (centerPosition[moveAxis] <= minEdge)
        {
            centerPosition = moveDirection * minEdge;
            velocityOfCenter = Vector3.zero;
        }
    }

    private void PlusAction(GameObject handlerObject)
    {
        centerPosition[actionAxis] = 0f;
        isGrabbed = false;
        OnPlusAction.Invoke();
    }

    private void MinusAction(GameObject handlerObject)
    {
        centerPosition[actionAxis] = 0f;
        isGrabbed = false;
        OnMinusAction.Invoke();
    }

    public void SelectCanvas(CanvasBehaviourBase canvas)
    {
        SelectedCanvas = canvas;
    }

    public void Grab()
    {
        if (isGrabbed == true) return;

        velocityOfCenter = Vector3.zero;
        isGrabbed = true;
    }

    public void Drag(Vector3 movement, GameObject handlerObject)
    {
        if (isGrabbed == false) return;

        MoveCenterPosition(DRAG_RATE * transform.InverseTransformVector(movement));
        JudgeAction(handlerObject);
    }

    private void JudgeAction(GameObject handlerObject)
    {
        if (centerPosition[actionAxis] > ACTION_THRESHOLD)
        {
            PlusAction(handlerObject);
        }

        if (centerPosition[actionAxis] < -ACTION_THRESHOLD)
        {
            MinusAction(handlerObject);
        }
    }

    public void Release(Vector3 velocity)
    {
        if (isGrabbed == false) return;

        velocityOfCenter = Vector3.Scale(moveDirection, transform.InverseTransformVector(velocity)) * -RELEASE_RATE;
        isGrabbed = false;
    }

    public GameObject AddGroup(GameObject canvas, int index = 0)
    {
        var instantiatedCanvas = Instantiate(canvas);
        var canvasBehaviour = instantiatedCanvas.GetComponent<CanvasBehaviourBase>();
        canvasList.Add(canvasBehaviour);
        instantiatedCanvas.transform.parent = transform;
        instantiatedCanvas.transform.localPosition = Vector3.zero;

        transform.localPosition = Vector3.Scale(actionDirection, CenterCanvas.CanvasSize()) * index;
        return instantiatedCanvas;
    }

    public GameObject AddCanvas(GameObject canvas, string canvasName = "")
    {
        if (canvasList.Count == 0)
        {
            return AddGroup(canvas);
        }

        var instantiatedCanvas = Instantiate(canvas);

        if (canvasName != "")
            canvas.name = canvasName;

        var canvasBehaviour = instantiatedCanvas.GetComponent<CanvasBehaviourBase>();

        instantiatedCanvas.transform.parent = transform;
        canvasList.Insert(CenterIndex + 1, canvasBehaviour);
        instantiatedCanvas.transform.localPosition = Vector3.zero;

        var prevCanvas = CenterCanvas;
        ShiftCenterPosition(CenterIndex + 1);
        var afterCanvas = CenterCanvas;
        ActivateCanvas(prevCanvas, afterCanvas);

        return instantiatedCanvas;
    }

    public void RemoveAll()
    {
        if (canvasList.Count == 0) return;

        DestroyImmediate(gameObject);
        velocityOfCenter = Vector3.zero;
        ShiftCenterPosition(0);
        canvasList.Clear();
    }

    public void RemoveCanvas()
    {
        if (canvasList.Count == 0) return;
        if (canvasList.Count == 1)
        {
            RemoveAll();
            return;
        }

        var removingCanvas = SelectedCanvas;

        var indexOfDisplaced = CenterIndex - CountFromCentralToEdge - 1;
        var indexOfDisplacedWhenLeftEdge = CountFromCentralToEdge + 1;
        if (CenterIndex == 0 && indexOfDisplacedWhenLeftEdge <= ListCount() - 1)
            canvasList[indexOfDisplacedWhenLeftEdge].gameObject.SetActive(true);
        else if (0 <= indexOfDisplaced && indexOfDisplaced <= ListCount() - 1)
            canvasList[indexOfDisplaced].gameObject.SetActive(true);
        canvasList.Remove(removingCanvas);

        if (CenterIndex != 0)
            ShiftCenterPosition(CenterIndex - 1);

        DestroyImmediate(removingCanvas.gameObject);
    }

    public bool Exist(CanvasBehaviourBase canvas)
    {
        foreach (var item in canvasList)
        {
            if (canvas == item)
                return true;
        }

        return false;
    }

    /// <summary>
    /// グループを複数制御する際に使用する
    /// </summary>
    /// <param name="index">グループリストのインデックス</param>
    public void UpdateLocalPosition(int index)
    {
        var velocity = Vector3.zero;
        var targetPositionOfGroup = Vector3.Scale(actionDirection, CenterCanvas.transform.localScale) * index;
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPositionOfGroup, ref velocity, MOVING_TIME);
    }
}
}