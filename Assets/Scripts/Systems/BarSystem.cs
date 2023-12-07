using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarSystem : MonoBehaviour
{
    [SerializeField] private UnitController unitController;
    
    public float SetBarValue
    {
        set=> hpValue.fillAmount = value*1/(unitController.CompareTag("Enemy")?unitController.UnitFreazeTime:unitController.UnitDefTime);
    }

    [SerializeField] private Image hpValue;
    [SerializeField] private Image abilityTimeValue;
}
