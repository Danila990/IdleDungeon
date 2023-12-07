using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

public class SkillSimpleFemale : State
{
    #region Приватные поля

    private UnitController unitController;
    private SkeletonAnimation skeletonAnimation;
    
    #endregion

    #region Публичные Методы

    public SkillSimpleFemale(UnitController unitController)
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
        
        unitController.AnimationController.SetSkill();
        skeletonAnimation.AnimationState.Complete  += HandleComplete;
        
    }

    private void HandleComplete(TrackEntry trackentry)
    {
        if (trackentry.ToString() == "ability_active")
        {
            //Debug.Log("Абилка");

            unitController.SetMove();
        }
    }

    public override void Update()
    {
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
