using System.Threading.Tasks;
using MagicOnion;

namespace MagicOnionClient
{
public interface IGamingHub : IStreamingHub<IGamingHub, IGamingHubReceiver>
{
    Task<RoomInfo[]> GetRoomInfos();
    Task<int> JoinRoomAsync(string userName);
    Task<int> JoinOtherRoomAsync(string userName, string roomID);
    Task LeaveRoomAsync();
    Task<PlayerInfo[]> GenerateAvatarAsync(AvatarTransform transform, byte[] avatarData);
    Task<int> InstantiateAsync(string resourceName);
    Task DestroyAsync(int id);
    Task SynchronizeAvatarAsync(AvatarTransform transform);
    Task MoveObjectAsync(int id, ObjectTransform transform);
    Task SpeakAsync(int index, float[] segment);
}
}
