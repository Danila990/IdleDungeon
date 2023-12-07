using System.Collections;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;


public class ButtonSkillTutorial : MonoBehaviour
{

    public GameObject button;
    public GameObject targetGO;
    [SerializeField] private GameObject blackScreen;
    public bool tut = false;
    public float timeToPause = 10f;
    
    
    public void ButtonPress() => buttonPress();
    public Image SkillImage;
    [SerializeField] private float time;
    [SerializeField] private UnitControllerTutorial uController;
    float positionTime = 0;
    
    public UnitControllerTutorial UController
    {
        get => uController;
        set => uController = value;
    }

    private bool active = false;
    private Vector3 mTarget;
    private void OnEnable()
    {
        if (!tut)
        {
            //StartCoroutine(ShowHideButton());
            SkillImage.fillAmount = 0;
        }
        active = true;
        button.GetComponent<Button>().enabled = false;
    }

    IEnumerator ShowHideButton()
    {
        button.GetComponent<Button>().enabled = false;
        SkillImage.fillAmount = 0;
        
        do
        {
            SkillImage.fillAmount += Time.deltaTime;
            yield return new WaitForSeconds(time);
        } while (SkillImage.fillAmount < 1f);
        
        
        button.GetComponent<Button>().enabled = true;
        
 
    }

    private Touch touch;
    private bool activeTouch=false;
    
    private void Update()
    {
        //Update the Text on the screen depending on current TouchPhase, and the current direction vector
        //m_Text.text = "Touch : " + message + "in direction" + direction;

        // Track a single touch as a direction control.
        if (Input.touchCount > 0)
        {

            
            touch = Input.GetTouch(0);

            // Handle finger movements based on TouchPhase
            switch (touch.phase)
            {
                //When a touch has first been detected, change the message and record the starting position
                case TouchPhase.Began:
                    // Record initial touch position.
                    //startPos = touch.position;
                    //message = "Begun ";
                    break;

                //Determine if the touch is a moving touch
                case TouchPhase.Moved:
                    // Determine direction by comparing the current touch position with the initial one
                    //direction = touch.position - startPos;
                    //message = "Moving ";
                    break;

                case TouchPhase.Ended:
                    // Report that the touch has ended when it ends
                    //message = "Ending ";
                    break;
            }
        }
    
    
        
        if (active)
        {
        
            SkillImage.fillAmount += time*Time.deltaTime;
            
            
            if (SkillImage.fillAmount >= 1f)
            {
                active = false;
                button.GetComponent<Button>().enabled = true;


                if (!tut)
                {
                    blackScreen.SetActive(true);
                    foreach (var cur in GameObject.FindObjectsOfType<UnitControllerTutorial>())
                    {
                        cur.transform.GetComponent<SkeletonAnimation>().timeScale = 0.1f;
                    }
                }
                
                
            }
        }

        if (!active && button.GetComponent<Button>().enabled && !tut)
        {
            
            
            
            var dt = Time.deltaTime;
            positionTime += dt * 15;
            transform.localScale = new Vector3(1 + Mathf.Sin(positionTime)*0.02f, 1 + Mathf.Sin(positionTime)*0.02f, 1f);

            // Уменьшаем время до постановки на паузу
            if (timeToPause <= 0)
            {
                foreach (var cur in GameObject.FindObjectsOfType<UnitControllerTutorial>())
                {
                    cur.transform.GetComponent<SkeletonAnimation>().timeScale = 0f;
                }
            }else timeToPause -= Time.deltaTime;
                
        }
  
    }

    private GameObject[] enemyTarget;
    
    private void buttonPress()
    {

        blackScreen.SetActive(false);


        foreach (var cur in GameObject.FindObjectsOfType<UnitControllerTutorial>())
        {
            cur.transform.GetComponent<SkeletonAnimation>().timeScale = 1f;
        }
        
        
            bool key = false;
            enemyTarget  = GameObject.FindGameObjectsWithTag("Enemy");

                    active = true;
                    uController.ActivateSkill();
                    key = true;

                button.GetComponent<Button>().enabled = false;
                SkillImage.fillAmount = 0;


        
    }

    private bool TargetBool()
    {
        
        Vector3 position = uController.transform.position;
        Vector3 rightDir = uController.transform.right;
        
        foreach (var n in enemyTarget)
        {
                
            
            Vector3 target = n.transform.position;
            var dir = (target - position).normalized;
            var dot = Vector3.Dot(dir, rightDir);

            if (uController.transform.localScale.x >= 0 && dot >= 0 ||
                uController.transform.localScale.x <= 0 && dot <= 0)
            {
                if (Vector2.Distance(uController.transform.position, n.transform.position) < 1f)
                {
                    return true;
                }
            }
        }

        return false;
    }
    
    
    
}


