using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameHeroesSet : State
{
    #region Приватные поля

    private GameManager gameManager;

    #endregion

    #region Публичные Методы

    public gameHeroesSet(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public override void Enter()
    {
        for (int index = 0; index < gameManager.HeroUnits.Count; index++)
        {
            gameManager.HeroUnits[index].transform.position = gameManager.LevelDB.HeroPoint[index].position;
            if(gameManager.LevelDB.BattlePoint.Count>0)gameManager.HeroUnits[index].GetComponent<UnitController>().StartPoint =
                gameManager.LevelDB.BattlePoint[index];
        }

        gameManager.ChangeState(GameManager.State.EnemySet);
    }


    #endregion
}
