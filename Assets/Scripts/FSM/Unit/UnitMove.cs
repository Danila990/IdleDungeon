using UnityEngine;

public class UnitMove : State
{
    #region Приватные поля

    private UnitController unitController;

    // Для хранения кеша
    private Transform unit;
    private Transform startPoint;
    private Transform unitTarget;
    private GameObject startPointGO;
    private GameObject unitTargetGO;
    private float speed;

    #endregion

    #region Публичные методы

    public UnitMove(UnitController unitController)
    {
        this.unitController = unitController;
    }

    public override void Enter()
    {
        unitController.AnimationController.SetMove();

        // Кешируем обращения
        if (unitController.StartPoint != null)
        {
            startPoint = unitController.StartPoint;//.transform;
            //startPointGO = unitController.StartPoint;
        }

        if (unitController.UnitTarget != null)
        {
            unitTarget = unitController.UnitTarget;//.transform;
            //unitTargetGO = unitController.UnitTarget;
        }

        unit = unitController.transform;
        speed = unitController.UnitSpeed;
    }

    public override void Update()
    {
        if (startPoint == null)
        {
            unitController.ReadyToBattle = true;
        }

        var readyToBattle = unitController.ReadyToBattle;
        
            
        // Движение к точки боя
        if (!readyToBattle)
        {
            unit.position = Vector2.MoveTowards(unit.position, startPoint.position, speed * Time.deltaTime);
            unit.position = new Vector3(unit.position.x, unit.position.y, unit.position.y);
            if (Vector2.Distance(unit.position, startPoint.position) < unitController.Accuracy)
            {
                unitController.ReadyToBattle = true;
            }
        }

        if (readyToBattle)
        {
            if (unitController.UnitTarget == null)
            {
                //Debug.Log("Ждём");
                unitController.SetWaitTarget();
            }
            else
            {
                if (unitController.UnitTarget.GetComponent<UnitController>().UnitCyrHP > 0f)
                {
                    //Debug.Log("К цели");
                    unit.position = Vector2.MoveTowards(unit.position, unitController.UnitTarget.position,
                        speed * Time.deltaTime);
                    unit.position = new Vector3(unit.position.x, unit.position.y, unit.position.y);
                    if (Vector2.Distance(unit.position, unitController.UnitTarget.position) < unitController.Accuracy)
                    {
                        unitController.SetAttack();
                    }
                }else unitController.SetSearchTarget();
            }
        }

        //if (startPointGO)
            //    ;else if (unitTarget) ;
        // Проверку на существование цели и стартовой точки
        /*if(readyToBattle&&unitTargetGO==null)unitController.SetWaitTarget();

        if (readyToBattle && unitTargetGO != null)
        {
            unit.position = Vector2.MoveTowards(unit.position, unitTarget.position, speed * Time.deltaTime);
            
        }

        if (!readyToBattle)
        {
            unit.position = Vector2.MoveTowards(unit.position, startPoint.position, speed * Time.deltaTime);
        }
        
        unit.position = new Vector3(unit.position.x, unit.position.y, unit.position.y); 
        if (Vector2.Distance(unit.position, readyToBattle?unitTarget.position:startPoint.position) < unitController.Accuracy)
        {
            if (!readyToBattle)
            {
                unitController.ReadyToBattle = true;
                //unitController.SetWaitTarget();// убрать!!! Только для тестов
            }
            else 
                if(unitTargetGO!=null)unitController.SetAttack();
        }*/
    }

    #endregion
}
