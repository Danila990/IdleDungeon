using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

public class SkillSpearMan : State
{
    #region Приватные поля

    private UnitController unitController;
    private SkeletonAnimation skeletonAnimation;
    
    #endregion

    #region Публичные Методы

    public SkillSpearMan(UnitController unitController)
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
        unitController.AnimationController.SetSkill();
        skeletonAnimation.AnimationState.Complete  += HandleComplete;
        // Иначе переключаемся на поиск ближайших целей
    }

    private void HandleComplete(TrackEntry trackentry)
    {
        if (trackentry.ToString() == "ability_active")
        {
           // if (unitController.UnitTarget != null &&
           //     Vector2.Distance(unitController.transform.position, unitController.UnitTarget.transform.position) <=
           //     unitController.Accuracy)
           // {
                //unitController.UnitTarget.GetComponent<UnitController>().UnitCyrHP -=(unitController.UnitDamage*unitController.UnitSkillDamage)/5;
                //Debug.Log("Super Attack");
           // }
            unitController.SetMove();
        }
    }

    public override void Update()
    {
//        if(unitController.UnitTarget.GetComponent<UnitController>().UnitCyrHP <= 0f)unitController.SetSearchTarget();
    }

    public override void Exit()
    {
        unitController.GetComponent<MeshRenderer>().sortingLayerName = "Unit";
        unitController.GetComponent<MeshRenderer>().sortingOrder = 0;
        //unitController.magic.SetActive(false);
        //unitController.transform.localScale = new Vector3(1f,1f,1f);
        skeletonAnimation.AnimationState.Complete  -= HandleComplete;
    }

    #endregion
}
