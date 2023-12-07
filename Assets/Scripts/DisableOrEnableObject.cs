using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOrEnableObject : MonoBehaviour
{
    [SerializeField] GameObject enableObject;
    [SerializeField] GameObject disableObject;
    
    public void DisableThis()
    {
        disableObject.SetActive(false);
    }
    
    public void EnableThis()
    {
        enableObject.SetActive(true);
    }

}
