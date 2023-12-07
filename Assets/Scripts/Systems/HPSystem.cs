using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class HPSystem : MonoBehaviour
{
    
    public float SetHP
    {
        set
        {
            if (hpValue != null)
                if (unitController != null)
                    hpValue.fillAmount = value * 1 / unitController.unitHP;
        }
    }

    [SerializeField] private Image hpValue;
    [SerializeField] private UnitController unitController;
}
