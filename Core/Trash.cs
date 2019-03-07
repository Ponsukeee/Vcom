using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using VRUtils.Components;

namespace VRUtils.Components
{
public class Trash : MonoBehaviour
{
    private readonly Dictionary<Grabbable, UnityAction> actions = new Dictionary<Grabbable, UnityAction>();
    private IGlowable glowable;

    private void Awake()
    {
        glowable = GetComponent<IGlowable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var grabbable = other.GetComponent<Grabbable>();
        if (grabbable == null) return;

        UnityAction action = null;
        action = () =>
        {
            Destroy(grabbable.gameObject);
            actions.Remove(grabbable);
            grabbable.onDrop.RemoveListener(action);
            
            if(!actions.Any())
                glowable?.Darken();
        };

        grabbable.onDrop.AddListener(action);
        actions.Add(grabbable, action);
        glowable?.Glow();
    }

    private void OnTriggerExit(Collider other)
    {
        var grabbable = other.GetComponent<Grabbable>();
        if (grabbable == null) return;

        grabbable.onDrop.RemoveListener(actions[grabbable]);
        actions.Remove(grabbable);
        if(!actions.Any())
            glowable?.Darken();
    }
}
}