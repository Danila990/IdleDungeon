using System;
using System.Collections;
//using System.Diagnostics;
using System.Linq;
using Spine;
using Spine.Unity;
using UnityEngine.UI;
using UnityEngine;

public class UnitControllerEnemy : MonoBehaviour
{

    public GameObject TextGuy;
    [SerializeField] private GameObject bFight;
    public bool startBattle = false;
    [SerializeField] private GameObject unitTarget2;
    #region Свойства

    public float UnitHP
    {
        get => unitHP;
    }

    public float UnitCyrHP
    {
        get => unitCyrHP;
        set
        {
            unitCyrHP = value;

            if (unitCyrHP <= 0f)
            {
                unitCyrHP = 0;
                if (meshRenderer.enabled) SetDie();
            }
            else hpValueBar.SetHP = unitCyrHP;
        }
    }

    public Transform UnitTarget
    {
        get => unitTarget;
        set => unitTarget = value;
    }

    public Transform Teleport
    {
        get => teleport;
        private set => teleport = value;
    }

    public Transform Death
    {
        get => death;
        private set => death = value;
    }

    public MeshRenderer MeshRenderer
    {
        get => meshRenderer;
        set => meshRenderer = value;
    }

    public AnimationController AnimationController
    {
        get => animationController;
        private set => animationController = value;
    }

    public float UnitSpeed
    {
        get => unitSpeed;
        private set => unitSpeed = value;
    }

    public float UnitDamage
    {
        get => unitDamage;
        set => unitDamage = value;
    }

    public float Accuracy
    {
        get => accuracy;
        private set => accuracy = value;
    }

    #endregion


    #region Поля

    
    [SerializeField] public float unitHP;

    [SerializeField] private float unitSpeed;
    [SerializeField] private float unitDamage;
 
    [SerializeField] private float accuracy;

    

    [SerializeField] private Transform unitTarget;
 
    [SerializeField] private MeshRenderer meshRenderer;
 
    [SerializeField] private AnimationController animationController;

 
    [SerializeField] private Transform teleport;
 
    [SerializeField] private Transform death;
    [SerializeField] private Transform skillButton;
 
    [SerializeField] private Transform hpBar;
 
    [SerializeField] private HPSystemTutorial hpValueBar;
     private float unitCyrHP;
    #endregion

    #region Публичные методы

   
    
    
    public void SetDie()
    {
        StartCoroutine(unitActivate());
        transform.tag = "Die";
        
        animationController.SetIdle();
        death.GetComponent<SkeletonAnimation>().state.SetAnimation(0, "death", false);
        
    }

    #endregion

    #region Приватные методы

    
    
    private void Start()
    {
        //unitTarget=GameObject.FindGameObjectWithTag("Hero").transform;
        //var gameManager=GameObject.Find("GameManager");
        LearnBattle.UnitCommand+=gameManager;
        //TavernManager.gotoStartPoint += GotoStartPoint;
        unitTarget.GetComponent<UnitControllerTutorial>().
        GetComponent<SkeletonAnimation>().AnimationState.Event += HandleEvent;
        unitCyrHP = unitHP;
        
    }

    void gameManager(int i)
    {
        //Debug.Log("deleg="+i);
    }
    
    public void ChangeState(int state)
    {
        switch (state)
        {
            case 0: ;
                StartCoroutine(unitTutorialState());        
                break;
            case 1: ;
                break;
            case 2: ;
                break;
            default: break;
        }
        
    }
    private void HandleEvent(TrackEntry trackEntry, Spine.Event e)
    {
        //Debug.Log(e.Data.Name); 
        if (unitTarget != null && Vector2.Distance(transform.position, unitTarget.transform.position) <= accuracy)
        {
            if (e.Data.Name == "shoot")
            {
                //Debug.Log("Удар "+unitDamage);
                unitTarget.GetComponent<UnitControllerTutorial>().UnitCyrHP -= unitDamage;
                if (unitTarget.GetComponent<UnitControllerTutorial>().UnitCyrHP <= 0f)
                {
                    AnimationController.SetIdle();
                    StartCoroutine(unitTutorialStateHide());
                }
                //Debug.Log("Фигачим");
                //Debug.Log(e.Data.Name);
            }
        }
        
         
    }


