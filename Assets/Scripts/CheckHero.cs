using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CheckHero : MonoBehaviour
{
    
    public delegate void ChangePosition();
    public static event ChangePosition onChangePosition;

    [SerializeField] private HeroBase heroBase;
    public HeroBase listHero;
    [SerializeField] private List<GameObject> heroUnitsPoint;
    [SerializeField] private List<GameObject> heroUnitsFrame;
    public List<GameObject> heroActive;

    private void Awake()
    {
        if (Time.timeScale != 1.0f)
            Time.timeScale = 1.0f;
    }

    void Start()
    {
        listHero.HeroList.Clear();
        for (int index = 0; index < heroBase.HeroList.Count; index++)
        {
            var hero = Instantiate(heroBase.HeroList[index], transform.position, Quaternion.identity);

            if (heroUnitsPoint != null)
            {
                hero.transform.position = heroUnitsPoint[index].transform.position;
                hero.GetComponent<UnitMoveToPos>().StartPoint = heroUnitsPoint[index].transform;
                hero.GetComponent<UnitMoveToPos>().PointFrame = heroUnitsFrame[index].transform;
                hero.transform.localScale = new Vector3(Random.Range(-1, 1) == 0 ? 1.5f : -1.5f, 1.5f, 1.5f);
            }
        }
    }
  
    private void Update()
    {
        /*if (Input.GetKeyDown((KeyCode) '1'))
        {
            
            onChangePosition();
        }*/
    }
}
