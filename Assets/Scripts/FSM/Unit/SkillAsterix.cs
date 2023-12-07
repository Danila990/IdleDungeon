using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

public class SkillAsterix : State
{
    #region Приватные поля

    private UnitController unitController;
    private SkeletonAnimation skeletonAnimation;
    private float distance = 3f;
    
    #endregion

    #region Публичные Методы

    public SkillAsterix(UnitController unitController)
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

        //unitController.StartCoroutine(testCoroutine());
        
        // Цель жива и Если расстояние до цели меньше чем accyracy иначе меняем состояние на движение к цели
        unitController.AnimationController.SetSkill();
        
        skeletonAnimation.AnimationState.Complete  += HandleComplete;
        // Иначе переключаемся на поиск ближайших целей
        
    }

    private void HandleComplete(TrackEntry trackentry)
    {
        if (trackentry.ToString() == "ability_active")
        {
            Vector3 position = unitController.transform.position;
            Vector3 rightDir = unitController.transform.right;
            
            var nearestTarget = GameObject.FindGameObjectsWithTag("Enemy");
        
            foreach (var n in nearestTarget)
            {
                
                Vector3 target = n.transform.position;
                var dir = (target - position).normalized;
                var dot = Vector3.Dot(dir, rightDir);

                if(unitController.transform.localScale.x>=0&&dot >= 0 || unitController.transform.localScale.x<=0 && dot <= 0)
                    if(Vector2.Distance(unitController.transform.position,n.transform.position)<distance)
                    {
                        n.GetComponent<UnitController>().UnitCyrHP -= unitController.UnitDamage*unitController.UnitSkillDamage;
                    }
            }
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
        //unitController.transform.localScale = new Vector3(1f,1f,1f);
        skeletonAnimation.AnimationState.Complete  -= HandleComplete;
    }

    #endregion
}
