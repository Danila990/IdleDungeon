using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;
using YG;

public class UnitAttack : State
{
	#region Приватные поля

	private UnitController unitController;
	private SkeletonAnimation skeletonAnimation;

	private float attackSoundDelay;
	private float attackTimer;

	#endregion

	#region Публичные Методы

	public UnitAttack(UnitController unitController)
	{
		this.unitController = unitController;
		attackSoundDelay = unitController.attackSound.length;
	}

	public override void Enter()
	{
		skeletonAnimation = unitController.GetComponent<SkeletonAnimation>();

		// Цель жива и Если расстояние до цели меньше чем accyracy иначе меняем состояние на движение к цели

		skeletonAnimation.AnimationState.Event += HandleEvent;

		unitController.AnimationController.SetAttack();

	}

	private void HandleEvent(TrackEntry trackEntry, Spine.Event e)
	{
		//Debug.Log(e.Data.Name); 
		if (unitController.UnitTarget != null && Vector2.Distance(unitController.transform.position, unitController.UnitTarget.transform.position) <= unitController.Accuracy)
		{
			if (e.Data.Name == "shoot")
			{
				switch (unitController.UnitSkill)
				{
					case UnitController.Skill.Viking:
						unitController.UnitTarget.GetComponent<UnitController>().UnitCyrHP -= unitController.UnitDamage / 3;
						break;
					case UnitController.Skill.SpearMan:
						if (unitController.AnimationController.ExtAnimationSwitch)
							unitController.UnitTarget.GetComponent<UnitController>().UnitCyrHP -=
								unitController.UnitDamage / 5;
						else
							unitController.UnitTarget.GetComponent<UnitController>().UnitCyrHP -=
								unitController.UnitDamage;
						break;
					default:
						unitController.UnitTarget.GetComponent<UnitController>().UnitCyrHP -= unitController.UnitDamage;
						break;
				}

				//Debug.Log("Фигачим");
				//Debug.Log(e.Data.Name);
				if (attackTimer == 0 || Time.time - attackTimer > attackSoundDelay)
				{
					Debug.Log($"AttackSound!!! {unitController.name} {Time.fixedDeltaTime} {Time.timeScale} {unitController.attackSound.length}");
					AudioLibrarySO.instance.PlayClip(unitController.attackSound, 9);
					attackTimer = Time.time;
				}
			}
		}
		else unitController.SetMove();
	}

	public override void Update()
	{
		if (unitController.UnitTarget != null && unitController.UnitTarget.GetComponent<UnitController>().UnitCyrHP <= 0f) unitController.SetSearchTarget();
	}

	public override void Exit()
	{
		skeletonAnimation.AnimationState.Event -= HandleEvent;
	}

	#endregion
}
