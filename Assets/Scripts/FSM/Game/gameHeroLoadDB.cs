using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class gameHeroLoadDB : State
{
    #region Приватные поля

    private GameManager gameManager;

    #endregion

    #region Публичные Методы

    public gameHeroLoadDB(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public override void Enter()
    {
        for (int index = 0; index < gameManager.HeroDB.HeroList.Count; index++)
        {
            var hero = Object.Instantiate(gameManager.HeroDB.HeroList[index], gameManager.transform.position,
                Quaternion.identity);
            hero.transform.SetParent(gameManager.transform);
            gameManager.HeroUnits.Add(hero);
        }

        //gameManager.ChangeState(GameManager.State.HeroesSet);
        gameManager.ChangeState(GameManager.State.LoadLevel);
    }


    #endregion
}
