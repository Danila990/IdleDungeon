using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeachMainCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        //GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
    }

}
