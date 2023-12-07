using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UnitMoveToPos : MonoBehaviour
{
    #region Приватные поля

    // Для хранения кеша
    private Transform unit;
    public Transform StartPoint
    {
        get
        {
            return startPoint;
        }
        set
        {
            startPoint = value;
        }
    }
    public Transform PointFrame
    {
        get
        {
            return pointFrame;
        }
        set
        {
            pointFrame = value;
        }
    }
    public GameObject FrameGO
    {
        get
        {
            return frameGO;
        }
        set
        {
            frameGO = value;
        }
    }

    [SerializeField] private int indexBase;

    public int IndexBase
    {
        get => indexBase;
        set => indexBase = value;
    }

    public int index;
    public GameObject tavernGO;
    public Transform startPoint;
    public Transform pointFrame;
    [SerializeField] private GameObject frameGO;
    [SerializeField] private Transform cyrPoint;
    private bool goToPoint = false;
    
    //private GameObject startPointGO;
    //private GameObject unitTargetGO;
    private float speed;

    #endregion

    #region Публичные методы

    void Start()
    {
        unit = gameObject.transform;
        speed = 10f;
        //startPoint = GameObject.Find("Exit").transform;
        //TavernManager.onChangePosition+=Change;
        TavernManager.gotoStartPoint += GotoStartPoint;
        TavernManager.gotoFramePoint += GotoFramePoint;
    }

    void GotoStartPoint()
    {
        //FrameGO.SetActive(false);
        
        //GetComponent<UnitController>().enabled = false;
        //GetComponent<UnitController>().SetIdle();
        //GetComponent<UnitController>().SetIdle();
        cyrPoint = startPoint;
        ChangeDirection();
        //Debug.Log("1");
        GetComponent<AnimationController>().SetMove();
        goToPoint = true;

    }

    void GotoFramePoint()
    {
        //FrameGO.SetActive(true);
        
        cyrPoint = pointFrame;
        ChangeDirection();
        GetComponent<AnimationController>().SetMove();
        goToPoint = true;
    }

    private void ChangeDirection()
    {
        transform.localScale = (transform.position - cyrPoint.position).x > 0
            //? new Vector3(-1f, 1f, 1)
        // new Vector3(1f, 1f, 1);
            ? new Vector3(-1.5f, 1.5f, 1)
            : new Vector3(1.5f, 1.5f, 1);
    }
    
    void Update()
    {
        
        if (goToPoint)
        {

            unit.position = Vector2.MoveTowards(unit.position,cyrPoint.position, speed * Time.deltaTime);
            unit.position = new Vector3(unit.position.x, unit.position.y, unit.position.y);
            if (Vector2.Distance(unit.position,cyrPoint.position) <0.1f)
            {
                
                GetComponent<AnimationController>().SetIdle();
                goToPoint=false;
            }
        }

    }

    private void OnDisable()
    {
        TavernManager.gotoStartPoint -= GotoStartPoint;
        TavernManager.gotoFramePoint -= GotoFramePoint;
    }

    #endregion
}
