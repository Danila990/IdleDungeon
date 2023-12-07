using Spine;
using Spine.Unity;
using UnityEngine;

public class SkillShaman : State
{
    #region Приватные поля

    private UnitController unitController;
    private SkeletonAnimation skeletonAnimation;
    private Transform skillEffect;
    private float heal;
    private GameObject target=null;
    
    #endregion

    #region Публичные Методы

    public SkillShaman(UnitController unitController)
    {
        this.unitController = unitController;
    }

    public override void Enter()
    {
        heal = unitController.UnitSkillDamage;
        
        GameObject blackScreen = GameObject.Find("BlackScreen");
        blackScreen.GetComponent<BlackScreen>().timer += 1f-blackScreen.GetComponent<BlackScreen>().timer;
        
        skeletonAnimation = unitController.GetComponent<SkeletonAnimation>();
        
        unitController.GetComponent<MeshRenderer>().sortingLayerName = "UI";
        unitController.GetComponent<MeshRenderer>().sortingOrder = 9;
        unitController.transform.localScale = new Vector3(1.5f,1.5f,1f);
        
        unitController.AnimationController.SetSkill();
        skeletonAnimation.AnimationState.Complete  += HandleComplete;
        
        var nearestTarget = GameObject.FindGameObjectsWithTag("Hero");
        
        float minHP=10000000;
        foreach (var n in nearestTarget)
        {
            if (minHP > n.GetComponent<UnitController>().UnitCyrHP)
            {
                minHP = n.GetComponent<UnitController>().UnitCyrHP;
                target = n;
            }
        }

        target.GetComponent<UnitController>().GetComponent<MeshRenderer>().sortingLayerName = "UI";
        target.GetComponent<UnitController>().GetComponent<MeshRenderer>().sortingOrder = 9;
        unitController.magic.SetActive(false);
        unitController.magic.SetActive(true);
        target.GetComponent<UnitController>().magic.SetActive(false);
        target.GetComponent<UnitController>().magic.SetActive(true);
        //target.transform.localScale = new Vector3(1.5f,1.5f,1f);
        
        skillEffect=unitController.transform.Find("SkillEffect");
        skillEffect.GetComponent<MeshRenderer>().enabled = true;
        skillEffect.position = target.transform.position;
        //skillEffect.GetComponent<SkeletonAnimation>().state.SetAnimation(0, "idle", false);
        
    }

    private void HandleComplete(TrackEntry trackentry)
    {
        
        if (trackentry.ToString() == "ability_active")
        {
            
            
            //Debug.Log("Абилка");
            /*var nearestTarget = GameObject.FindGameObjectsWithTag("Hero");
            GameObject target=null;
            float minHP=10000000;
            foreach (var n in nearestTarget)
            {
                if (minHP > n.GetComponent<UnitController>().UnitCyrHP)
                {
                    minHP = n.GetComponent<UnitController>().UnitCyrHP;
                    target = n;
                }
            }*/
            target.GetComponent<UnitController>().UnitCyrHP += target.GetComponent<UnitController>().unitHP/100*unitController.UnitSkillDamage;
            skillEffect.GetComponent<MeshRenderer>().enabled = false;
            unitController.SetMove();
        }
         
        
       
        //if (unitController.UnitTarget != null &&
        //    Vector2.Distance(unitController.transform.position, unitController.UnitTarget.transform.position) <=
        //    unitController.Accuracy)
        //{
            
        //}
        //unitController.SetAttack();
    }

    public override void Update()
    {
        if (target != null) skillEffect.position = target.transform.position;
        //        if(unitController.UnitTarget.GetComponent<UnitController>().UnitCyrHP <= 0f)unitController.SetSearchTarget();
    }

    public override void Exit()
    {
        skillEffect.GetComponent<MeshRenderer>().enabled = false;
        
        unitController.GetComponent<MeshRenderer>().sortingLayerName = "Unit";
        unitController.GetComponent<MeshRenderer>().sortingOrder = 0;
        //unitController.magic.SetActive(false);
        //unitController.transform.localScale = new Vector3(1f,1f,1f);
        
            
        target.GetComponent<UnitController>().GetComponent<MeshRenderer>().sortingLayerName = "Unit";
        target.GetComponent<UnitController>().GetComponent<MeshRenderer>().sortingOrder = 0;
        target.transform.localScale = new Vector3(1f,1f,1f);
        
        skeletonAnimation.AnimationState.Complete  -= HandleComplete;
    }

    #endregion
}
