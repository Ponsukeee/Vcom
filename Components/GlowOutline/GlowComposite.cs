using UnityEngine;

namespace VRUtils.Components
{
public class GlowComposite : MonoBehaviour
{
    [SerializeField] private float intensity;
    
    private Material compositeMat;
    
    void Start()
    {
        compositeMat = new Material(Shader.Find("GlowComposite"));
    }
    
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        compositeMat.SetFloat("_Intensity", intensity);
        Graphics.Blit(src, dest, compositeMat);
    }
}
}