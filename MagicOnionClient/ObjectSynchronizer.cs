using UnityEngine;

namespace MagicOnionClient
{
public class ObjectSynchronizer : MonoBehaviour
{
    public ObjectTransform　TargetTransform { get; set; }
    private Vector3 velocity;

    private Transform tf;
    public GameObject GameObject { get; private set; }
    private static readonly float SmoothingDelay = 5;
    
    private void Update()
    {
        Synchronize();
    }

    private void Synchronize()
    {
        tf.position = Vector3.Lerp(tf.position, TargetTransform.Position, Time.deltaTime * SmoothingDelay);
        tf.rotation = Quaternion.Lerp(tf.rotation, TargetTransform.Rotation, Time.deltaTime * SmoothingDelay);
    }

    public void SetTargets(GameObject obj)
    {
        GameObject = obj;
        tf = obj.transform;
    }
}
}
