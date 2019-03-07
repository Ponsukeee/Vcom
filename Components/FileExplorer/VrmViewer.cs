using VRSNS.Core;
using UnityEngine;
using VRSNS.VRM;

namespace VRUtils.Components
{
public class VrmViewer : MonoBehaviour, IViewer
{
    public async void Display(string filePath, GameObject handlerObject)
    {
        var avatarData = await VRMImporter.ReadAllBytesAsync(filePath);
        var client = handlerObject.transform.root.GetComponent<Client>();
        await Client.Avatar.GenerateAvatar(avatarData);
    }
}
}