    private IEnumerator unitTutorialStateHide()
    {
        yield return new WaitForSeconds(1f);
        Teleport.GetComponent<SkeletonAnimation>().state.SetAnimation(0, "teleport", false);
        var enabled = meshRenderer.enabled;
        hpBar.gameObject.SetActive(!enabled);
        meshRenderer.enabled = !enabled;
    }
    
    private IEnumerator unitTutorialState()
    {
        AnimationController.SetIdle();
        yield return new WaitForSeconds(1f);
        
        if (CompareTag("Hero"))
        {
            AnimationController.SetIdle();
            Teleport.GetComponent<SkeletonAnimation>().state.SetAnimation(0, "teleport", false);
            yield return new WaitForSeconds(0.5f);
            var enabled = meshRenderer.enabled;
            hpBar.gameObject.SetActive(!enabled);
            meshRenderer.enabled = !enabled;
            
            yield return new WaitForSeconds(1.5f);
            TextGuy.SetActive(true);
            yield return new WaitForSeconds(3f);
            bFight.SetActive(true);
        }
        else
        {
            
            yield return new WaitForSeconds(3.0f);
            TextGuy.SetActive(true);
            //StartCoroutine(unitWaitForBattle());
        }

        yield return null;
    }
    
    private void Update()
    {

        if (startBattle)
        {
            
            if (unitTarget.GetComponent<UnitControllerTutorial>().UnitCyrHP > 0f)
            {
                //Debug.Log("1");
                //Debug.Log("К цели");
                transform.position = Vector2.MoveTowards(transform.position, UnitTarget.position,
                    3 * Time.deltaTime);
                //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
                if (Vector2.Distance(transform.position, UnitTarget.position) < accuracy)
                {
                    GetComponent<AnimationController>().SetAttack();
                    startBattle = false;
                    //SetAttack();
                }
            }//else SetSearchTarget();
        }
        
        /*unitFSM.CurrentState.Update();
        ChangeDirection();
        
        if (waitTarget)
        {
            unitTarget = null;
            var nearestTarget = tag == "Hero"
                ? GameObject.FindGameObjectsWithTag("Enemy")
                : GameObject.FindGameObjectsWithTag("Hero");
            float dist = 500f;

            if (nearestTarget != null)
            {
                //var nearestTarget = tag == "Hero"
                //    ? GameObject.FindGameObjectsWithTag("Enemy") 
                //    : GameObject.FindGameObjectsWithTag("Hero");
                bool targetUnitStart = false;
                /*foreach (var n in nearestTarget.Where(x => x.GetComponent<UnitController>().startPoint == startPoint))
                {
                    targetUnitStart = true;
                    if (n.GetComponent<UnitController>().readyToBattle)
                    {
                        var cyrDist = Vector2.Distance(transform.position, n.transform.position);
                        if (cyrDist < dist)
                        {
                            dist = cyrDist;
                            unitTarget = n.transform;
                        }
                    }
                }*/

               /* if (!targetUnitStart)
                {
                    SetSearchTarget();
                }

                if (unitTarget != null)
                {
                    waitTarget = false;
                    SetMove();
                }
            }
        }*/
    }


    private IEnumerator unitActivate()
    {
        var enabled = meshRenderer.enabled;
        //if (enabled) readyToBattle = false;
        if (!enabled)
        {
            yield return new WaitForSeconds(0.5f);
        }
        
        hpBar.gameObject.SetActive(!enabled);
        if (CompareTag("Hero"))
        {
            skillButton.gameObject.SetActive(!enabled);
            //Сброс прицела
            foreach (var cyr in GameObject.FindObjectsOfType<ButtonSkill>())
            {
                cyr.restartSkill();
            }
        }
        meshRenderer.enabled = !enabled;
        yield return new WaitForSeconds(0.5f);
        if (!enabled)
        {
            //startGame = true;
            //SetMove();
        }

        yield return null;
    }
    #endregion
}
