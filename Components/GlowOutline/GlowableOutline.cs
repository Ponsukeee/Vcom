using UnityEngine;
using VRUtils.Components;

namespace VRUtils.Components
{
public class GlowableOutline : MonoBehaviour, IGlowable
{
    [SerializeField] private Color glowColor;
    
    private Renderer rend;
    private Color defaultColor;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        defaultColor = rend.material.GetColor("_GlowColor");
    }

    public void Glow()
    {
        rend.material.SetColor("_GlowColor", glowColor);
        rend.material.SetColor("_Color0", glowColor);
    }

    public void Darken()
    {
        if(rend == null) return;
        rend.material.SetColor("_GlowColor", defaultColor);
        rend.material.SetColor("_Color0", defaultColor);
    }
}
}