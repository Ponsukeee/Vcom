using System;
using System.IO;
using UniRx.Async;
using UnityEngine;
using VRM;

namespace MagicOnionClient
{
public class VRMImporter
{
    public static async UniTask<GameObject> LoadVRMAsync(byte[] bytes)
    {
        var context = new VRMImporterContext();
        try
        {
            context.ParseGlb(bytes);
            var meta = context.ReadMeta(false);
            await context.LoadAsyncTask();
            context.ShowMeshes();
            return context.Root;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            throw;
        }
    }

    public static async UniTask<GameObject> LoadVRMAsync(string path)
    {
        var bytes = await ReadAllBytesAsync(path);
        return await LoadVRMAsync(bytes);
    }

    public static async UniTask<byte[]> ReadAllBytesAsync(string path)
    {
        byte[] result;
        using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None, bufferSize: 4096, useAsync: true))
        {
            result = new byte[stream.Length];
            await stream.ReadAsync(result, 0, (int) stream.Length);
        }

        return result;
    }
}
}