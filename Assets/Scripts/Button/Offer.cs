using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using YG;

public class Offer : MonoBehaviour
{
	[SerializeField] private TavernManager tavernManager;
	[SerializeField] private GameObject frameOffer;
	[SerializeField] private TMP_Text priceText;
	[SerializeField] private int levelActivateOffer;

	// Start is called before the first frame update
	void Start()
	{
		var gameLevel = SaveSystem.GetGameLevel();

		frameOffer.SetActive(SaveSystem.IsOfferAvailable());

		string price = "200";
#if UNITY_WEBGL
        price = YandexGame.PurchaseByID(GameManager.EXCLUSIVE).priceValue;
#endif

        if (LocalizationSettings.SelectedLocale.ToString() == "English")
            priceText.text = "Buy " + price + " Yan";
        else priceText.text = " упить " + price + " ян";
	}


	public void buttonOfferOn()
	{
		AudioLibrarySO.instance.PlayButtonSound();

#if UNITY_WEBGL

		if(YandexGame.auth)
		{
			YandexGame.postPurchaseEvent = OfferReward;
			YandexGame.BuyPayments(GameManager.EXCLUSIVE);
		}
		else
		{
			YandexGame.AuthDialog();
		}

#endif
	}

	private void OfferReward()
	{
		SaveSystem.SaveHeroStats(9, -1, 1, false);
		SaveSystem.UseOffer(true);
		tavernManager.updateFrameData();
		frameOffer.SetActive(false);
	}

	public void closeOffer()
	{
		AudioLibrarySO.instance.PlayButtonSound();
		frameOffer.SetActive(false);
	}

}
