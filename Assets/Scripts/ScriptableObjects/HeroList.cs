using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataBase", menuName = "Scriptable Object/ListHero Base")]
public class HeroList : ScriptableObject
{
   /* [Serializable]
    public struct HeroStruct
    {
        public GameObject Unit;
        public GameObject UnitPrefab;
        public int Place;
    }

    public List<HeroStruct> Hero;
*/
    [Serializable]
    public struct HeroStruct
    {
        public String HeroName;
        public Texture Avatar;
        public Sprite HeroSkill;
        public String SkillDescription;
        public int HeroHP;
        public int HeroDamage;
        public HeroAttackSpeed heroAttackSpeed;
        public int HeroSkillRestart;
        public int HeroCrit;
    }

    public enum HeroAttackSpeed
    {
        Низкая,
        Средняя,
        Высокая
    }
    
    public List<HeroStruct> Hero;
    
    public List<GameObject> Unit
    {
        get => unit;
        set => unit = value;
    }

    public List<GameObject> UnitPrefab
    {
        get => unitPrefab;
        set => unitPrefab = value;
    }

    public List<int> Place
    {
        get => place;
        set => place = value;
    }
    public List<int> Level
    {
        get => level;
        set => level = value;
    }
    public int Money
    {
        get => money;
        set => money = value;
    }


    [SerializeField] private List<GameObject> unit;
    [SerializeField] private List<GameObject> unitPrefab;
    [SerializeField] private List<int> place;
    [SerializeField] private List<int> level;
    [SerializeField] private int money;
}
