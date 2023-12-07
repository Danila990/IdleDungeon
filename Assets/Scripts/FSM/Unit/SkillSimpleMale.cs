using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

public class SkillSimpleMale : State
{
    #region Приватные поля

    private UnitController unitController;
    private SkeletonAnimation skeletonAnimation;
    private float damage;
    
    #endregion

    #region Публичные Методы

    public SkillSimpleMale(UnitController unitController)
    {
        this.unitController = unitController;
    }

    public override void Enter()
    {
        damage=unitController.UnitDamage;

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
            //Debug.Log("Абилка");


            //if (e.Data.Name == "shoot")
            //{
            if (unitController.UnitTarget != null &&
                Vector2.Distance(unitController.transform.position, unitController.UnitTarget.transform.position) <=
                unitController.Accuracy)
            {
                unitController.UnitTarget.GetComponent<UnitController>().UnitCyrHP -=unitController.UnitDamage*unitController.UnitSkillDamage;
            }

            unitController.SetMove();
        }
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
        unitController.GetComponent<MeshRenderer>().sortingLayerName = "Unit";
        unitController.GetComponent<MeshRenderer>().sortingOrder = 0;
        //unitController.transform.localScale = new Vector3(1f,1f,1f);
        skeletonAnimation.AnimationState.Complete  -= HandleComplete;
    }

    #endregion
}
