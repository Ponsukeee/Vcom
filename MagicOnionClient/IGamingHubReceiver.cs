namespace MagicOnionClient
{
public interface IGamingHubReceiver
{
    void OnJoinRoom(int id, string playerName, int playerCount);
    void OnLeaveRoom(int id, string userName, int playerCount);
    void OnDestroyRoom(int id);
    void OnGenerateAvatar(int id, AvatarTransform player, byte[] avatarData);
    void OnInstantiate(int id, string resourceName);
    void OnDestroy(int id);
    void OnSynchronizeAvatar(int id, AvatarTransform transform);
    void OnMoveObject(int id, ObjectTransform transform);
    void OnSpeak(int index, float[] segment);
}
}
