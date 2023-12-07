public class UnitIdle : State
{
    #region Приватные поля

    private UnitController unitController;

    #endregion

    #region Публичные Методы

    public UnitIdle(UnitController unitController)
    {
        this.unitController = unitController;
    }

    public override void Enter()
    {
        unitController.AnimationController.SetIdle();
    }

    #endregion
}
