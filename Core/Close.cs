using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Close : MonoBehaviour
{
    public void CloseRoot()
    {
        Destroy(transform.root.gameObject);
    }
}
