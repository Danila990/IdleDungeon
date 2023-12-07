using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using YG;

public class StartGame : MonoBehaviour
{
	[SerializeField] private AudioLibrarySO audioLibrary;
	private bool nextStep = false;

	void Awake()
	{
        AudioLibrarySO.instance = audioLibrary;
	}

	// Start is called before the first frame update
	void Start()
	{
		Debug.Log("Game start");
#if UNITY_WEBGL
		YandexGame.PurchaseSuccessEvent = OnPurchaseSuccess;
		YandexGame.PurchaseFailedEvent = OnPurchaseFailed;

		if (LocalizationSettings.InitializationOperation.IsDone)
		{
			Debug.Log($"Start language set to {YandexGame.EnvironmentData.language} after localization initialized");
			LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(YandexGame.savesData.language);
		}
		else
		{
			LocalizationSettings.InitializationOperation.Completed += x =>
			{
				Debug.Log($"Start language set to {YandexGame.EnvironmentData.language} before localization initialized");
				LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(YandexGame.savesData.language);
			};
		}
#endif
        LoadGame();
    }

	void LoadGame()
	{
		SaveSystem.LoadGame(OnGameLoad);
	}

	private void OnGameLoad()
	{
		Debug.Log("Game successfully loaded");

		if (SaveSystem.GetHero(4).bUnlocked > HeroUnlockState.Locked)
		{
			if (SaveSystem.GetHero(5).level >= 2)
			{
				SceneManager.LoadSceneAsync("Tavern");
			}
			else
			{
				SceneManager.LoadSceneAsync("TavernTutor");
			}
		}
		else
		{
			SceneManager.LoadSceneAsync("TavernTutor");
		}
	}

	private void OnPurchaseSuccess(string obj)
	{
		switch (obj)
		{
			case GameManager.NO_ADS:
				SaveSystem.BuyNoAds(true);
				break;
		}
		YandexGame.postPurchaseEvent?.Invoke();
		YandexGame.postPurchaseEvent = null;
	}

	private void OnPurchaseFailed(string obj)
	{
		Debug.Log($"Failed to buy payment {obj}");
	}
}
