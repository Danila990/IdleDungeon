using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class HPSystemTutorial : MonoBehaviour
{
    
    public float SetHP
    {
        set=> hpValue.fillAmount = value*1/unitControllerTutorial.unitHP;
    }

    [SerializeField] private Image hpValue;
    [SerializeField] private UnitControllerTutorial unitControllerTutorial;
}

