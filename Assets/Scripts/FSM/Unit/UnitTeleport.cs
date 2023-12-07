using System.Collections;
using Spine.Unity;
using UnityEngine;

public class UnitTeleport : State
{
	#region Приватные поля

	private UnitController unitController;

	#endregion

	#region Публичные Методы

	public UnitTeleport(UnitController unitController)
	{
		this.unitController = unitController;
	}

	public override void Enter()
	{
		unitController.AnimationController.SetIdle();

		if (unitController.transform.CompareTag("Hero"))
		{
			unitController.Teleport.GetComponent<SkeletonAnimation>().state.SetAnimation(0, "teleport", false);
			AudioLibrarySO.instance.PlayTelepInSound();
		}

	}

	#endregion
}
