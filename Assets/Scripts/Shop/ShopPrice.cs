using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using YG;

public class ShopPrice : MonoBehaviour
{
	[SerializeField] private inapppurchase inapppurchasedata;
	public levelsReward levelInfo;
	public progress progressInfo;

	[SerializeField] private TextMeshProUGUI goldSlot1text;
	[SerializeField] private TextMeshProUGUI goldSlot2text;
	[SerializeField] private TextMeshProUGUI goldSlot3text;

	[SerializeField] private TextMeshProUGUI goldSlot1Price;
	[SerializeField] private TextMeshProUGUI goldSlot2Price;
	[SerializeField] private TextMeshProUGUI goldSlot3Price;

	// Start is called before the first frame update
	void Awake()
	{
		var level = SaveSystem.GetGameLevel();
		var goldslot1 = ((int)(levelInfo.dataArray[0].Startreward + progressInfo.dataArray[0].Plusvictoryreward * level) * inapppurchasedata.dataArray[0].Goldslot1);
		var goldslot2 = ((int)(levelInfo.dataArray[0].Startreward + progressInfo.dataArray[0].Plusvictoryreward * level) * inapppurchasedata.dataArray[0].Goldslot2);
		var goldslot3 = ((int)(levelInfo.dataArray[0].Startreward + progressInfo.dataArray[0].Plusvictoryreward * level) * inapppurchasedata.dataArray[0].Goldslot3);

		goldSlot1text.text = goldslot1.ToString();
		goldSlot2text.text = goldslot2.ToString();
		goldSlot3text.text = goldslot3.ToString();

		string language;
		if (LocalizationSettings.SelectedLocale.ToString() == "English")
			language = " Yan";
		else language = " ян";

        for (int i = 0; i < YandexGame.PaymentsData.id.Length; i++)
		{
			string id = YandexGame.PaymentsData.id[i];
			switch(id)
			{
				case "gold_1":
					goldSlot1Price.text = YandexGame.PaymentsData.priceValue[i] + language;
					break;

				case "gold_2":
					goldSlot2Price.text = YandexGame.PaymentsData.priceValue[i] + language;
					break;

				case "gold_3":
					goldSlot3Price.text = YandexGame.PaymentsData.priceValue[i] + language;
					break;
			}
		}
	}
}
