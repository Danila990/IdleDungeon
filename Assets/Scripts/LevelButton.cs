using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
	public upgraderules upgraderulesData;
	public string nameLevel;
	public GameManager gameManager;
	public TutorialGameManager tutorialManager;
	public levelsReward levelInfo;
	public progress progressInfo;
	public heroesStats heroesInfo;
	public GameObject heroesAddIcon;
	public TextMeshProUGUI textMoney;
	public TextMeshProUGUI textMoneyReward;
	public TextMeshProUGUI textMoneyDefeat;
	public TextMeshProUGUI textMoneyDefeatReward;
	private int gameLevel = 0;
	private int Money = 0;
	private bool initB = false;
	private bool saveB = false;
	private AsyncOperation operation;
	public int x2 = 1;

	private bool key = false;

	public void OnButton()
	{
		gameManager.operation.allowSceneActivation = true;
	}
	public void OnButtonLevel()
	{
		AudioLibrarySO.instance.PlayButtonSound();

		if (gameLevel < gameManager.rewardRange - 1)
		{
			var rez = (int)(Money + int.Parse(textMoney.text));
			if (SceneManager.GetActiveScene().name != "BattleLearn")
			{
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

				var rez = (int)(Money + int.Parse(textMoney.text));
				SaveSystem.SaveMoney(rez, true);
			}
			else
			{
				var rez = SaveSystem.GetMoney() + int.Parse(textMoneyReward.text);
				if (SceneManager.GetActiveScene().name != "BattleLearn")
				{
					SaveSystem.SaveGameLevel(gameLevel + 1, false);
				}
				SaveSystem.SaveMoney(rez, true);
				gameManager.operation.allowSceneActivation = true;
			}

		}
	}

	public void OnButtonTutor()
	{
		AudioLibrarySO.instance.PlayButtonSound();

		SaveSystem.SaveHeroStats(5, 3, 1, false);
		SaveSystem.SaveHeroUnlocked(5, HeroUnlockState.Unlocked, true);

		operation.allowSceneActivation = true;
	}

	public void OnButtonMoneyTutorial(int number)
	{
		AudioLibrarySO.instance.PlayButtonSound();
		operation.allowSceneActivation = true;
	}


	public void OnButtonMoney(int number)
	{
		if (gameLevel < gameManager.rewardRange - 1)
		{
			var rez = (int)(Money + int.Parse(textMoneyDefeat.text));
			if (SceneManager.GetActiveScene().name != "BattleLearn")
			{
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

				var rez = (int)(Money + int.Parse(textMoneyDefeat.text));
				SaveSystem.SaveMoney(rez, true);
			}
			else
			{
				var rez = SaveSystem.GetMoney() + int.Parse(textMoneyReward.text);
				if (SceneManager.GetActiveScene().name != "BattleLearn")
				{
					SaveSystem.SaveGameLevel(gameLevel + 1, false);
				}
				SaveSystem.SaveMoney(rez, true);
				gameManager.operation.allowSceneActivation = true;
			}

		}

	}

	public void OnButtonHero()
	{
		operation.allowSceneActivation = true;
	}

	private void Start()
	{
		gameLevel = SaveSystem.GetGameLevel();
		//Debug.Log("Game="+gameLevel);
		Money = SaveSystem.GetMoney();



		if (!initB)
		{
			initB = true;
			if (SceneManager.GetActiveScene().name == "BattleLearn2") textMoney.text = (100).ToString();
			if (SceneManager.GetActiveScene().name != "BattleLearn2" &&
				SceneManager.GetActiveScene().name != "BattleLearn")
			{
				textMoney.text = ((int)calcUpgradePrice(gameLevel)).ToString();
				//((int)(levelInfo.dataArray[0].Startreward + progressInfo.dataArray[0].Plusvictoryreward * gameLevel)).ToString();
				textMoneyReward.text =
					((int)(int.Parse(textMoney.text) * levelInfo.dataArray[0].Bonusreward)).ToString();
			}

			if (SceneManager.GetActiveScene().name != "BattleLearn2" &&
				SceneManager.GetActiveScene().name != "BattleLearn")
			{
				var i = heroesInfo.dataArray.FirstOrDefault(s => s.Unlockonlvl == gameLevel);
				if (i != null) heroesAddIcon.SetActive(true);
				{
					textMoneyDefeat.text = ((int)(calcUpgradePrice(gameLevel) / levelInfo.dataArray[0].Defeatreward))
						.ToString();
					//((int)(( (levelInfo.dataArray[0].Startreward + progressInfo.dataArray[0].Plusvictoryreward * gameLevel) / levelInfo.dataArray[0].Defeatreward))).ToString();
					textMoneyDefeatReward.text =
						((int)(int.Parse(textMoney.text) * levelInfo.dataArray[0].Bonusreward)).ToString();
					((int)(int.Parse(textMoneyDefeat.text) * levelInfo.dataArray[0].Bonusreward)).ToString();

				}
			}
		}
	}

	IEnumerator AsyncLoadingTavernTutor()
	{
		operation = SceneManager.LoadSceneAsync("TavernTutor");
		// Предотвращаем автоматическое переключение при завершении загрузки
		operation.allowSceneActivation = false;

		yield return null;
	}

	/*IEnumerator AsyncLoadingTavern()
    {
        operation = SceneManager.LoadSceneAsync("Tavern");
        // Предотвращаем автоматическое переключение при завершении загрузки
        operation.allowSceneActivation = false;
 
        yield return null;
    }*/
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
				SaveSystem.SaveHeroUnlocked(4, HeroUnlockState.Unlocked, true);
			}

			if (SceneManager.GetActiveScene().name == "BattleLearn2")
			{
				SaveSystem.SaveMoney(100, false);
				SaveSystem.SaveGameLevel(gameLevel + 1, false);
				SaveSystem.SetTutorial(1, false);
			}
		}
	}
}
