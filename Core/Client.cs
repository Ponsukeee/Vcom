using System;
using System.Threading.Tasks;
using MagicOnionClient;
using UniRx;
using UniRx.Async;
using UnityEngine;
using WebClient;
using Notification = VRUtils.Components.Notification;

namespace VRSNS.Core
{
public class Client : MonoBehaviour
{
    [SerializeField] private string restServerHostName;
    [SerializeField] private string moServerHostName;
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject leftHand;

    private User User { get; set; }
    public VRMAvatar Avatar { get; private set; }
    private GameClient gameClient;

    private void Awake()
    {
        Avatar = new VRMAvatar(head, rightHand, leftHand);
        gameClient = new GameClient();
    }

    public async void CreateUser(string name, string email, string password, string passwordConfirmation)
    {
        try
        {
            User = await User.CreateAsync(restServerHostName, name, email, password, passwordConfirmation);
            await SetUpAvatar();
            await ConnectGameServer();
            Notification.Notify($"{User.Name}様のアカウントを作成しました");
        }
        catch (Exception e)
        {
            Notification.Notify("サインアップに失敗しました");
            Debug.LogError(e);
            throw;
        }
    }

    public async void Signin(string email, string password)
    {
        try
        {
            User = await User.SigninAsync(restServerHostName, email, password);
            await SetUpAvatar();
            await ConnectGameServer();
            Notification.Notify($"ようこそ{User.Name}様");
        }
        catch (Exception e)
        {
            Notification.Notify("サインインに失敗しました");
            Debug.LogError(e);
            throw;
        }
    }

    public async void JoinRoom(string roomID)
    {
        await gameClient.JoinRoom(moServerHostName, User.Name, roomID);
        await gameClient.GenerateAvatarForOthers(Avatar);
    }

    private async Task SetUpAvatar()
    {
        var avatarData = await User.DownloadVrmAsync();
        await Avatar.GenerateAvatar(avatarData);
        foreach (var model in GetComponentsInChildren<Valve.VR.InteractionSystem.RenderModel>())
        {
            model.gameObject.SetActive(false);
        }
        var synchronizer = Avatar.Root.AddComponent<AvatarSynchronizer>();
        synchronizer.SetTargets(head, rightHand, leftHand);
        synchronizer.OnMove.Subscribe(avatarTransform => gameClient.SynchronizeAvatar(avatarTransform));
        synchronizer.isMine = true;
    }

    private async UniTask ConnectGameServer()
    {
        await gameClient.JoinRoom(moServerHostName, User.Name);
        await gameClient.GenerateAvatarForOthers(Avatar);
    }

    public async UniTask<RoomInfo[]> GetRooms()
    {
        return await gameClient.GetRooms();
    }

    public async void UploadVrm()
    {
        if (User == null) return;
        if (Avatar.AvatarData != null)
        {
            try
            {
                await User.UploadVrmAsync(Avatar.AvatarData);
                Notification.Notify("アバターをアップロードしました");
            }
            catch (Exception e)
            {
                Notification.Notify("アップロードに失敗しました");
                Debug.LogError(e);
                throw;
            }
        }
    }

    public async void LeaveRoom()
    {
        try
        {
            await gameClient.LeaveAsync();
            Notification.Notify("ルームを退室しました");
        }
        catch (Exception e)
        {
            Notification.Notify("ルーム退室に失敗しました");
            Debug.LogError(e);
            throw;
        }
    }

    private async void OnApplicationQuit()
    {
        if (User != null)
            await gameClient.DisposeAsync();
    }
}
}