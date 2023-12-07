using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

[Serializable]
public class HeroDescriptor
{
	public int id;
	public int place;
	public int level;
	public HeroUnlockState bUnlocked;
}

public enum HeroUnlockState
{
	Locked,
	InLibrary,
	Unlocked
}

[Serializable]
public struct SaveData
{
	public bool bInitialized;
	public int money;
	public int level;
	public int moneyLvlUp;
	public bool offerAvailable;
	public bool bFastMode;
	public int autoPlayCount;
	public bool bAutoPlayActive;
	public DateTime autoPlayLastRestore;
	public int levelsCompleted;
	public bool noAdsBought;
	public int tutorial;

	public List<HeroDescriptor> heroes;

	public override string ToString()
	{
		return JsonUtility.ToJson(this);
	}
}


public static class SaveSystem
{
	private static SaveData save;

	public static void SaveHeroStats(int id, int place, int level, bool bSave)
	{
		int index = save.heroes.FindIndex((x) => x.id == id);
		if (index == -1)
		{
			save.heroes.Add(new HeroDescriptor { id = id, place = place, level = level, bUnlocked = HeroUnlockState.InLibrary });
		}
		else
		{
			save.heroes[index].place = place;
			save.heroes[index].level = level;
		}
		if (bSave)
			ConfirmSave();
	}

	public static void SaveHeroUnlocked(int id, HeroUnlockState bUnlocked, bool bSave)
	{
		int index = save.heroes.FindIndex((x) => x.id == id);
		if (index == -1)
		{
			save.heroes.Add(new HeroDescriptor { id = id, place = -1, level = 1, bUnlocked = bUnlocked });
		}
		else
		{
			save.heroes[index].bUnlocked = bUnlocked;
		}

		if (bSave)
			ConfirmSave();
	}

	public static void SetTutorial(int tut, bool bSave)
	{
		save.tutorial = tut;

		if (bSave)
			ConfirmSave();
	}

	public static int GetTutorial()
	{
		return save.tutorial;
	}

	public static void SetLevelsCompleted(int completed, bool bSave)
	{
		save.levelsCompleted = completed;
		if (bSave)
			ConfirmSave();
	}

	public static int GetLevelsCompleted()
	{
		return save.levelsCompleted;
	}

	public static void SetAutoPlayActive(bool state, bool bSave)
	{
		save.bAutoPlayActive = state;

		if (bSave)
			ConfirmSave();
	}

	public static bool GetAutoPlayActive()
	{
		return save.bAutoPlayActive;
	}

	public static void SetAutoPlayLastRestore(DateTime date, bool bSave)
	{
		save.autoPlayLastRestore = date;
		if (bSave)
			ConfirmSave();
	}

	public static DateTime GetAutoPlayLastRestore()
	{
		return save.autoPlayLastRestore;
	}

	public static void SetAutoPlayCount(int count, bool bSave)
	{
		save.autoPlayCount = count;

		if (bSave)
			ConfirmSave();
	}

	public static int GetAutoPlayCount()
	{
		return save.autoPlayCount;
	}

	public static void SetFastMode(bool mode, bool bSave)
	{
		save.bFastMode = mode;
		if (bSave)
			ConfirmSave();
	}

	public static bool GetFastMode()
	{
		return save.bFastMode;
	}

	public static bool IsOfferAvailable()
	{
		return save.offerAvailable;
	}

	public static void UseOffer(bool bSave)
	{
		save.offerAvailable = false;

		if (bSave)
			ConfirmSave();
	}

	public static void Initialize()
	{
		save.levelsCompleted = 0;
		save.autoPlayLastRestore = DateTime.Now;
		save.autoPlayCount = 0;
		save.bAutoPlayActive = false;
		save.level = 0;
		save.heroes = new List<HeroDescriptor>();
		save.noAdsBought = false;
		save.offerAvailable = true;

		for (int i = 0; i < 10; i++)
		{
			SaveHeroUnlocked(i, HeroUnlockState.Locked, false);
		}

		save.money = 0;

		save.bInitialized = true;

		Debug.Log("Game data initialized at first time");
		ConfirmSave();
		SceneManager.LoadSceneAsync("BattleLearn");
	}

	public static void LoadGame(Action onLoad)
	{
		try
		{
			Debug.Log("Load game");
#if UNITY_EDITOR
			string saveString = PlayerPrefs.GetString("SaveGame");
			save = JsonUtility.FromJson<SaveData>(saveString);
			onLoad?.Invoke();
			return;
#endif

#if UNITY_ANDROID
#elif UNITY_WEBGL
			Debug.Log("Connected yandex load callback");
			YandexGame.GetDataEvent = () =>
			{
				Debug.Log("Loaded data: ");
				Debug.Log(YandexGame.savesData.saveData);
				if (YandexGame.savesData.saveData.bInitialized)
				{
					Debug.Log("Initialized data loaded");
					save = YandexGame.savesData.saveData;
					if (save.noAdsBought)
					{
						YandexGame.StickyAdActivity(false);
					}
					onLoad?.Invoke();
				}
				else
				{
					Debug.Log("Data loaded, but loaded data not marked as inititalized");
					Initialize();
				}
			};
#endif
		}
		catch (Exception e)
		{
			Debug.Log("Error with default game flow");
			Debug.Log(e.Message);
			Initialize();
		}
	}

	public static bool Initialized()
	{
		return save.bInitialized;
	}

	public static int GetMoneyLvlUp()
	{
		return save.moneyLvlUp;
	}

	public static int GetMoney()
	{
		return save.money;
	}

	public static HeroDescriptor GetHero(int id)
	{
		return save.heroes.Find(x => x.id == id);
	}

	public static int GetGameLevel()
	{
		Debug.Log($"Loading level {save.level}");
		return save.level;
	}

	public static void SaveMoney(int money, bool bSave)
	{
		save.money = money;
		if (bSave)
			ConfirmSave();
	}

	public static void SaveMoneyLvlUp(int mlu, bool bSave)
	{
		save.moneyLvlUp = mlu;
		if (bSave)
			ConfirmSave();
	}

	public static void SaveGameLevel(int level, bool bSave)
	{
		Debug.Log($"Saving level {level}");
		save.level = level;
		if (bSave)
			ConfirmSave();
	}

	public static void ConfirmSave()
	{
		Debug.Log("Saving");
#if UNITY_EDITOR
		string saveString = JsonUtility.ToJson(save);
		PlayerPrefs.SetString("SaveGame", saveString);
		PlayerPrefs.Save();
#endif

#if UNITY_ANDROID
#elif UNITY_WEBGL
		YandexGame.savesData.saveData = save;
		Debug.Log("Saved data: ");
		Debug.Log(YandexGame.savesData.saveData);
		YandexGame.SaveProgress();
#endif
	}

	public static void BuyNoAds(bool bSave)
	{
		save.noAdsBought = true;

		YandexGame.StickyAdActivity(false);

		if (bSave)
			ConfirmSave();
	}

	public static bool NoAdsBought()
	{
		return save.noAdsBought;
	}
}
