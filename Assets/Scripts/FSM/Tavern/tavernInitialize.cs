using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tavernInitialize : State
{
    #region Приватные поля
    private TavernManager tavernManager;
    #endregion

    #region Публичные Методы

    public tavernInitialize(TavernManager tavernManager)
    {
        this.tavernManager = tavernManager;
    }
    
    public override void Enter()
    {
        //Отчищаем список героев в таверне
        tavernManager.Hero.Clear();
        
        //Начинаем добавлять героев
        //Debug.Log("HeroAdd");
        //for (int index = 0; index < tavernManager.HeroList.Place.Count; index++)
        for (int index = 0; index < tavernManager.HeroList.Place.Count; index++)
        {
            //Если в списке героев есть герой для таверны Place>=0 работаем с ним
            if (tavernManager.HeroList.Place[index] >= 0)
            {
                //var cyrHeroList = tavernManager.HeroList.Hero[index];
                // Блокируем ячейки для выбора героя
                //tavernManager.HeroFrameList.GetComponent<HeroFrameListInfo>().FrameAndButton[index].Button.SetActive(false);
                tavernManager.HeroFrameList.GetComponent<HeroFrameListInfo>().FrameAndButton[index].Frame.SetActive(true);
                
                //Добавляем героя в список героев таверны
                tavernManager.Hero.Add(Object.Instantiate(tavernManager.HeroList.Unit[index], tavernManager.HeroUnitsPoint[tavernManager.HeroList.Place[index]].transform.position, Quaternion.identity));
                //Назначаем точки расстановки
                tavernManager.Hero[tavernManager.Hero.Count-1].GetComponent<UnitMoveToPos>().StartPoint = tavernManager.HeroUnitsPoint[tavernManager.HeroList.Place[index]].transform;
                tavernManager.Hero[tavernManager.Hero.Count-1].GetComponent<UnitMoveToPos>().PointFrame = tavernManager.HeroUnitsFrame[tavernManager.HeroList.Place[index]].transform;
                //Разворачиваем мордочкой в случайную сторону
                tavernManager.Hero[tavernManager.Hero.Count-1].transform.localScale = new Vector3(Random.Range(-1, 1) == 0 ? 1.5f : -1.5f, 1.5f, 1.5f);
                
                //tavernManager.Hero[tavernManager.Hero.Count-1].GetComponent<UnitMoveToPos>().FrameGO = tavernManager.HeroFrame[tavernManager.HeroList.Hero[index].Place];

                //Для связи ячейки выбора и персонажа
                tavernManager.Hero[tavernManager.Hero.Count-1].GetComponent<UnitMoveToPos>().index = tavernManager.HeroList.Place[index];

                //Меняем текстуру на точке редактирования персонажа
                tavernManager.HeroFrame[tavernManager.HeroList.Place[index]].GetComponent<SpriteRenderer>().sprite = tavernManager.HeroFrameSprite[0];
                
                //tavernManager.Hero[tavernManager.Hero.Count-1].GetComponent<UnitMoveToPos>().tavernGO = tavernManager.gameObject;
            }
        }
        if(tavernManager.Hero.Count<=0)tavernManager.BBattle.SetActive(false);
    }
    
    
    #endregion
}
