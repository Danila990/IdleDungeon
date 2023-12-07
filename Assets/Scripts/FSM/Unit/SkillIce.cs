using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

public class SkillIce : State
{
 #region Приватные поля

    private UnitController unitController;
    private SkeletonAnimation skeletonAnimation;
    public float distance = 1.5f;
    
    #endregion

    #region Публичные Методы

    public SkillIce(UnitController unitController)
    {
        this.unitController = unitController;
    }

    
    public override void Enter()
    {
       GameObject blackScreen = GameObject.Find("BlackScreen");
       blackScreen.GetComponent<BlackScreen>().timer += 1f-blackScreen.GetComponent<BlackScreen>().timer;
  
        skeletonAnimation = unitController.GetComponent<SkeletonAnimation>();
     
        unitController.GetComponent<MeshRenderer>().sortingLayerName = "UI";
        unitController.GetComponent<MeshRenderer>().sortingOrder = 9;
        unitController.magic.SetActive(false);
        unitController.magic.SetActive(true);
        //unitController.transform.localScale = new Vector3(1.5f,1.5f,1f);
        // Цель жива и Если расстояние до цели меньше чем accyracy иначе меняем состояние на движение к цели
        skeletonAnimation.AnimationState.Start  += HandleStart;
        skeletonAnimation.AnimationState.Complete  += HandleComplete;
        unitController.AnimationController.SetSkill();
        // Иначе переключаемся на поиск ближайших целей
    }

    private void HandleStart(TrackEntry trackentry)
    {
        if (trackentry.ToString() == "ability_active")
        {
            var targetGO = GameObject.Find("NovaFrost");
            var nearestTarget = GameObject.FindGameObjectsWithTag("Enemy");
        
            foreach (var n in nearestTarget)
            {
                    if(Vector2.Distance(targetGO.transform.position,n.transform.position)<distance)
                    {
                        n.GetComponent<UnitController>().SetFreeze();
                        
                        
                        //var Prefab = Resources.Load("NovaFrost") as GameObject;
                        //GameObject.Instantiate(Prefab, n.transform.position, Quaternion.identity);
                        
                        //.UnitCyrHP -= unitController.UnitDamage*3f;
                    }
            }
        }
    }

    private void HandleComplete(TrackEntry trackentry)
    {
        if (trackentry.ToString() == "ability_active")
        {
            var targetGO = GameObject.Find("NovaFrost");
            GameObject.Destroy(targetGO);
            unitController.SetMove(); 
        }
    }

    public override void Update()
    {
//        if(unitController.UnitTarget.GetComponent<UnitController>().UnitCyrHP <= 0f)unitController.SetSearchTarget();
    }

 
    public override void Exit()
    { 
        //Debug.Log("Exit");
        unitController.GetComponent<MeshRenderer>().sortingLayerName = "Unit";
        unitController.GetComponent<MeshRenderer>().sortingOrder = 0;
        
        foreach (var target in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            target.GetComponent<UnitController>().GetComponent<MeshRenderer>().sortingLayerName = "Unit";
            target.GetComponent<UnitController>().GetComponent<MeshRenderer>().sortingOrder = 0;
        }
        //unitController.transform.localScale = new Vector3(1f,1f,1f);
        skeletonAnimation.AnimationState.Complete  -= HandleStart;
        skeletonAnimation.AnimationState.Complete  -= HandleComplete;
    }

    #endregion
}
