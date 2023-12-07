using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataBase", menuName = "Scriptable Object/Hero Base")]
public class HeroBase : ScriptableObject
{
    public List<GameObject> HeroList
    {
        get => heroUnit;
        set => heroUnit = value;
    }

    [SerializeField] private List<GameObject> heroUnit;
}
