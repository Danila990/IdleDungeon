using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnChek : MonoBehaviour
{
    public delegate void ChangePosition();
    public static event ChangePosition Point;
    
    
    public GameObject tavernManager;

    [SerializeField] private int index;

    void Start()
    {
        OnChek.Point += DebugInfo;
    }

    private void OnDisable()
    {
        OnChek.Point -= DebugInfo;
    }

    /*public Shader shader_on;
    public Shader shader_off;
    public GameObject checkHero;
    public GameObject gameO;
    
    public bool check = true;
    // Start is called before the first frame update
    void Start()
    {
        gameO=GameObject.Find("Game");
        //gameO.GetComponent<CheckHero>().listHero.HeroList.Add(checkHero);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        check = !check;
        //Debug.Log(transform.GetComponent<Renderer>().material.shader);
        if (check)
        {
            gameO.GetComponent<CheckHero>().listHero.HeroList.Add(checkHero);
            transform.GetComponent<Renderer>().material.shader = shader_on;
        }
        else
        {
            gameO.GetComponent<CheckHero>().listHero.HeroList.Remove(checkHero);
            transform.GetComponent<Renderer>().material.shader = shader_off;
        }
        
    }*/
    public void OnTestButon()
    {
        Point?.Invoke();
    }

    private void DebugInfo()
    {
        //Debug.Log("ButtonOn");
    }
    
    private void OnMouseDown()
    {
        
        tavernManager.GetComponent<TavernManager>().OnFrameButton(index);
    }
}
