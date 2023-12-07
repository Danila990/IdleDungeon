using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OnFrame : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int index;
    [SerializeField] private TavernManager tavernManager;
    float speed = 20.0f; //how fast it shakes
    float amount = 1.0f; //how much it shakes
    public TextMeshPro level;
    public GameObject arrow;
    public GameObject group;
    
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

    [SerializeField]private bool rotateFrame = false;
    
    
    void Start()
    {
        //tavernManager = GameObject.Find("Game").GetComponent<TavernManager>();
        
        //basePosition = this.transform.localPosition;
        baseRotation = this.transform.localRotation;
        //baseScale = this.transform.localScale;
    }

    private void OnMouseDown()
    {
        tavernManager.OnFrame(index);
    }

    
    
    
    [Range(0, 1)]
    public float power = .045f;

    /*[Header("Position Jiggler")]
    public bool jigPosition = true;
    public Vector3 positionJigAmount;
    */
    [Range(0, 120)]
    public float positionFrequency = 30;
    float positionTime;

    [Header("Rotation Jiggler")]
    public bool jigRotation = true;
    public Vector3 rotationJigAmount;
    [Range(0, 120)]
    public float rotationFrequency = 10;
    float rotationTime;

    /*
    [Header("Scale Jiggler")]
    public bool jigScale = true;
    public Vector3 scaleJigAmount = new Vector3(.1f, -.1f, .1f);
    [Range(0, 120)]
    public float scaleFrequency = 10;
    float scaleTime;*/

    Vector3 basePosition;
    Quaternion baseRotation;
    Vector3 baseScale;

    // Update is called once per frame
    void Update()
    {
        var dt = Time.deltaTime;
        positionTime += dt * positionFrequency;
        rotationTime += dt * rotationFrequency;
        //scaleTime += dt * scaleFrequency;

        //if (jigPosition)
        //    transform.localPosition = basePosition + positionJigAmount * Mathf.Sin(positionTime) * power;
        if(rotateFrame)
            transform.localRotation = baseRotation * Quaternion.Euler(rotationJigAmount * Mathf.Sin(positionTime) * power);
        //if (jigScale)
        //    transform.localScale = baseScale + scaleJigAmount * Mathf.Sin(scaleTime) * power;
    }
    
    // Update is called once per frame
    /*void Update()
    {

        if (shake)
        {
            transform.position = new Vector3(transform.position.x+Mathf.Sin(Time.time * speed) * 0.4f/50, transform.position.y,transform.position.z);
        }

    }*/
}
