using Spine.Unity;

public class UnitDie : State
{
    #region Приватные поля

    private UnitController unitController;

    #endregion

    #region Публичные Методы

    public UnitDie(UnitController unitController)
    {
        this.unitController = unitController;
    }

    public override void Enter()
    {
        unitController.AnimationController.SetIdle();
        unitController.Death.GetComponent<SkeletonAnimation>().state.SetAnimation(0, "death", false);
    }

    #endregion
}
