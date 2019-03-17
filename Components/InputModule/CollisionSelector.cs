using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace VRUtils.InputModule
{
public class CollisionSelector : MonoBehaviour, IInputModuleSelector
{
    private readonly List<IInputModule> collidingCandidates = new List<IInputModule>();
    private IInputModule currentInputModule;
    private IInputModule handlingModule;
    public UnityEvent onSelect = new UnityEvent();

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;
        var inputModule = other.GetComponent<IInputModule>();
        if (inputModule == null) return;

        collidingCandidates.Add(inputModule);
        SetInputModule(inputModule);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!enabled) return;
        var inputModule = other.GetComponent<IInputModule>();

        if (inputModule == null) return;
        collidingCandidates.Remove(inputModule);

        if (currentInputModule != inputModule) return;
        RemoveInputModule();
    }

    private void SetInputModule(IInputModule inputModule)
    {
        if (currentInputModule == inputModule || handlingModule != null) return;

        onSelect.Invoke();

        currentInputModule?.OnUnset();
        currentInputModule = inputModule;
        currentInputModule.OnSet();
    }

    private void RemoveInputModule()
    {
        if (collidingCandidates.Any())
        {
            SetInputModule(collidingCandidates[0]);
        }
        else
        {
            currentInputModule?.OnUnset();
            currentInputModule = null;
        }
    }

    public IInputModule GetCurrentModule()
    {
        return currentInputModule;
    }

    public void NotifyHandlingModule(IInputModule handlingModule)
    {
        this.handlingModule = handlingModule;
    }

    private void OnDisable()
    {
        handlingModule = null;
        collidingCandidates.Clear();
        currentInputModule = null;
    }
}
}