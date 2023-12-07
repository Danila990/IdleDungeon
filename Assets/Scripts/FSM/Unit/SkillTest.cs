using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

public class SkillTest : State
{
   #region Приватные поля

    private UnitController unitController;
    private SkeletonAnimation skeletonAnimation;

    #endregion

    #region Публичные Методы

    public SkillTest(UnitController unitController)
    {
        this.unitController = unitController;
    }

    public override void Enter()
    {
        skeletonAnimation = unitController.GetComponent<SkeletonAnimation>();

        // Цель жива и Если расстояние до цели меньше чем accyracy иначе меняем состояние на движение к цели
        unitController.AnimationController.SetSkill();
        skeletonAnimation.AnimationState.Complete  += HandleComplete;
        // Иначе переключаемся на поиск ближайших целей
    }

    private void HandleComplete(TrackEntry trackentry)
    {
        if (trackentry.ToString() == "ability_active")
        {
            //Debug.Log("Абилка");    
        }
         
        //if (e.Data.Name == "shoot")
        //{
            unitController.UnitTarget.GetComponent<UnitController>().UnitCyrHP -= unitController.UnitDamage*3;
            unitController.SetAttack();
        //    Debug.Log("Фигачим");
            //Debug.Log(e.Data.Name);
        //}
    }

    public override void Update()
    {
//        if(unitController.UnitTarget.GetComponent<UnitController>().UnitCyrHP <= 0f)unitController.SetSearchTarget();
    }

    public override void Exit()
    {
        skeletonAnimation.AnimationState.Complete  -= HandleComplete;
    }

    #endregion
}
