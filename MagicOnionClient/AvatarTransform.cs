using MessagePack;

namespace MagicOnionClient
{
[MessagePackObject]
public struct AvatarTransform
{
    [Key(0)]
    public ObjectTransform Head { get; set; }
    [Key(1)]
    public ObjectTransform RightHand { get; set; }
    [Key(2)]
    public ObjectTransform LeftHand { get; set; }
}
}
