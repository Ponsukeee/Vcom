using System;
using System.Threading.Tasks;
using MagicOnionClient;
using UniRx;
using UnityEngine;
using Valve.VR.InteractionSystem;
using RestClient;
using VRSNS.VRM;
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

    private static string restHN;
    private static string moHN;

    private static User User { get; set; }
    public static VRMAvatar Avatar { get; private set; }
    private static GameClient gameClient;
    public static bool InRoom { get; private set; }

    private void Awake()
    {
        Avatar = new VRMAvatar(head, rightHand, leftHand);
        gameClient = new GameClient();

        restHN = restServerHostName;
        moHN = moServerHostName;
    }

    public static async void CreateUser(string name, string email, string password, string passwordConfirmation)
    {
        try
        {
            User = await User.CreateAsync(restHN, name, email, password, passwordConfirmation);
            await SetUpAvatar();
            await JoinRoom();
            Notification.Notify($"{User.Name}様のアカウントを作成しました");
        }
        catch (Exception e)
        {
            Notification.Notify("サインアップに失敗しました");
            Debug.LogError(e);
            throw;
        }
    }

    public static async Task Signin(string email, string password)
    {
        try
        {
            User = await User.SigninAsync(restHN, email, password);
            await SetUpAvatar();
            await JoinRoom();
            Notification.Notify($"ようこそ{User.Name}様");
        }
        catch (Exception e)
        {
            Notification.Notify("サインインに失敗しました");
            Debug.LogError(e);
            throw;
        }
    }

    private static async Task SetUpAvatar()
    {
        var avatarData = await User.DownloadVrmAsync();
        await Avatar.GenerateAvatar(avatarData);
        Player.instance.rightHand.GetComponentInChildren<RenderModel>()?.gameObject.SetActive(false);
        Player.instance.leftHand.GetComponentInChildren<RenderModel>()?.gameObject.SetActive(false);

        var synchronizer = Avatar.Root.AddComponent<AvatarSynchronizer>();
        synchronizer.SetTargets(Avatar.Head, Avatar.RightHand, Avatar.LeftHand);
        synchronizer.OnMove.Subscribe(avatarTransform => gameClient.SynchronizeAvatar(avatarTransform));
        synchronizer.isMine = true;

        var voiceChat = Avatar.Root.AddComponent<VoiceChat>();
        voiceChat.StartVoiceChat();
        voiceChat.OnSampleReady += gameClient.Speak;
    }

    public static async Task JoinRoom(string roomID = "")
    {
        try
        {
            if (roomID.Equals(""))
            {
                await gameClient.JoinRoom(moHN, User.Name);
            }
            else
            {
                await gameClient.JoinRoom(moHN, User.Name, roomID);
            }
            
            await gameClient.GenerateAvatarForOthers(Avatar);
            InRoom = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static async Task<RoomInfo[]> GetRooms()
    {
        return await gameClient.GetRooms();
    }

    public static async void UploadVrm()
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

    public static async void LeaveRoom()
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
        {
            await gameClient.DisposeAsync();
            InRoom = false;
        }
    }
}
}