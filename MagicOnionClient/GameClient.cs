using System;
using System.Threading.Tasks;
using Grpc.Core;
using UnityEngine;
using VRSNS.VRM;
using VRUtils.Components;

namespace MagicOnionClient
{
public class GameClient
{
    private GamingHubReceiver receiver;
    private IGamingHub hub;
    private Channel channel;
    private int selfID;

    public GameClient()
    {
        receiver = new GamingHubReceiver();
    }

    public async Task JoinRoom(string hostName, string userName)
    {
        try
        {
            channel = new Channel($"{hostName}:12345", ChannelCredentials.Insecure, new[]
            {
                new ChannelOption(ChannelOptions.MaxReceiveMessageLength, int.MaxValue),
                new ChannelOption(ChannelOptions.MaxSendMessageLength, int.MaxValue),
            });
            hub = await receiver.ConnectAsync(channel);
            selfID = await hub.JoinRoomAsync(userName);
            Notification.Notify("ルームに入室しました");
        }
        catch (Exception e)
        {
            Notification.Notify("ルーム入室に失敗しました");
            Debug.LogError(e);
            throw;
        }
    }

    public async Task JoinRoom(string hostName, string userName, string roomID)
    {
        try
        {
            selfID = await hub.JoinOtherRoomAsync(userName, roomID);
            Notification.Notify("ルームに入室しました");
        }
        catch (Exception e)
        {
            Notification.Notify("ルーム入室に失敗しました");
            Debug.LogError(e);
            throw;
        }
    }

    public async Task GenerateAvatarForOthers(VRMAvatar avatar)
    {
        try
        {
            var avatarTransform = CreateAvatarTransform(avatar.Head.transform, avatar.RightHand.transform, avatar.LeftHand.transform);
            var roomPlayers = await hub.GenerateAvatarAsync(avatarTransform, avatar.AvatarData);
            foreach (var playerInfo in roomPlayers)
            {
                if (selfID != playerInfo.ID)
                    receiver.OnGenerateAvatar(playerInfo.ID, avatarTransform, playerInfo.AvatarData);
            }
        }
        catch (Exception e)
        {
            Notification.Notify("アバター生成に失敗しました");
            Debug.LogError(e);
            await LeaveAsync();
            throw;
        }
    }

    public async Task LeaveAsync()
    {
        if (hub != null)
        {
            await hub.LeaveRoomAsync();
            receiver.OnDestroyRoom(selfID);
        }
    }

    public void SynchronizeAvatar(AvatarTransform avatarTransform)
    {
        hub.SynchronizeAvatarAsync(avatarTransform);
    }

    public async Task DisposeAsync()
    {
        if (hub != null)
        {
            await hub.DisposeAsync();
            await channel.ShutdownAsync();
        }
    }

    public async Task<RoomInfo[]> GetRooms()
    {
        RoomInfo[] result = null;
        if (hub != null)
        {
            result = await hub.GetRoomInfos();
        }

        return result;
    }

    public void Speak(int index, float[] segment)
    {
        hub.SpeakAsync(index, segment);
    }

    public Task WaitForDisconnect()
    {
        return hub?.WaitForDisconnect();
    }
    
    public static AvatarTransform CreateAvatarTransform(Transform headTransform, Transform rightHandTransform, Transform leftHandTransform)
    {
        var head = new ObjectTransform() {Position = headTransform.position, Rotation = headTransform.rotation};
        var rightHand = new ObjectTransform() {Position = rightHandTransform.position, Rotation = rightHandTransform.rotation};
        var leftHand = new ObjectTransform() {Position = leftHandTransform.position, Rotation = leftHandTransform.rotation};
        var avatarTransform = new AvatarTransform() {Head = head, RightHand = rightHand, LeftHand = leftHand};

        return avatarTransform;
    }
}
}
