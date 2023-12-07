using System;
using System.Collections;
using System.Linq;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnitController;

public class UnitController : MonoBehaviour
{
    #region Свойства

    public GameObject freezeEffect;
    public AudioClip deathSound;
    public AudioClip attackSound;
    public AudioClip skillSound;
    public TextMeshPro tempText;
    public Shader shader_on;
    public Shader shader_off;
    



    public float UnitHP
    {
        get => unitHP;
    }

    public float UnitCyrHP
    {
        get => unitCyrHP;
        set
        {
            if (blockDamage)
            {
                if(unitHP<value) unitCyrHP = value;
            }else unitCyrHP = value;

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

    public bool ReadyToBattle
    {
        get => readyToBattle;
        set => readyToBattle = value;
    }

    public AnimationController AnimationController
    {
        get => animationController;
        private set => animationController = value;
    }

    public Transform StartPoint
    {
        get => startPoint;
        set => startPoint = value;
    }

    public float UnitSpeed
    {
        get => unitSpeed;
        set => unitSpeed = value;
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
    public float AccuracyBOSS
    {
        get => accuracyBOSS;
        private set => accuracyBOSS = value;
    }
    
    public Skill UnitSkill => skill;

    

   IEnumerator WaitTime(float time)
    {
        yield return new WaitForSeconds(time);
        SetTeleport();
        //unitFSM.ChangeState(new UnitTeleport(this));
        //StartCoroutine(unitActivate());
        yield return null;
    }

    #endregion

    Coroutine inst = null;
    Coroutine inst1 = null;
    
    //Убрать!!!
    public GameObject magic;

    
    
    
    #region Поля

    
    

    [SerializeField] private int heroDBIndex;

    public int HeroDBIndex
    {
        get => heroDBIndex;
        set => heroDBIndex = value;
    }


    public enum Skill
    {
        SimpleMale,
        Shaman,
        Asterix,
        FishMan,
        SimpleFemale,
        SpearMan,
        Viking,
        Varvar,
        Knight,
        Ice
        
    }

    [SerializeField] private Skill skill;

    public void ActivateSkill()
	{
		AudioLibrarySO.instance.PlayClip(skillSound, 2);
		Debug.Log("Skill");
		switch (skill)
        {
            case Skill.SimpleMale:
                unitFSM.ChangeState(new SkillSimpleMale(this));
                break;
            case Skill.Shaman:
                unitFSM.ChangeState(new SkillShaman(this));
                break;
            case Skill.Asterix:
                unitFSM.ChangeState(new SkillAsterix(this));
                break;
            case Skill.FishMan:
                unitFSM.ChangeState(new SkillFishMan(this));
                break;
            case Skill.SimpleFemale:
                unitFSM.ChangeState(new SkillSimpleFemale(this));
                
                //Debug.Log("123");
                var coroutine1 = inst1;
                if (coroutine1 != null)
                {
                    StopCoroutine(coroutine1);
                    //Debug.Log(inst);
                }
                var coroutine = inst;
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                    //Debug.Log(inst);
                }
                inst=StartCoroutine(unitBlockDamage(unitDefTime));
                
                break;
            case Skill.SpearMan:
                unitFSM.ChangeState(new SkillSpearMan(this));
                //sxd
                StartCoroutine(unitExtAnimation(unitAbilityTime)); 
                break;
            case Skill.Viking:
                unitFSM.ChangeState(new SkillViking(this));
                break;      
            case Skill.Varvar:
                unitFSM.ChangeState(new SkillVarvar(this));
                break;
            case Skill.Ice:
                unitFSM.ChangeState(new SkillIce(this));
                break;
            case Skill.Knight:
                unitFSM.ChangeState(new SkillKnight(this));
                break;
            default: break;
        }

    }
    
    [SerializeField] public float unitHP;

    [SerializeField] private float unitSpeed;
    [SerializeField] private float unitDamage;
    [SerializeField] private float unitSkillDamage;
    [SerializeField] private float unitAbilityTime;
    [SerializeField] private float unitFreazeTime;
    public float cyrUnitFreezeTime = 0;
    
    public float UnitFreazeTime
    {
        get => unitFreazeTime;
        set => unitFreazeTime = value;
    }

    public float UnitDefTime
    {
        get => unitDefTime;
        set => unitDefTime = value;
    }

    [SerializeField] private float unitDefTime;

    public float UnitAbilityTime
    {
        get => unitAbilityTime;
        set => unitAbilityTime = value;
    }

    public float UnitSkillDamage
    {
        get => unitSkillDamage;
        set => unitSkillDamage = value;
    }

    //[HideInInspector] 
    [SerializeField] private float accuracy;
    [SerializeField] private float accuracyBOSS;
    
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform unitTarget;
    //[HideInInspector] 
    [SerializeField] private MeshRenderer meshRenderer;
    //[HideInInspector] 
    [SerializeField] private AnimationController animationController;

    //[HideInInspector] 
    [SerializeField] private Transform teleport;
    //[HideInInspector] 
    [SerializeField] private Transform death;
    [SerializeField] private Transform skillButton;

    public Transform SkillButton
    {
        get => skillButton;
        set => skillButton = value;
    }

    //[HideInInspector] 
    [SerializeField] private Transform hpBar;
    [SerializeField] private BarSystem valueBar;
    //[HideInInspector] 
    [SerializeField] private HPSystem hpValueBar;

    private UnitFSM unitFSM;
    private Transform unitCanvas;
    private float unitCyrHP;
    [SerializeField] private bool readyToBattle = false;
    private bool blockDamage = false;
    #endregion

    #region Публичные методы

    public void SetFreeze() => StartCoroutine(unitFreeze(unitFreazeTime));

    public void SetBlockDamage()
    {
        var coroutine1 = inst1;
        if (coroutine1 != null) StopCoroutine(coroutine1);
        var coroutine = inst;
        if (coroutine != null) StopCoroutine(coroutine);
        
        inst=StartCoroutine(unitBlockDamage(unitDefTime));
    }


    public void SetAttack() => unitFSM.ChangeState(new UnitAttack(this));
    public void SetIdle() => unitFSM.ChangeState(new UnitIdle(this));
    public void SetMove() => unitFSM.ChangeState(new UnitMove(this));

    public void SetTeleport()
    {
        if (CompareTag("Hero"))
        {
            //if (!(transform.GetComponent<MeshRenderer>().enabled)) 
                //Герой Перонаж не виден
              //  {
              if (SkillButton)
                  if(SkillButton.GetComponent<ButtonSkill>().targetGO)
                      SkillButton.GetComponent<ButtonSkill>().targetGO.GetComponent<SpriteRenderer>().enabled = false;
              
                    unitFSM.ChangeState(new UnitTeleport(this));
                    StartCoroutine(unitActivate());
                //}
                //else 
                // Задержка после убийства
               // WaitTime(0.3f);
        }
        else
        {
            StartCoroutine(unitWaitForBattle());
        }
        
    }

    
    
    public void SetDie()
    {
        //Подчищаем при смерти прицельных героев
        if (SkillButton)
        {
            switch (SkillButton.GetComponent<ButtonSkill>().UController.ToString())
            {
                case "Ice(Clone) (UnitController)":
                case "Knight(Clone) (UnitController)":
                case "Varvar(Clone) (UnitController)":
                    if(!SkillButton.GetComponent<ButtonSkill>().button.GetComponent<Button>().IsActive())
                    {
                        foreach (var target in GameObject.FindGameObjectsWithTag("Enemy"))
                        {
                            target.GetComponent<UnitController>().GetComponent<MeshRenderer>().sortingLayerName = "Unit";
                            target.GetComponent<UnitController>().GetComponent<MeshRenderer>().sortingOrder = 0;
                        }
                        foreach (var target in GameObject.FindGameObjectsWithTag("Hero"))
                        {
                            target.GetComponent<UnitController>().GetComponent<MeshRenderer>().sortingLayerName = "Unit";
                            target.GetComponent<UnitController>().GetComponent<MeshRenderer>().sortingOrder = 0;
                        }
                    }
                break;
                default: break;
            }
            if(SkillButton.GetComponent<ButtonSkill>().targetGO)
                SkillButton.GetComponent<ButtonSkill>().targetGO.GetComponent<SpriteRenderer>().enabled = false;
        }
        
        StartCoroutine(unitActivate());
        Destroy(hpBar.gameObject);
        transform.tag = "Die";
        AudioLibrarySO.instance.PlayClip(deathSound, 3);
        unitFSM.ChangeState(new UnitDie(this));
    }

    public void SetSearchTarget()
    {
        unitTarget = null;
        var nearestTarget = tag == "Hero"
            ? GameObject.FindGameObjectsWithTag("Enemy")
            : GameObject.FindGameObjectsWithTag("Hero");
        float dist = 500f;
        foreach (var n in nearestTarget)
        {
            var cyrDist = Vector2.Distance(transform.position, n.transform.position);
            if (cyrDist < dist)
            {
                dist = cyrDist;
                unitTarget = n.transform;
            }
        }

        if (unitTarget) SetMove();
        else
        {
            if (CompareTag("Hero"))
                // Добавить задержку
            {
                SetIdle();
                StartCoroutine(WaitTime(0.5f));
            }
            //SetTeleport();
            else SetIdle();
            //Debug.Log("Зафигачили Всех!");
        }
    }
    
    

    private bool waitTarget=false;
    public bool startGame=false;
    public void SetWaitTarget()
    {
        SetIdle();
        waitTarget = true;
    }
    #endregion

    #region Приватные методы

    private void Start()
    {
        // Считываем характеристики
        var gameMNGR = GameObject.Find("GameManager");

        string name = gameMNGR.GetComponent<GameManager>().HeroList.Hero[HeroDBIndex].HeroName;
        // По имени находим стат
        foreach (var cyr in gameMNGR.GetComponent<GameManager>().HeroData.dataArray)
        {
            if(cyr.Id==name)unitSkillDamage=cyr.Abilitystat;
        }
        
        unitFSM = new UnitFSM();
        unitFSM.Initialize(new UnitIdle(this));

        meshRenderer = GetComponent<MeshRenderer>();
        unitCanvas = CompareTag("Hero")
            ? GameObject.Find("Hero Canvas").transform
            : GameObject.Find("Enemy Canvas").transform;
        hpBar.SetParent(unitCanvas);
        unitCyrHP = unitHP;
        if (CompareTag("Hero"))
        {
            unitCanvas = GameObject.Find("Panel Button").transform;
            skillButton.SetParent(unitCanvas);
        }else transform.localScale = new Vector3(-1f, 1f, 1f);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == "skill_aim")
        {
            this.GetComponent<Collider2D>().enabled = false;
            SetBlockDamage();
            var nearestTarget = GameObject.FindGameObjectsWithTag("Hero");

            foreach (var n in nearestTarget)
            {
                n.GetComponent<Collider2D>().enabled = false;
            }

            StartCoroutine(unitBlockD(2f));
        }
    }
    public void OnCollider()
    {
            this.GetComponent<Collider2D>().enabled = false;
            SetBlockDamage();
            var nearestTarget = GameObject.FindGameObjectsWithTag("Hero");

            foreach (var n in nearestTarget)
            {
                n.GetComponent<Collider2D>().enabled = false;
            }

            StartCoroutine(unitBlockD(2f));
    }


    private void Update()
    {
        /*if (Input.GetKeyDown("space"))
        {
            if( inst != null ) StopCoroutine(inst);
            if( inst1 != null) StopCoroutine(inst1);
        }*/
        
        //Текст над юнитами
        tempText.text = "HP: "+unitHP.ToString()+"<br>"+"DMG: "+unitDamage;
        
        
        if(!meshRenderer.enabled)
            if(freezeEffect!=null)freezeEffect.GetComponent<MeshRenderer>().enabled = false;
        
        unitFSM.CurrentState.Update();
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
                foreach (var n in nearestTarget.Where(x => x.GetComponent<UnitController>().startPoint == startPoint))
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
                }

                if (!targetUnitStart)
                {
                    SetSearchTarget();
                }

                if (unitTarget != null)
                {
                    waitTarget = false;
                    SetMove();
                }
            }
        }
    }

    private void ChangeDirection()
    {
        if (startGame)
        {
            if (transform.position.x > 8f || transform.position.x < -8f)
                transform.position = new Vector3(transform.position.x > 0 ? 8f : -8f, transform.position.y,
                    transform.position.z);
            if (transform.position.y > 5f || transform.position.y < -5f)
                transform.position = new Vector3(transform.position.x, transform.position.y > 0 ? 5f : -5f,
                    transform.position.y > 0 ? 5f : -5f);
            switch (readyToBattle)
            {
                case false:
                {
                    if (startPoint != null)
                    {
                        transform.localScale = (transform.position - startPoint.position).x > 0
                            ? new Vector3(-1, 1, 1)
                            : new Vector3(1, 1, 1);
                    }

                    break;
                }
                default:
                {
                    if (unitTarget != null)
                    {
                        transform.localScale = (transform.position - unitTarget.position).x > 0
                            ? new Vector3(-1, 1, 1)
                            : new Vector3(1, 1, 1);

                    }

                    break;
                }
            }
        }
    }

    IEnumerator unitActivate()
    {
        var enabled = meshRenderer.enabled;
        if (enabled) readyToBattle = false;
        if (!enabled)
        {
            yield return new WaitForSeconds(0.5f);
        }
        
        // Отключаем бары
        hpBar.Find("Group").gameObject.SetActive(!enabled);
        if(enabled)hpBar.Find("GroupDef").gameObject.SetActive(!enabled);
        if(enabled&& animationController.name=="SpearMan(Clone)")hpBar.Find("GroupExt").gameObject.SetActive(!enabled);
        
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
            startGame = true;
            SetMove();
        }

        yield return null;
    }

    IEnumerator unitWaitForBattle()
    {
        yield return new WaitForSeconds(1f);
        startGame = true;
        SetMove();
        yield return null;
    }


    IEnumerator unitBarTime(float time)
    {
        // Активируем панель заморозки
        hpBar.Find("GroupDef").gameObject.SetActive(enabled);
        
        while (time > 0f)
        {
            time -= Time.deltaTime;
            if(valueBar)valueBar.SetBarValue = time;
          //  Debug.Log("Значение: "+time);
            yield return null;
        }
        //Debug.Log("Завершили цикл");
        if(valueBar)valueBar.SetBarValue = 1;
        
        if(hpBar)hpBar.Find("GroupDef").gameObject.SetActive(!enabled);
        yield return null;
    }

    IEnumerator unitBarExtTime(float time)
    {
        // Активируем панель заморозки
        hpBar.Find("GroupExt").gameObject.SetActive(enabled);
        
        var cyr = hpBar.GetComponent<ExtBarSystem>();
        
        //Debug.Log("Значение: "+time+" - "+cyr.name);
        
        while (time > 0f)
        {
            time -= Time.deltaTime;
            if(cyr)cyr.SetBarValue = time;
                //Debug.Log("Значение: "+time);
            yield return null;
        }
        //Debug.Log("Завершили расширенную анимацию");
        if(cyr)cyr.SetBarValue = 1;
        
        if(hpBar)hpBar.Find("GroupExt").gameObject.SetActive(!enabled);

        //unitFSM.ChangeState();
            
        yield return null;
    }
    
    IEnumerator unitFreeze(float time)
    {
        // Запускаем корутину уменьшения значения панели
        StartCoroutine(unitBarTime(time));
        
        if(freezeEffect!=null)freezeEffect.GetComponent<MeshRenderer>().enabled = true;
        unitSpeed = unitSpeed / 2;
        GetComponent<SkeletonAnimation>().timeScale = 0.5f;
        if(transform.name!="BossRock(Clone)" && transform.name!="BigFoot(Clone)" && transform.name!="Demon(Clone)")
            transform.GetComponent<Renderer>().material.shader = shader_on;
        
        //unitCyrBar = time;
        /*var cycleTime = time;
        do
        {
            if(transform.GetComponent<Renderer>().material.shader!=shader_on)
                transform.GetComponent<Renderer>().material.shader = shader_on;
            yield return 0;
            cycleTime -= Time.deltaTime;
        } while (cycleTime>=0f);
        */
        yield return new WaitForSeconds(time);
        unitSpeed = unitSpeed * 2;
        //Debug.Log("Сброс шейдера");
        transform.GetComponent<Renderer>().material.shader = shader_off;
        GetComponent<SkeletonAnimation>().timeScale = 1f;
        if(freezeEffect!=null)freezeEffect.GetComponent<MeshRenderer>().enabled = false;
        yield return null;
    }
    
    IEnumerator unitBlockDamage(float time)
    {
        if (transform.name == "SpearMan(Clone)")
           hpBar.Find("GroupExt").GetComponent<RectTransform>().anchoredPosition=new Vector3(0,44,0);
        
        inst1=StartCoroutine(unitBarTime(time));
        blockDamage = true;
        yield return new WaitForSeconds(time);
        blockDamage = false;
        
        if (transform.name == "SpearMan(Clone)")
            hpBar.Find("GroupExt").GetComponent<RectTransform>().anchoredPosition=new Vector3(0,22,0);
        yield return null;
    }
    
    IEnumerator unitExtAnimation(float time)
    {
        if (hpBar.Find("GroupDef").gameObject.activeSelf)
            hpBar.Find("GroupExt").GetComponent<RectTransform>().anchoredPosition=new Vector3(0,44,0);
        else
            hpBar.Find("GroupExt").GetComponent<RectTransform>().anchoredPosition=new Vector3(0,22,0);
        
        StartCoroutine(unitBarExtTime(time));
        animationController.ExtAnimationSwitch = true;
        //Debug.Log("Пикенёр:"+time);
        yield return new WaitForSeconds(time);
        //Debug.Log("Пикенёр завершил анимацию");
        animationController.ExtAnimationSwitch = false;
        
        yield return null;
    }

    IEnumerator unitBlockD(float time)
    {
        GetComponent<MeshRenderer>().sortingLayerName = "UI";
        GetComponent<MeshRenderer>().sortingOrder = 6;
        yield return new WaitForSeconds(time);
        GetComponent<MeshRenderer>().sortingLayerName = "Unit";
        GetComponent<MeshRenderer>().sortingOrder = 0;
        yield return null;
    }
    
    #endregion
}
