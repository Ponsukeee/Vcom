using Components.Controller;
using UnityEngine;
using VRUtils.Components;
using VRUtils.InputModule;

namespace VRUtils.Components
{
public class Generator : MonoBehaviour, IInputModule
{
    [SerializeField] private GameObject generatingObject;

    private IInputModule Generate(InputType input, DeviceInfo deviceInfo)
    {
        var generated = Instantiate(generatingObject);
        generated.transform.position = GetComponent<Collider>().ClosestPointOnBounds(deviceInfo.transform.position);
        generated.transform.localRotation = transform.localRotation;
        generated.GetComponent<IGeneratable>()?.OnGenerate();
        
        var dragging = generated.AddComponent<GenerateDragging>();
        return dragging.HandleInput(input, deviceInfo);
    }

    public IInputModule HandleInput(InputType input, DeviceInfo deviceInfo)
    {
        switch (input)
        {
            case InputType.Click:
                return Generate(input, deviceInfo);
        }

        return null;
    }

    public void OnSet()
    {
        GetComponent<IGlowable>().Glow();
    }

    public void OnUnset()
    {
        GetComponent<IGlowable>().Darken();
    }
}
}