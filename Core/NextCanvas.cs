using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextCanvas : MonoBehaviour
{
    [SerializeField] private GameObject nextCanvas;
    private GameObject instantiated;
    
    public void ShowNext()
    {
        if (instantiated != null)
        {
            Destroy(instantiated);
        }
        
        instantiated = Instantiate(nextCanvas);
        instantiated.transform.position = transform.position + Vector3.forward * -0.2f;
    }

    public void HideShowing()
    {
        Destroy(instantiated);
    }
}
