using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;

public class PopUp : MonoBehaviour
{

	[NonSerialized] public ADManager ADManager;
	[SerializeField] private GameObject popUpWin;
	[SerializeField] private GameObject popUpDef;
	//[SerializeField] private GameObject popUpBonus;
	[SerializeField] private GameObject titleText;
	[SerializeField] private GameObject imageSlot;
	[SerializeField] private GameObject imageSlotBonus;
	[SerializeField] private GameObject imageSlotReward;
	[SerializeField] private GameObject button;
	[SerializeField] private GameObject buttonBonus;
	[SerializeField] private GameObject buttonCloseBonus;
	[Header("Localization")]
	[SerializeField] private LocalizedString localizedVictory;
	[SerializeField] private LocalizedString localizedDefeat;
	[SerializeField] private LocalizedString localizedBonus;

	public GameObject heroesAddIcon;
	public TextMeshProUGUI textMoney;
	public TextMeshProUGUI textMoneyBonus;
	public TextMeshProUGUI textMoneyReward;

	private int money = 0;
	private int moneyReward = 0;
	private bool winPopUp = false;

	public upgraderules upgraderulesData;
	public GameManager gameManager;
	public levelsReward levelInfo;
	public progress progressInfo;
	public heroesStats heroesInfo;
	private int gameLevel = 0;

	private bool saveB = false;
	private AsyncOperation operation;

	private bool key = false;

	private int haveHero = -1;

	public void ActivateWinPopUp()
	{
		HideAllPopUp();
		winPopUp = true;

		popUpWin.SetActive(true);
		localizedVictory.GetLocalizedStringAsync().Completed += x =>
		{
			titleText.GetComponent<TextMeshProUGUI>().text = x.Result;
		};
		textMoney.text = moneyReward.ToString();
		titleText.SetActive(true);
		imageSlot.SetActive(true);
		button.SetActive(true);
	}

	public void ActivateDefPopUp()
	{
		HideAllPopUp();

		popUpDef.SetActive(true);
		localizedDefeat.GetLocalizedStringAsync().Completed += x =>
		{
			titleText.GetComponent<TextMeshProUGUI>().text = x.Result;
		};

		textMoney.text = ((int)(moneyReward / levelInfo.dataArray[0].Defeatreward)).ToString();
		titleText.SetActive(true);
		imageSlot.SetActive(true);
		heroesAddIcon.SetActive(false);
		button.SetActive(true);
	}

	public void ActivateBonusPopUp()
	{
		HideAllPopUp();

		buttonCloseBonus.SetActive(true);
		localizedBonus.GetLocalizedStringAsync().Completed += x =>
		{
			titleText.GetComponent<TextMeshProUGUI>().text = x.Result;
		};
		textMoneyReward.text = ((int)(moneyReward * levelInfo.dataArray[0].Bonusreward)).ToString();

		titleText.SetActive(true);
		imageSlotBonus.SetActive(true);
		buttonBonus.SetActive(true);
	}

	public void CloseReward()
	{
		AudioLibrarySO.instance.PlayButtonSound();
		var gameLevel = SaveSystem.GetGameLevel();
		HandleUnlockCHaracter();
		if (SceneManager.GetActiveScene().name != "BattleLearn" && winPopUp)
		{
			SaveSystem.SaveGameLevel(gameLevel + 1, false);
		}
		SaveSystem.ConfirmSave();
		gameManager.operation.allowSceneActivation = true;
	}

	public void RewardPlay()
	{
		AudioLibrarySO.instance.PlayButtonSound();

		//Реклама GoogleAd
		ADManager.ShowRewarded(() => { money += moneyReward; RewardedBonus(); });

		//Debug.Log("Reward");
#if UNITY_EDITOR
		//RewardedBonus(null);
#else
        /*IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedBonus;
        
        //Реклама IronSource
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
                
            IronSource.Agent.showRewardedVideo();
        }
        else
        {
            Debug.Log("unity-script: IronSource.Agent.isRewardedVideoAvailable - False");
        }*/
#endif
	}

	public void RewardedBonus()
	{
		imageSlotBonus.SetActive(enabled);
		textMoneyBonus.text = textMoneyReward.text;
		imageSlotReward.SetActive(enabled);

		if (winPopUp) ActivateWinPopUp();
		else ActivateDefPopUp();
	}

	private void HideAllPopUp()
	{
		//winPopUp = false;
		if (winPopUp) popUpDef.SetActive(false);
		else popUpWin.SetActive(false);
		//popUpBonus.SetActive(false);
		titleText.SetActive(false);
		imageSlot.SetActive(false);
		imageSlotBonus.SetActive(false);
		button.SetActive(false);
		buttonBonus.SetActive(false);
		buttonCloseBonus.SetActive(false);
	}

