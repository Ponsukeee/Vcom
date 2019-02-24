using UnityEngine;

namespace VRUtils.Components
{
public class Glowable : MonoBehaviour, IGlowable
{
    [SerializeField] private Color glowColor;
    [SerializeField] private float lerpFactor;

    private Color targetColor;
    public Color CurrentColor { get; private set; }
    
    public Renderer[] Renderers { get; private set; }
    
    private void Start()
    {
        Renderers = GetComponentsInChildren<Renderer>();
        GlowOutlineController.AddGlowObject(this);
    }

    private void Update()
    {
        CurrentColor = Color.Lerp(CurrentColor, targetColor, lerpFactor * Time.deltaTime);
//        CurrentColor = targetColor;
        if (CurrentColor.Equals(targetColor))
        {
            this.enabled = false;
        }
    }

    public void Glow()
    {
        enabled = true;
        targetColor = glowColor;
    }

    public void Darken()
    {
        enabled = true;
        targetColor = Color.black;
    }
    
    private void OnEnable()
    {
        GlowOutlineController.AddGlowObject(this);
    }

    private void OnDisable()
    {
        GlowOutlineController.RemoveGlowObject(this);
    }

    private void OnDestroy()
    {
        GlowOutlineController.RemoveGlowObject(this);
    }
}
}