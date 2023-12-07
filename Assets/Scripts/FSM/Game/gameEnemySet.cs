using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameEnemySet : State
{
    #region Приватные поля

    private GameManager gameManager;
    private bool endState;

    #endregion

    #region Публичные Методы

    public gameEnemySet(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public override void Enter()
    {
        endState = false;
    }

    public override void Update()
    {
        if (!endState&&GameObject.Find("EnemyPoints").GetComponent<EnemySpawner>().LoadComplete)
        {
            endState = true;
            var Enemys = GameObject.FindGameObjectsWithTag("Enemy");
    
            gameManager.EnemyUnits.Clear();
        
            int [] mas = new int[6];
            var n1 = Enemys.Length % gameManager.HeroUnits.Count;
            var n2 = (Enemys.Length - n1) / gameManager.HeroUnits.Count;
            for (int index = 0; index < gameManager.HeroUnits.Count; index++)
            {
                mas[index]= n2+(n1>0?1:0);
                n1--;
            }


            List<GameObject> tempEnemy=new List<GameObject>();
            
            for (int index = 0; index < Enemys.Length; index++)
            {
                gameManager.EnemyUnits.Add(Enemys[index]);
                tempEnemy.Add(Enemys[index]);
            }

            // Перебираем массив точек
            if(gameManager.LevelDB.BattlePoint.Count>0)
            for (int index = 0; index < gameManager.HeroUnits.Count; index++)
            {
                while (mas[index]>0)
                {
                    // Уменьшаем количество юнитов на точке
                    mas[index]--;
                    float dist = 0f;
                    int indexMas = 0;
                    GameObject target=null;
                    
                    // Перебираем массив ищем самых дальних от точки
                    for (var i = 0; i < tempEnemy.Count; i++)
                    {
                        if (tempEnemy[i] != null)
                        {
                            var d = Vector2.Distance(tempEnemy[i].transform.position,
                                gameManager.LevelDB.BattlePoint[index].transform.position);
                            if (d > dist)
                            {
                                target = tempEnemy[i];
                                dist = d;
                                indexMas = i;
                            }
                        }
                    }
                    
                    gameManager.EnemyUnits[indexMas].GetComponent<UnitController>().StartPoint = gameManager.LevelDB.BattlePoint[index];
                    tempEnemy[indexMas]=null;
                    
                }
               
                
                
            }
           
            for (int index = 0; index < Enemys.Length; index++)
            {
                    gameManager.EnemyUnits.Add(Enemys[index]);
            }

            gameManager.ChangeState(GameManager.State.StartGame);            
        }
    }
    
    #endregion
}
