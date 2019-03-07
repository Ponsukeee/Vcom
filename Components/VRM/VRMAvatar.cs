using RootMotion.FinalIK;
using UniRx.Async;
using UnityEngine;

namespace VRSNS.VRM
{
public class VRMAvatar
{
    public GameObject Root { get; private set; }
    public GameObject Head { get; private set; }
    public GameObject RightHand { get; private set; }
    public GameObject LeftHand { get; private set; }
    public byte[] AvatarData { get; private set; }

    public VRMAvatar(GameObject head, GameObject rightHand, GameObject leftHand)
    {
        Head = head;
        RightHand = rightHand;
        LeftHand = leftHand;
    }

    public async UniTask GenerateAvatar(byte[] newAvatarData)
    {
        if (AvatarData != null)
        {
            GameObject.DestroyImmediate(Root);
        }
        
        Root = await VRMImporter.LoadVRMAsync(newAvatarData);
        Root.transform.localScale = 1.15f * Vector3.one;
        AvatarData = newAvatarData;

        InitializeAvatar();
    }

    private void InitializeAvatar()
    {
        Root.transform.position = new Vector3(Head.transform.position.x, 0f, Head.transform.position.z);

        var vrIK = Root.AddComponent<VRIK>();
        vrIK.AutoDetectReferences();

        vrIK.solver.rightArm.stretchCurve = new AnimationCurve();
        vrIK.solver.leftArm.stretchCurve = new AnimationCurve();
        vrIK.solver.spine.headTarget = Head.transform;
        vrIK.solver.rightArm.target = RightHand.transform;
        vrIK.solver.leftArm.target = LeftHand.transform;

        vrIK.solver.rightLeg.swivelOffset = -15;
        vrIK.solver.leftLeg.swivelOffset = 15;
        vrIK.solver.locomotion.footDistance = 0.15f;
        vrIK.solver.locomotion.stepThreshold = 0.4f;
        vrIK.solver.locomotion.maxVelocity = 0.3f;
        vrIK.solver.locomotion.velocityFactor = 0.3f;
        vrIK.solver.locomotion.rootSpeed = 30;
    }
}
}