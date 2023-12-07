using UnityEngine;
using UnityEngine.Purchasing;

public class MotherFakaSpeed : MonoBehaviour
{
    public GameObject autoSkill; 
    private bool key=false;
    private float fixedDeltaTime;
    
    private void Awake()
    {
        fixedDeltaTime = Time.fixedDeltaTime;
    }
    
    // Start is called before the first frame update
    public void Turn()
    {
        key = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (key)
        {
            if(!autoSkill.GetComponent<AutoSkill>().SkillAvtoTimerEnable)autoSkill.GetComponent<AutoSkill>().UseButtonOn();
            Time.timeScale = 5.0f;
        }
        
    }
}
