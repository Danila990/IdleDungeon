using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TestManager : MonoBehaviour
{
    //public UnitController unitController;

    public GameObject text;
    public GameObject go1;
    public GameObject go2;
    Resolution[] resolutions;


    private void Start()
    {
        resolutions = Screen.resolutions;
        
        //foreach (Resolution res in resolutions)
        //{
         //   print(res.width + "x" + res.height);
        //}

        //testC.SetTeleport1();
        //StartCoroutine(StartBattle());
    }/*
    private IEnumerator StartBattle()
    {
        yield return new WaitForSeconds(2f);
        
        var nearestTarget = GameObject.FindGameObjectsWithTag("Enemy");
            
        foreach (var n in nearestTarget)
        {
            //n.GetComponent<UnitController>().ReadyToBattle = true;
            //n.GetComponent<UnitController>().startGame = true;
            n.GetComponent<UnitController>().SetTeleport();
            //n.GetComponent<UnitController>().SetSearchTarget();
        }
            
        nearestTarget = GameObject.FindGameObjectsWithTag("Hero");
        foreach (var n in nearestTarget)
        {
            //n.GetComponent<UnitController>().ReadyToBattle = true;
            //n.GetComponent<UnitController>().startGame = true;
            n.GetComponent<UnitController>().SetTeleport();
            //n.GetComponent<UnitController>().SetSearchTarget();
        }
        
        yield return 0;
    } */
    void Update()
    {
        //go1.transform.position = new Vector3((1920f)/100f, go1.transform.position.y, go1.transform.position.z);
        
        //go1.GetComponent<RectTransform>().offsetMin=new Vector2(go2.GetComponent<RectTransform>().sizeDelta.x-100f,0f);
        //go1.GetComponent<RectTransform>().offsetMax=new Vector2(go2.GetComponent<RectTransform>().sizeDelta.x-100f,0f);
        
        //text.GetComponent<Text>().text = go1.transform.position.ToString()+"("+go1.GetComponent<RectTransform>().offsetMin.x.ToString()+")"+"("+go2.GetComponent<RectTransform>().offsetMin.x.ToString()+")"+resolutions[resolutions.Length-1].width.ToString();
        //text.GetComponent<Text>().text = Screen.width.ToString()+"("+go1.GetComponent<RectTransform>().offsetMin.x.ToString()+")"+"("+go2.GetComponent<RectTransform>().sizeDelta.x.ToString()+")";
        
        /*if (Input.GetKeyDown((KeyCode) '1'))
         { 
             //Debug.Log("Key");
             //unitController.SetTeleport();
 
             
             var nearestTarget = GameObject.FindGameObjectsWithTag("Enemy");
             
             foreach (var n in nearestTarget)
             {
                 //n.GetComponent<UnitController>().ReadyToBattle = true;
                 //n.GetComponent<UnitController>().startGame = true;
                 n.GetComponent<UnitController>().SetTeleport();
                 //n.GetComponent<UnitController>().SetSearchTarget();
             }
             
             nearestTarget = GameObject.FindGameObjectsWithTag("Hero");
             foreach (var n in nearestTarget)
             {
                 //n.GetComponent<UnitController>().ReadyToBattle = true;
                 //n.GetComponent<UnitController>().startGame = true;
                 n.GetComponent<UnitController>().SetTeleport();
                 //n.GetComponent<UnitController>().SetSearchTarget();
             }
 
         }
         */
        //if (Input.GetKeyDown((KeyCode) '2'))
        //{
        //}
        /*
            var nearestTarget = GameObject.FindGameObjectsWithTag("Enemy");
            
            foreach (var n in nearestTarget)
            {
                n.GetComponent<UnitController>().ReadyToBattle = false;
                n.GetComponent<UnitController>().SetMove();
            }
            
            nearestTarget = GameObject.FindGameObjectsWithTag("Hero");
            foreach (var n in nearestTarget)
            {
                n.GetComponent<UnitController>().ReadyToBattle = false;
                n.GetComponent<UnitController>().SetTeleport();
            }
            //Debug.Log("Move");
        }
         
         if (Input.GetKeyDown((KeyCode) '3'))
         {
             unitController.ActivateSkill();
         }
        /*
         if (Input.GetKeyDown((KeyCode) '4'))
         {
             TestC.SetAttack();
            
         }
         if (Input.GetKeyDown((KeyCode) '5'))
         {
             TestC.SetSkill();
             
         }*/
    }

    public void ButtonLeft()
    {
      //  go1.GetComponent<RectTransform>().offsetMin=new Vector2(go1.GetComponent<RectTransform>().offsetMin.x+10f,0f);
      //  go1.GetComponent<RectTransform>().offsetMax=new Vector2(go1.GetComponent<RectTransform>().offsetMax.x-10f,0f);
        go1.transform.position = new Vector3(go1.transform.position.x - 0.1f, go1.transform.position.y,
            go1.transform.position.z);

    }
    
    public void ButtonRight()
    {
        go1.transform.position = new Vector3(go1.transform.position.x + 0.1f, go1.transform.position.y,
            go1.transform.position.z);

    }
    
  /* void OnGUI()
    {
        for(int i=0;i<resolutions.Length;i++)
        {
            GUI.Label(new Rect(100,10+i*25,100,20),resolutions[i].width.ToString()+"x"+resolutions[i].height.ToString());
        }
    }*/

}