	public void OnButtonLevel()
	{
		AudioLibrarySO.instance.PlayButtonSound();
		if (gameLevel < gameManager.rewardRange - 1)
		{
			var rez = (int)(money + int.Parse(textMoney.text));
			if (SceneManager.GetActiveScene().name != "BattleLearn" && winPopUp)
			{
				HandleUnlockCHaracter();
				SaveSystem.SaveGameLevel(gameLevel + 1, false);
			}
			SaveSystem.SaveMoney(rez, true);
			gameManager.operation.allowSceneActivation = true;
		}
		else
		{
			if (!key)
			{
				key = true;

				var rez = (int)(money + int.Parse(textMoney.text));
				SaveSystem.SaveMoney(rez, true);

				ActivateBonusPopUp();
			}
			else
			{
				var rez = SaveSystem.GetMoney() + int.Parse(textMoneyReward.text);
				if (SceneManager.GetActiveScene().name != "BattleLearn" && winPopUp)
				{
					HandleUnlockCHaracter();
					SaveSystem.SaveGameLevel(gameLevel + 1, false);
				}
				SaveSystem.SaveMoney(rez, true);
				gameManager.operation.allowSceneActivation = true;
			}

		}
	}

	private void HandleUnlockCHaracter()
	{
		if (haveHero != -1)
		{
			Debug.Log($"Character {haveHero} unlocked");
			SaveSystem.SaveHeroUnlocked(haveHero, HeroUnlockState.InLibrary, false);
		}
	}

	private void Start()
	{
		gameLevel = SaveSystem.GetGameLevel();/*
		SaveSystem.SaveGameLevel(gameLevel + 1);*/

		money = SaveSystem.GetMoney();


		if (SceneManager.GetActiveScene().name == "BattleLearn2")
		{
			textMoney.text = (100).ToString();
		}

		if (SceneManager.GetActiveScene().name != "BattleLearn2" && SceneManager.GetActiveScene().name != "BattleLearn")
		{
			var i = heroesInfo.dataArray.FirstOrDefault(s => (s.Unlockonlvl - 1) == gameLevel);
			if (i != null && i.Ind != 9)
			{
				heroesAddIcon.SetActive(true);
				haveHero = i.Ind;
				Debug.Log($"Ability to unlock {haveHero}");
			}
			else
			{
				Debug.Log("Error with processing unlockable cahracter");
			}

			moneyReward = calcUpgradePrice(gameLevel);
		}
	}

	IEnumerator AsyncLoadingTavernTutor()
	{
		operation = SceneManager.LoadSceneAsync("TavernTutor");
		// Предотвращаем автоматическое переключение при завершении загрузки
		operation.allowSceneActivation = false;

		yield return null;
	}

	private int calcUpgradePrice(int cyrLevel)
	{
		int lastIndex = -1;
		int upgradePrice = levelInfo.dataArray[0].Startreward;
		//Debug.Log("Level "+cyrLevel);

		for (int i = 2; i <= (cyrLevel + 1); i++)
		{

			//Перебираем уровни
			// Если уровень не найден в таблице берём последний найденный индекс
			for (int index = 0; index < upgraderulesData.dataArray.Length; index++)
			{
				//Если нашли
				if (i == upgraderulesData.dataArray[index].Lvl)
				{
					//Debug.Log("Совпадение");
					lastIndex = index;
					break;
				}
				//Debug.Log("Индекс");
			}

			if (lastIndex == -1)
			{
				upgradePrice += (int)progressInfo.dataArray[0].Plusvictoryreward;
				//Debug.Log("Обычный"+upgradePrice);
			}
			else
			{
				upgradePrice += (int)Math.Round(progressInfo.dataArray[0].Plusvictoryreward *
												 upgraderulesData.dataArray[lastIndex].Coef);
				//Debug.Log("Коэф"+upgradePrice+" К="+upgraderulesData.dataArray[lastIndex].Coef);
			}


		}

		return upgradePrice;
	}

	void Update()
	{
		if (!saveB && (SceneManager.GetActiveScene().name == "BattleLearn" || SceneManager.GetActiveScene().name == "BattleLearn2"))
		{
			saveB = true;
			StartCoroutine(AsyncLoadingTavernTutor());
			if (SceneManager.GetActiveScene().name == "BattleLearn")
			{
				SaveSystem.SaveHeroStats(4, 3, 1, false);
				SaveSystem.SaveHeroUnlocked(4, HeroUnlockState.Unlocked, true);
			}

			if (SceneManager.GetActiveScene().name == "BattleLearn2")
			{
				SaveSystem.SaveMoney(100, false);
				SaveSystem.SaveGameLevel(gameLevel + 1, false);
				SaveSystem.SetTutorial(1, true);
			}
		}

	}
}

