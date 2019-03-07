using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRSNS.Core;

public class SignInPanelBehaviour : MonoBehaviour
{
    private void Update()
    {
        if (Client.InRoom)
        {
            Destroy(transform.root.gameObject);
        }
    }
}
