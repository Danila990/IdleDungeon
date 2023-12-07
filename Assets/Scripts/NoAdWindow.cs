using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using YG;

public class NoAdWindow : MonoBehaviour
{
	[SerializeField] private Button closeButton;
	[SerializeField] private Button buyButton;
	[SerializeField] private GameObject objectToHide;
	[SerializeField] private TextMeshProUGUI _priceText;

    private void Awake()
	{
        Purchase samplePurchase = YandexGame.PurchaseByID(GameManager.NO_ADS);

        if (LocalizationSettings.SelectedLocale.ToString() == "English")
            _priceText.text = "Buy " + samplePurchase.priceValue + " Yan";
        else _priceText.text = " упить " + samplePurchase.priceValue + " ян";

        closeButton.onClick.AddListener(() =>
		{
			Close();
		});

		buyButton.onClick.AddListener(() =>
		{
#if UNITY_WEBGL
			YandexGame.postPurchaseEvent = () =>
			{
				Close();
			};
			YandexGame.BuyPayments(GameManager.NO_ADS);
#endif
		});    
    }

	private void OnEnable()
	{
		Time.timeScale = 0f;
	}

	private void Continue()
	{
		Time.timeScale = SaveSystem.GetFastMode() ? 2 : 1;
	}

	private void Close()
	{
		Continue();
		objectToHide.SetActive(false);
	}
}
