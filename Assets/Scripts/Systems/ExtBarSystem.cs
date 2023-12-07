using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtBarSystem : MonoBehaviour
{
    [SerializeField] private UnitController unitController;
    
    public float SetBarValue
    {
        set=> hpValue.fillAmount = value*1/unitController.UnitAbilityTime;
    }

    [SerializeField] private Image hpValue;
}
