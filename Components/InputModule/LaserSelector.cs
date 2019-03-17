using UnityEngine;
using UnityEngine.Events;

namespace VRUtils.InputModule
{
[RequireComponent(typeof(LineRenderer))]
public class LaserSelector : MonoBehaviour, IInputModuleSelector
{
    public float MaxDistance { get; } = 5f;
    public RaycastHit HitInfo { get; private set; }
    public bool IsHit { get; private set; }
    
    public UnityEvent onSelect = new UnityEvent();
    private IInputModule currentInputModule;
    private IInputModule handlingModule;
    public bool IsHandling => handlingModule != null;

    public UnityEvent onChangedHandlingModule = new UnityEvent();
    
    private Transform tf;

    private void Awake()
    {
        tf = transform;
    }

    private void FixedUpdate()
    {
        RaycastHit raycastHit;
        IsHit = Physics.Raycast(tf.position, tf.forward, out raycastHit, MaxDistance);
        HitInfo = raycastHit;
        
        if (IsHit)
        {
            SetInputModule();
        }
        else if (currentInputModule != null)
        {
            RemoveInputModule();
        }
    }

    private void SetInputModule()
    {
        var inputModule = HitInfo.transform.GetComponent<IInputModule>();
        if (currentInputModule == inputModule || IsHandling) return;

        if (inputModule == null)
        {
            RemoveInputModule();
            return;
        }
        onSelect.Invoke();

        currentInputModule?.OnUnset();
        currentInputModule = inputModule;
        currentInputModule.OnSet();
    }

    private void RemoveInputModule()
    {
        if (handlingModule != null) return;
        if (currentInputModule == null) return;
        currentInputModule?.OnUnset();
        currentInputModule = null;
    }

    public IInputModule GetCurrentModule()
    {
        return currentInputModule;
    }

    public void NotifyHandlingModule(IInputModule handlingModule)
    {
        if (this.handlingModule != handlingModule)
        {
            onChangedHandlingModule?.Invoke();
        }
        this.handlingModule = handlingModule;
    }

    private void OnDisable()
    {
        currentInputModule?.OnUnset();
        currentInputModule = null;
        handlingModule = null;
    }
}
}