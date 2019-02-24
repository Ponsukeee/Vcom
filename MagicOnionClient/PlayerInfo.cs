using MessagePack;

namespace MagicOnionClient
{
[MessagePackObject]
public class PlayerInfo
{
    [Key(0)]
    public int ID;
    [Key(1)]
    public string Name;
    [Key(2)]
    public AvatarTransform Transform;
    [Key(3)]
    public byte[] AvatarData;
}
}
