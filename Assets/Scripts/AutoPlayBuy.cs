using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using YG;

public class AutoPlayBuy : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {
        Purchase samplePurchase = YandexGame.PurchaseByID("ulimited_autoplay");

        if (LocalizationSettings.SelectedLocale.ToString() == "English")
            _text.text = "Buy " +
                "" + samplePurchase.priceValue + " Yan";
        else _text.text = " упить " +
                "" + samplePurchase.priceValue + " ян";
    }
}
