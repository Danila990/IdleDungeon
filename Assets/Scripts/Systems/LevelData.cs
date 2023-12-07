using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    public GameObject RoomCanvas;
    public GameObject Room;
    //public List<GameObject> EnemiesDirectory => enemiesDirectory;
    //public List<Transform> EnemyPoint => enemyPoint;
    public List<Transform> HeroPoint => heroPoint;
    public List<Transform> BattlePoint => battlePoint;

    //[SerializeField] private List<GameObject> enemiesDirectory;
    //[SerializeField] private List<Transform> enemyPoint;
    [SerializeField] private List<Transform> heroPoint;
    [SerializeField] private List<Transform> battlePoint;
     //private List<GameObject> battlePoint;
     
    /*[Serializable]
    public struct battlePointStruct
    {
        public GameObject battlePoint;
        public List<GameObject> hero;
        public List<GameObject> enemy;
    }*/

    
}
