using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFrameList : MonoBehaviour
{

  //  float speed = 20.0f; //how fast it shakes
  //  float amount = 1.0f; //how much it shakes
    public bool RotateFrame
    {
        get
        {
            return rotateFrame;
        }
        set
        {
            if (value==false) transform.localRotation = Quaternion.Euler(Vector3.zero);
            rotateFrame = value;
        }
    }

    private bool rotateFrame = false;
    
    
    void Start()
    {
        //Debug.Log(transform.localRotation.z);
        baseRotation = transform.rotation; //this.transform.localRotation;
    }

    
    
    [Range(0, 1)]
    public float power = 450f;

    [Range(0, 120)]
    public float positionFrequency = 30;
    float positionTime;

    [Header("Rotation Jiggler")]
    //public bool jigRotation = true;
    public Vector3 rotationJigAmount;
    [Range(0, 120)]
    //public float rotationFrequency = 10;


//    Vector3 basePosition;
    Quaternion baseRotation;
    //Vector3 baseScale;

    // Update is called once per frame
    void Update()
    {
        var dt = Time.deltaTime;
        positionTime += dt * positionFrequency;

        if(rotateFrame)
            transform.GetComponent<RectTransform>().eulerAngles=new Vector3(0f,0f,(Mathf.Sin(positionTime) * 2.5f));
            //transform.localRotation = baseRotation * Quaternion.Euler(rotationJigAmount * Mathf.Sin(positionTime) * power);
    }
  
}
