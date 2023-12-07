using Spine.Unity;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Scriptable Object/Scriptable Unit")]
public class Unit : ScriptableObject
{
    #region Публичные Данные

    public enum UnitType
    {
        Hero,
        Enemy
    }

    public enum CombatType
    {
        Melee,
        Range
    }

    #endregion Публичные данные

    #region Публичные Характеристики

    public CombatType Combat => combat;

    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    public float MaxHP
    {
        get => maxHP;
        set => maxHP = value;
    }

    public float AtkPower
    {
        get => atkPower;
        set => atkPower = value;
    }

    /*public float AtkSpeed
    {
        get => atkSpeed;
        set => atkSpeed = value;
    }*/

    //public string Text => text;

    #endregion Публичные Характеристики

    #region Сериализованные поля

    [Header("Combat type")] 
    [SerializeField] private CombatType combat;

    [Header("Basic stats")] 
    [SerializeField] private float speed;
    [SerializeField] private float maxHP;
    [SerializeField] private float atkPower;
    //[SerializeField] private float atkSpeed;
    
    /*[Header("Unit description")] [TextArea] [SerializeField]
    private string text;*/

    #endregion Сериализованные поля
}
