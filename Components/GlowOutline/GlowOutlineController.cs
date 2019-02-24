using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace VRUtils.Components
{
public class GlowOutlineController : MonoBehaviour
{
    private static readonly List<Glowable> GlowableList = new List<Glowable>();

    [SerializeField, Range(0, 2)] private float bold;

    private CommandBuffer commandBuffer;

    private Material glowMat;
    private Material blurMat;
    private Vector2 blurSize;

    private int prePassTexID;
    private int blurTexID;
    private int tempTexID;
    private int blurSizeID;
    private int glowColorID;

    void Start()
    {
        commandBuffer = new CommandBuffer();
        GetComponent<Camera>().AddCommandBuffer(CameraEvent.BeforeImageEffects, commandBuffer);

        glowMat = new Material(Shader.Find("Glow"));
        blurMat = new Material(Shader.Find("Blur"));

        blurSize = new Vector2(bold / (Screen.width >> 1), bold / (Screen.height >> 1));

        prePassTexID = Shader.PropertyToID("_PrePassTex");
        blurTexID = Shader.PropertyToID("_BlurTex");
        tempTexID = Shader.PropertyToID("_TempTex");
        blurSizeID = Shader.PropertyToID("_BlurSize");
        glowColorID = Shader.PropertyToID("_GlowColor");
    }

    public static void AddGlowObject(Glowable glowable)
    {
        GlowableList.Add(glowable);
    }

    public static void RemoveGlowObject(Glowable glowable)
    {
        GlowableList.Remove(glowable);
    }

    void Update()
    {
        BuildCommand();
    }

    private void BuildCommand()
    {
        commandBuffer.Clear();

        commandBuffer.GetTemporaryRT(prePassTexID, Screen.width, Screen.height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32,
            RenderTextureReadWrite.Default, QualitySettings.antiAliasing);
        commandBuffer.SetRenderTarget(prePassTexID);
        commandBuffer.ClearRenderTarget(true, true, Color.clear);

        foreach (var glowable in GlowableList)
        {
            commandBuffer.SetGlobalColor(glowColorID, glowable.CurrentColor);
            foreach (var rend in glowable.Renderers)
            {
                commandBuffer.DrawRenderer(rend, glowMat);
            }
        }

        commandBuffer.GetTemporaryRT(blurTexID, Screen.width >> 1, Screen.height >> 1, 0, FilterMode.Bilinear);
        commandBuffer.GetTemporaryRT(tempTexID, Screen.width >> 1, Screen.height >> 1, 0, FilterMode.Bilinear);

        commandBuffer.Blit(prePassTexID, blurTexID);
        commandBuffer.SetGlobalVector(blurSizeID, blurSize);
        for (int i = 0; i < 2; i++)
        {
            commandBuffer.Blit(blurTexID, tempTexID, blurMat, 0);
            commandBuffer.Blit(tempTexID, blurTexID, blurMat, 1);
        }
    }
}
}