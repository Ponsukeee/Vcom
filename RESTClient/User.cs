using System;
using System.IO;
using UniRx.Async;
using UnityEngine;
using UnityEngine.Networking;
using VRUtils.Components;

namespace RestClient
{
[Serializable]
public class User
{
    [SerializeField] private string name;
    public string Name => name;
    [SerializeField] private string token;

    public bool IsSignin => !token.Equals("");

    public static string url;

    public static async UniTask<User> CreateAsync(string serverHostName, string name, string email, string password, string passwordConfirmation)
    {
        url = $@"http://{serverHostName}/";
        var form = new WWWForm();
        form.AddField("name", name);
        form.AddField("email", email);
        form.AddField("password", password);
        form.AddField("password_confirmation", passwordConfirmation);
        var request = UnityWebRequest.Post(url + "users", form);
        await request.SendWebRequest();
        Debug.Log(request.downloadHandler.text);
        try
        {
            return JsonUtility.FromJson<User>(request.downloadHandler.text);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            throw;
        }
    }

    public static async UniTask<User> SigninAsync(string serverHostName, string email, string password)
    {
        url = $@"http://{serverHostName}/";
        var form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        var request = UnityWebRequest.Post(url + "users/signin", form);
        await request.SendWebRequest();
        Debug.Log(request.downloadHandler.text);
        Notification.Notify("サインインに成功しました");
        try
        {
            return JsonUtility.FromJson<User>(request.downloadHandler.text);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async void UploadVrmAsync(string path)
    {
        var bytes = await ReadAllBytesAsync(path);
        UploadVrmAsync(bytes);
    }

    public async UniTask UploadVrmAsync(byte[] bytes)
    {
        var form = new WWWForm();
        form.AddField("name", name + "'s model");
        form.AddBinaryData("data", bytes);
        var request = UnityWebRequest.Post(url + "vrms", form);
        request.SetRequestHeader("Authorization", "Bearer " + token);
        await request.SendWebRequest();
        Debug.Log(request.downloadHandler.text);
    }

    public async UniTask<byte[]> DownloadVrmAsync()
    {
        var request = UnityWebRequest.Get(url + "vrms");
        request.SetRequestHeader("Authorization", "Bearer " + token);
        await request.SendWebRequest();
        if (request.isHttpError || request.isNetworkError)
        {
            Debug.Log("エラー" + request.downloadHandler.text);
            return null;
        }
        
        return request.downloadHandler.data;
    }

    private static async UniTask<byte[]> ReadAllBytesAsync(string path)
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