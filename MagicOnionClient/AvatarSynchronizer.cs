using UniRx;
using UnityEngine;

namespace MagicOnionClient
{
public class AvatarSynchronizer : MonoBehaviour
{
    public AvatarTransform　TargetTransform { get; set; }
    public bool isMine = false;
    private int count;
    public float SmoothingDelay = 10f;
    public Subject<AvatarTransform> OnMove { get; } = new Subject<AvatarTransform>();

    private Transform head;
    private Transform rightHand;
    private Transform leftHand;
    
    private void Update()
    {
        if (isMine)
        {
//            if (!GamingHubReceiver.IsSynchronizing) return;
            
            if (count >= 5)
            {
                var avatarTransform = GameClient.CreateAvatarTransform(head, rightHand, leftHand);
                OnMove.OnNext(avatarTransform);
                count = 0;
            }
            count++;
        }
        else
        {
            head.position = Vector3.Lerp(head.position, TargetTransform.Head.Position, Time.deltaTime * SmoothingDelay);
            head.rotation = Quaternion.Lerp(head.rotation, TargetTransform.Head.Rotation, Time.deltaTime * SmoothingDelay);
        
            rightHand.position = Vector3.Lerp(rightHand.position, TargetTransform.RightHand.Position, Time.deltaTime * SmoothingDelay);
            rightHand.rotation = Quaternion.Lerp(rightHand.rotation, TargetTransform.RightHand.Rotation, Time.deltaTime * SmoothingDelay);
        
            leftHand.position = Vector3.Lerp(leftHand.position, TargetTransform.LeftHand.Position, Time.deltaTime * SmoothingDelay);
            leftHand.rotation = Quaternion.Lerp(leftHand.rotation, TargetTransform.LeftHand.Rotation, Time.deltaTime * SmoothingDelay);
        }
    }

    public void SetTargets(GameObject head, GameObject rightHand, GameObject leftHand)
    {
        this.head = head.transform;
        this.rightHand = rightHand.transform;
        this.leftHand = leftHand.transform;
    }
}
}
