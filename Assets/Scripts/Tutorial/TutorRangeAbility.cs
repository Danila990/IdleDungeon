using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorRangeAbility : MonoBehaviour
{
    [SerializeField] private GameObject g1;
    [SerializeField] private GameObject g2;
    private GameObject target;
    private bool key = false;
    private float cyrTime;
    private float fixedDeltaTime;
    public bool enemyORhero = true;
    
        // Start is called before the first frame update
    void Start()
    {
        fixedDeltaTime = Time.fixedDeltaTime;
        
        cyrTime=Time.timeScale;
            
        Time.timeScale = 0.25f;
            
        GetComponent<Image>().enabled=true;
        target = enemyORhero?GameObject.FindGameObjectWithTag("Enemy"):GameObject.FindGameObjectWithTag("Hero");
        StartCoroutine(DoMove(1f, target.transform.position));
    }


    public void closeTut()
    {
        gameObject.SetActive(false);
        key = true;
    }
    
    
       
    
    private IEnumerator DoMove(float time, Vector2 targetPosition)
    {
        while (!key)
        {
            target = enemyORhero?GameObject.FindGameObjectWithTag("Enemy"):GameObject.FindGameObjectWithTag("Hero");
            if(target)targetPosition = target.transform.position;
            
            g1.transform.position=Vector3.zero;
            g1.SetActive(true);
            g2.SetActive(true);
            yield return new WaitForSeconds(0.12f);
            g2.transform.localScale = new Vector3(0.8f, 0.8f, 1);


            Vector2 startPosition = g1.transform.position;
            float startTime = Time.realtimeSinceStartup;
            float fraction = 0f;
            while (fraction < 1f)
            {
                fraction = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / time);
                g1.transform.position = Vector2.Lerp(startPosition, targetPosition, fraction);
                yield return null;
            }

            g2.transform.localScale = new Vector3(1, 1, 1);
            yield return new WaitForSeconds(0.5f);
            g2.SetActive(false);
            yield return new WaitForSeconds(0.25f);
            g1.SetActive(false);
        }
    }
}
