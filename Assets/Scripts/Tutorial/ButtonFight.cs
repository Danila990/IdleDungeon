using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFight : MonoBehaviour
{
    float positionTime = 0;

    [SerializeField] private float f;
    [SerializeField] private float t;

    [SerializeField] private float de;
    
    [SerializeField] private GameObject text1;
    [SerializeField] private GameObject text2;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject hero;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnButtonFight()
    {
    
        
       // Debug.Log("Start");
        hero.GetComponent<UnitControllerTutorial>().startBattle = true;
        enemy.GetComponent<UnitControllerTutorial>().startBattle = true;
        
        hero.GetComponent<AnimationController>().SetMove();
        enemy.GetComponent<AnimationController>().SetMove();
        
        text1.SetActive(false);
        text2.SetActive(false);
        
        gameObject.SetActive(false);
    }

// Update is called once per frame
    void Update()
    {
        var dt = Time.deltaTime;
        positionTime += dt * de;

        transform.localScale = new Vector3(1 + Mathf.Sin(positionTime)*f, 1 + Mathf.Sin(positionTime)*f, 1f);
        //GetComponent<Image>().color = new Color((Mathf.Sin(positionTime) + t) * f, (Mathf.Sin(positionTime) + t) * f,
        //    (Mathf.Sin(positionTime) + t) * f, 1.0f);
        
//[Color.black]transform.localScale = new Vector3((+t)*f, 1, 1);
    }
}
