using VRSNS.Core;
using UnityEngine;
using VRSNS.VRM;

namespace VRUtils.Components
{
public class VrmViewer : MonoBehaviour, IViewer
{
    public async void Display(string filePath)
    {
        var avatarData = await VRMImporter.ReadAllBytesAsync(filePath);
        await Client.Avatar.GenerateAvatar(avatarData);
    }
}
}
