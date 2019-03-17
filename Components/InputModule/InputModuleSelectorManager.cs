using UnityEngine;

namespace VRUtils.InputModule
{
public class InputModuleSelectorManager
{
    public IInputModuleSelector CurrentSelector { get; private set; }
    private readonly CollisionSelector collisionSelector;
    private readonly LaserSelector laserSelector;

    public InputModuleSelectorManager(GameObject deviceObject, IInputDevice inputDevice)
    {
        collisionSelector = deviceObject.AddComponent<CollisionSelector>();
        laserSelector = deviceObject.AddComponent<LaserSelector>();

        collisionSelector.onSelect.AddListener(inputDevice.HapticPulse);
        laserSelector.onSelect.AddListener(inputDevice.HapticPulse);
        
        deviceObject.AddComponent<LaserAppearanceManager>();

        CurrentSelector = collisionSelector;
        laserSelector.enabled = false;
    }

    public void SwitchSelector()
    {
        if (CurrentSelector.Equals(collisionSelector))
        {
            collisionSelector.enabled = false;
            laserSelector.enabled = true;
            CurrentSelector = laserSelector;
        }
        else
        {
            laserSelector.enabled = false;
            collisionSelector.enabled = true;
            CurrentSelector = collisionSelector;
        }
    }
}
}