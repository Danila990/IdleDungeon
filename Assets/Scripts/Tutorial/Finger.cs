using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finger : MonoBehaviour
{
    private bool UIfinger = false;
    private float t=0.5f;
    private float cyrT=0;
    [SerializeField]private float size;
    [SerializeField] private float size1;
    
    // Start is called before the first frame update
    void Start()
    {
        cyrT = t;
    }

    // Update is called once per frame
    void Update()
    {
        cyrT -= Time.deltaTime;
        if (cyrT <= t/2)
        {
            transform.localScale = new Vector3(size1, size1, 1f);
        }
        if (cyrT <= 0)
        {
            cyrT = t;
            transform.localScale = new Vector3(size, size, 1f);
        }
    }
}
