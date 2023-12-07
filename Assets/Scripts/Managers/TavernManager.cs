using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;

using UnityEngine.UI;
using YG;
using Object = UnityEngine.Object;

//using static UnityEngine.Object;


public class TavernManager : MonoBehaviour
{
	[SerializeField] private upgraderules upgraderulesData;
	[SerializeField] private inapppurchase inapppurchasedata;
	public levelsReward levelInfo;
	public progress progressInfo;

	public TextMeshProUGUI tempLVLTextUpdate;
	private int moneyLvlUp = 0;
	public TextMeshProUGUI tempLVLText;
	public LocalizedString levelLocalized;
	public delegate void ChangePosition();
	public static event ChangePosition gotoStartPoint;
	public static event ChangePosition gotoFramePoint;
	public ADManager adManager;


	public List<GameObject> HeroFrame
	{
		get => heroFrame;
		set => heroFrame = value;
	}
	public List<GameObject> HeroUnitsPoint
	{
		get => heroUnitsPoint;
		set => heroUnitsPoint = value;
	}
	public List<GameObject> HeroUnitsFrame
	{
		get => heroUnitsFrame;
		set => heroUnitsFrame = value;
	}
	public Sprite[] HeroFrameSprite
	{
		get => heroFrameSprite;
		set => heroFrameSprite = value;
	}
	public HeroList HeroList
	{
		get => heroList;
		set => heroList = value;
	}
	public List<GameObject> Hero
	{
		get => hero;
		set => hero = value;
	}
	public GameObject BBattle
	{
		get => bBattle;
		set => bBattle = value;
	}
	public GameObject BHero
	{
		get => bHero;
		set => bHero = value;
	}
	public GameObject HeroFrameList
	{
		get => heroFrameList;
		set => heroFrameList = value;
	}
	public GameObject HeroFrameInfoAdd
	{
		get => heroFrameInfoAdd;
		set => heroFrameInfoAdd = value;
	}
	public GameObject HeroFrameInfoRemove
	{
		get => heroFrameInfoRemove;
		set => heroFrameInfoRemove = value;
	}



	private GameFSM tavernFSM;
	private int cyrIndexCheck = -1;
	private int cyrIndexHero = -1;
	private int cyrIndexHeroUpdate = -1;
	private string cyrNameHero;

	public enum State
	{
		Initialize,
		Wait,
		LoadData
	}

	[SerializeField] private GameObject bBattle;
	[SerializeField] private State tavernState;
	[SerializeField] private GameObject bHero;
	[SerializeField] private GameObject heroFrameList;
	[SerializeField] private GameObject heroFrameInfo;
	[SerializeField] private GameObject heroFrameInfoAdd;
	[SerializeField] private GameObject heroFrameInfoRemove;
	[SerializeField] private Sprite[] heroFrameSprite;
	[SerializeField] private HeroList heroList;
	[SerializeField] List<GameObject> hero;
	[SerializeField] private List<GameObject> heroUnitsPoint;
	[SerializeField] private List<GameObject> heroUnitsFrame;
	[SerializeField] private List<GameObject> heroFrame;
	[SerializeField] private List<GameObject> heroListFrame;
	//[SerializeField] private int numAddHero;
	[SerializeField] private TextMeshProUGUI money;
	[SerializeField] private TextMeshProUGUI crystal;
	[SerializeField] private List<TextMeshProUGUI> heroLevelListFrame;
	[SerializeField] private TextMeshProUGUI heroLevelAddFrame;

	[SerializeField] private GameObject shop;
	private int gameLevel = 0;
	[SerializeField] private heroesStats heroesData;
	[SerializeField] private progress progressData;


	public void updateFrameData() => SetFrameData();

	public void DelHeroName()
	{
		//Удаляем героя выбранного из списка
		GameObject cyrHero = hero.Find(rez => rez.GetComponent<UnitMoveToPos>().IndexBase == cyrIndexHeroUpdate);

		if (cyrHero != null)
		{
			int ind1 = hero.IndexOf(cyrHero);
			//Debug.Log("Hero seach index=" + ind1);
			var go1 = hero[ind1];

			heroFrame[cyrHero.GetComponent<UnitMoveToPos>().index].GetComponent<SpriteRenderer>().sprite = heroFrameSprite[1];

			heroFrame[cyrHero.GetComponent<UnitMoveToPos>().index].GetComponent<OnFrame>().group.SetActive(false);

			hero.Remove(hero[ind1]);
			Destroy(go1);

		}

	}

	public void DelHero()
	{
		AudioLibrarySO.instance.PlayButtonSound();
		var heroStruct = heroList.Place.Find(rez => rez == cyrIndexCheck);
		int ind = heroList.Place.IndexOf(heroStruct);

		heroList.Place[ind] = cyrIndexCheck;


		heroList.Place[ind] = -1;

		//Разблокируем героя для добавления
		heroFrameList.GetComponent<HeroFrameListInfo>().FrameAndButton[ind].Frame.SetActive(false);

		ind = hero.IndexOf(hero.Find(rez => rez.GetComponent<UnitMoveToPos>().index == cyrIndexCheck));
		var go = hero[ind];
		hero.Remove(hero[ind]);
		Destroy(go);


		if (heroFrameInfoAdd.activeSelf)
		{
			UpdateHeroData();
			heroFrame[cyrIndexCheck].GetComponent<OnFrame>().group.SetActive(true);
			AddHero();
		}
		else
		{
			heroFrame[cyrIndexCheck].GetComponent<SpriteRenderer>().sprite = heroFrameSprite[1];

			heroFrame[cyrIndexCheck].GetComponent<OnFrame>().group.SetActive(false);

			cyrIndexCheck = -2;
			SaveData();
			RotateFrame(false);
			RotateListFrame(false);

			heroFrameInfoAdd.SetActive(true);
			heroFrameInfoRemove.SetActive(false);
		}
	}

	public void AddHero()
	{
		string name = "";
		// Если ячейка не была выбрана
		if (cyrIndexCheck < 0)
		{
			// Трясём всех
			foreach (var cyr in heroFrame)
			{
				cyr.GetComponent<OnFrame>().RotateFrame = true;
			}
		}
		else
		{
			switch (cyrIndexHeroUpdate)
			{
				case 0:
					name = "IceBase";
					break;
				case 1:
					name = "KnightBase";
					break;
				case 2:
					name = "ShamanBase";
					break;
				case 3:
					name = "AsterixBase";
					break;
				case 4:
					name = "SimpleFemaleBase";
					break;
				case 5:
					name = "SimpleMaleBase";
					break;
				case 6:
					name = "SpearManBase";
					break;
				case 7:
					name = "VarvarBase";
					break;
				case 8:
					name = "VikingBase";
					break;
				case 9:
					name = "FishmanBase";
					break;
				default: break;
			}


			var heroStruct = heroList.Unit.Find(rez => rez.name == name);
			int ind = heroList.Unit.IndexOf(heroStruct);
			heroList.Place[ind] = cyrIndexCheck;
			DelHeroName();

			hero.Add(Object.Instantiate(heroList.Unit[ind], heroUnitsFrame[heroList.Place[ind]].transform.position,
				Quaternion.identity));


			//Блокируем повторное добавление
			heroFrameList.GetComponent<HeroFrameListInfo>().FrameAndButton[ind].Frame.SetActive(true);

			//Назначаем точки расстановки
			hero[hero.Count - 1].GetComponent<UnitMoveToPos>().StartPoint =
				heroUnitsPoint[heroList.Place[ind]].transform;
			hero[hero.Count - 1].GetComponent<UnitMoveToPos>().PointFrame =
				heroUnitsFrame[heroList.Place[ind]].transform;
			//Разворачиваем мордочкой в случайную сторону
			hero[hero.Count - 1].transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
			hero[hero.Count - 1].GetComponent<UnitMoveToPos>().index = cyrIndexCheck;

			heroFrame[cyrIndexCheck].GetComponent<SpriteRenderer>().sprite = heroFrameSprite[0];
			UpdateHeroData();
			heroFrame[cyrIndexCheck].GetComponent<OnFrame>().group.SetActive(true);

			SaveData();
			RotateFrame(false);
			//все фреймы в листе должны трястись
			foreach (var cyr in heroListFrame)
			{
				cyr.GetComponent<RotateFrameList>().RotateFrame = false;
			}

			RotateListFrame(false);


			heroFrameInfoAdd.SetActive(false);
			heroFrameInfoRemove.SetActive(true);
		}
	}

	public void HeroLevelUp()
	{
		AudioLibrarySO.instance.PlayButtonUpgradeSound();
		if (heroList.Level[cyrIndexHeroUpdate] < 100)
		{
			if (heroList.Money >= Math.Round(heroesData.dataArray.First(s => s.Id == heroList.Hero[cyrIndexHeroUpdate].HeroName).Upgradestartprice + (heroList.Level[cyrIndexHeroUpdate] - 1) * progressData.dataArray[0].Plusupgradeprice))
			{
				moneyLvlUp += (int)Math.Round(heroesData.dataArray.First(s => s.Id == heroList.Hero[cyrIndexHeroUpdate].HeroName).Upgradestartprice + (heroList.Level[cyrIndexHeroUpdate] - 1) * progressData.dataArray[0].Plusupgradeprice);
				//moneyLvlUp+=price;
				if (SceneManager.GetActiveScene().name != "TavernTutor") tempLVLTextUpdate.text = moneyLvlUp.ToString();

				heroList.Money -= (int)Math.Round(heroesData.dataArray.First(s => s.Id == heroList.Hero[cyrIndexHeroUpdate].HeroName).Upgradestartprice + (heroList.Level[cyrIndexHeroUpdate] - 1) * progressData.dataArray[0].Plusupgradeprice);
				//heroList.Money -= price;
				heroList.Level[cyrIndexHeroUpdate] += 1;

				UpdateHeroData();
				UpdateHeroDataInfo();

				SaveData();
			}
		}

	}

	public void CloseShop()
	{
		AudioLibrarySO.instance.PlayButtonSound();
		foreach (var cyr in Hero)
		{
			cyr.GetComponent<MeshRenderer>().enabled = true;
		}

		shop.SetActive(false);
	}

	public void OpenShop()
	{
		AudioLibrarySO.instance.PlayButtonSound();
		foreach (var cyr in Hero)
		{
			cyr.GetComponent<MeshRenderer>().enabled = false;
		}

		ButtonOff();
		shop.SetActive(true);
	}

	public void BuyGold(int num)
	{
		AudioLibrarySO.instance.PlayButtonSound();
		var level = SaveSystem.GetGameLevel();

#if UNITY_WEBGL
		if(YandexGame.auth)
		{
			YandexGame.postPurchaseEvent = () =>
			{
				HandleGoldBouhgt(num, level);
			};
			YandexGame.BuyPayments("gold_" + num);
		}
		else
		{
			YandexGame.AuthDialog();
		}
#else
        HandleGoldBouhgt(num, level);
#endif
	}

	private void HandleGoldBouhgt(int num, int level)
	{
		switch (num)
		{
			case 1:
				heroList.Money += ((int)(levelInfo.dataArray[0].Startreward + progressInfo.dataArray[0].Plusvictoryreward * level) * inapppurchasedata.dataArray[0].Goldslot1);
				break;
			case 2:
				heroList.Money += ((int)(levelInfo.dataArray[0].Startreward + progressInfo.dataArray[0].Plusvictoryreward * level) * inapppurchasedata.dataArray[0].Goldslot2);
				break;
			case 3:
				heroList.Money += ((int)(levelInfo.dataArray[0].Startreward + progressInfo.dataArray[0].Plusvictoryreward * level) * inapppurchasedata.dataArray[0].Goldslot3);
				break;
		}

		money.text = heroList.Money.ToString();
		SaveSystem.SaveMoney(heroList.Money, true);
	}


	private void SaveData()
	{
		SaveSystem.SaveMoney(heroList.Money, false);
		SaveSystem.SaveMoneyLvlUp(moneyLvlUp, false);
		SaveSystem.SaveGameLevel(gameLevel, false);

		for (int i = 0; i < heroList.Place.Count; i++)
		{
			SaveSystem.SaveHeroStats(i, heroList.Place[i], heroList.Level[i], false);
		}
		SaveSystem.ConfirmSave();
	}

	private void LoadData()
	{
		moneyLvlUp = SaveSystem.GetMoneyLvlUp();

		if (SceneManager.GetActiveScene().name != "TavernTutor")
		{
			tempLVLTextUpdate.text = moneyLvlUp.ToString();
		}

		heroList.Money = SaveSystem.GetMoney();

		for (int i = 0; i < heroList.Place.Count; i++)
		{
			var hero = SaveSystem.GetHero(i);
			heroList.Place[i] = hero.place;
			heroList.Level[i] = hero.level;
		}

		gameLevel = SaveSystem.GetGameLevel();

		levelLocalized.GetLocalizedStringAsync().Completed += x =>
		{
			tempLVLText.text = $"{x.Result}: {gameLevel + 1}";
		};


		money.text = heroList.Money.ToString();
		SetFrameData();
	}

	private void SetFrameData()
	{
		if (SceneManager.GetActiveScene().name != "TavernTutor")
		{
			for (int index = 0; index < heroesData.dataArray.Length; index++)
			{
				if (heroesData.dataArray[index].Ind == 9)
				{
					heroListFrame[heroesData.dataArray[index].Ind].transform.parent.gameObject.SetActive(!SaveSystem.IsOfferAvailable());
				}
				else
				{/*
					if ((heroesData.dataArray[index].Unlockonlvl - 1) < gameLevel)*/
					var heroData = heroesData.dataArray[index];
					if (SaveSystem.GetHero(heroData.Ind).bUnlocked > HeroUnlockState.Locked)
					{
						heroListFrame[heroData.Ind].transform.parent.gameObject.SetActive(true);
					}
				}
			}
		}
		else
		{
			if (SceneManager.GetActiveScene().name != "TavernTutor")
				for (int index = 0; index < heroesData.dataArray.Length; index++)
				{
					heroListFrame[heroesData.dataArray[index].Ind].transform.parent.gameObject.SetActive(true);
				}
		}
	}


	private void RotateFrame(bool active)
	{
		foreach (var cyr in heroFrame)
		{
			cyr.GetComponent<OnFrame>().RotateFrame = active;
		}
	}

	private void RotateListFrame(bool active)
	{
		foreach (var cyr in heroListFrame)
		{
			cyr.GetComponent<RotateFrameList>().RotateFrame = active;
		}
	}

	// Мордочка героя в списке
	public void OnFrameButton(int index)
	{
		AudioLibrarySO.instance.PlayCharacterChooseSound();
		var hero = SaveSystem.GetHero(index);

		if (hero.bUnlocked == HeroUnlockState.InLibrary)
		{
			adManager.ShowRewarded(() => { SaveSystem.SaveHeroUnlocked(index, HeroUnlockState.Unlocked, true); Time.timeScale = 1f; });
			return;
		}

		if (cyrIndexCheck < 0)
		{
			cyrIndexHero = index;

			cyrIndexHeroUpdate = index;

			if (!heroFrameList.GetComponent<HeroFrameListInfo>().FrameAndButton[index].Frame.activeSelf)
			{
				UpdateHeroDataInfo();
				heroFrameInfo.SetActive(true);
				heroFrameInfoAdd.SetActive(true);
				heroFrameInfoRemove.SetActive(false);
			}
			else
			{
				// Вносим данные
				cyrIndexCheck = heroList.Place[index];

				UpdateHeroDataInfo();
				heroFrameInfo.SetActive(true);
				heroFrameInfoRemove.SetActive(true);
				heroFrameInfoAdd.SetActive(false);
			}

			heroFrameList.SetActive(false);
		}
		else
		{
			cyrIndexHero = index;
			cyrIndexHeroUpdate = index;
			UpdateHeroDataInfo();
			heroFrameList.SetActive(false);
			heroFrameInfoRemove.SetActive(true);
			heroFrameInfo.SetActive(true);
			AddHero();
		}
	}

	// Нажали на рамку в таверне
	public void OnFrame(int index)
	{
		//Debug.Log("Нажали рамку");
		cyrIndexCheck = index;
		foreach (var cyr in heroListFrame)
		{
			if (cyr.GetComponent<RotateFrameList>().RotateFrame)
			{
				foreach (var cyr1 in heroFrame)
				{
					cyr1.GetComponent<OnFrame>().RotateFrame = false;
				}
				break;
			}
		}

		bool check = false;
		foreach (var cyr in hero)
		{
			// Если индекс героя совпадает с индексом нажатой ячейки значит ячейка занята
			if (cyr.GetComponent<UnitMoveToPos>().index == index)
			{

				check = true;
				if (heroFrameInfoAdd.activeSelf && heroFrame[0].GetComponent<OnFrame>().RotateFrame)
				{

				}
				else
				{

					RotateFrame(false);
					heroFrameList.SetActive(false);
					heroFrameInfoAdd.SetActive(false);
					cyrIndexHeroUpdate = cyr.GetComponent<UnitMoveToPos>().IndexBase;
					UpdateHeroDataInfo();
					heroFrameInfoRemove.SetActive(true);
					heroFrameInfo.SetActive(true);
				}
			}
		}

		if (!check)
		{
			// Добавляем героя сразу если открыто окно информации
			if (heroFrameInfoAdd.activeSelf) AddHero();
			else
			{
				// Ячейка не занята героем
				heroFrame[index].GetComponent<OnFrame>().RotateFrame = true;
				//все фреймы в листе должны трястись
				RotateListFrame(true);
				heroFrameList.SetActive(true);
				heroFrameInfo.SetActive(false);
				heroFrameInfoAdd.SetActive(false);
				heroFrameInfoRemove.SetActive(false);
			}
		}
		else
		{
			if (heroFrameInfoAdd.activeSelf && heroFrame[0].GetComponent<OnFrame>().RotateFrame)
			{
				DelHero();
			}
		}
		// Если ячейка пустая вызываем список героев
	}

	private float fixedDeltaTime;

	void Awake()
	{
		fixedDeltaTime = Time.fixedDeltaTime;
	}

	// Start is called before the first frame update
	void Start()
	{
		AudioLibrarySO.instance.PlayMenuMusic();
		Time.timeScale = 1f;
		//StartCoroutine(LoadData());
		LoadData();
		tavernFSM = new GameFSM();
		tavernFSM.Initialize(new tavernInitialize(this));
		SetState(State.Wait);
	}

	void placeFrameActivate(bool active)
	{
		foreach (var cyr in heroFrame)
		{
			cyr.SetActive(active);
		}
	}

	//IEnumerator UpdateHeroData()
	void UpdateHeroData()
	{


		// Включаем значок апгрейда в рамках на полу
		for (int index = 0; index < heroList.Place.Count; index++)
		{
			if (heroList.Place[index] > -1)
			{
				//var price = calcUpgradePrice((int) progressData.dataArray[0].Plusupgradeprice, (heroList.Level[index]), index);
				HeroFrame[heroList.Place[index]].GetComponent<OnFrame>().group.SetActive(true);
				HeroFrame[heroList.Place[index]].GetComponent<OnFrame>().level.text = heroList.Level[index].ToString();
				if (heroList.Money >= Math.Round(heroesData.dataArray.First(s => s.Id == heroList.Hero[index].HeroName).Upgradestartprice + (heroList.Level[index] - 1) * progressData.dataArray[0].Plusupgradeprice))
					//if(heroList.Money >= price)
					HeroFrame[heroList.Place[index]].GetComponent<OnFrame>().arrow.SetActive(true);
				else HeroFrame[heroList.Place[index]].GetComponent<OnFrame>().arrow.SetActive(false);
			}
			//yield return null;
		}

		// Включаем значок апгрейда в списке героев
		var h = heroFrameList.GetComponent<HeroFrameListInfo>().LevelUpList;

		for (int index = 0; index < heroLevelListFrame.Count; index++)
		{
			//var price = calcUpgradePrice((int) progressData.dataArray[0].Plusupgradeprice, (heroList.Level[index]-1), index);
			heroLevelListFrame[index].text = heroList.Level[index].ToString();

			if (heroList.Money >= Math.Round(heroesData.dataArray.First(s => s.Id == heroList.Hero[index].HeroName).Upgradestartprice + (heroList.Level[index] - 1) * progressData.dataArray[0].Plusupgradeprice))
				//if(heroList.Money >= price)
				h[index].SetActive(true);
			else h[index].SetActive(false);
			//yield return null;
		}



		money.text = heroList.Money.ToString();
		//yield return null;
	}

	// Тут меняем карточку героя
	//IEnumerator UpdateHeroDataInfo()
	private void UpdateHeroDataInfo()
	{
		//var price = calcUpgradePrice((int) progressData.dataArray[0].Plusupgradeprice, (heroList.Level[cyrIndexHeroUpdate]), cyrIndexHeroUpdate);

		money.text = heroList.Money.ToString();
		//if (heroList.Money >= price)
		if (heroList.Money >= (Math.Round(heroesData.dataArray.First(s => s.Id == heroList.Hero[cyrIndexHeroUpdate].HeroName).Upgradestartprice + (heroList.Level[cyrIndexHeroUpdate] - 1) * progressData.dataArray[0].Plusupgradeprice)))
		{
			heroFrameInfo.GetComponent<HeroInfo>().LevelUP.SetActive(true);
			heroFrameInfo.GetComponent<HeroInfo>().BLevelUP.GetComponent<Image>().enabled = false;
			//yield return null;
		}
		else
		{
			heroFrameInfo.GetComponent<HeroInfo>().LevelUP.SetActive(false);
			heroFrameInfo.GetComponent<HeroInfo>().BLevelUP.GetComponent<Image>().enabled = true;
			//yield return null;
		}

		heroLevelAddFrame.text = heroList.Level[cyrIndexHeroUpdate].ToString();
		heroFrameInfo.GetComponent<HeroInfo>().LevelUpTax.text = (Math.Round(heroesData.dataArray.First(s => s.Id == heroList.Hero[cyrIndexHeroUpdate].HeroName).Upgradestartprice + (heroList.Level[cyrIndexHeroUpdate] - 1) * progressData.dataArray[0].Plusupgradeprice)).ToString();
		//heroFrameInfo.GetComponent<HeroInfo>().LevelUpTax.text=price.ToString();
		heroFrameInfo.GetComponent<HeroInfo>().Avatar.GetComponent<RawImage>().texture =
			heroList.Hero[cyrIndexHeroUpdate].Avatar;

		LocalizedString characterName = new LocalizedString("Titles", heroList.Hero[cyrIndexHeroUpdate].HeroName);
		characterName.GetLocalizedStringAsync().Completed += x =>
		{
			heroFrameInfo.GetComponent<HeroInfo>().HeroName.GetComponent<TextMeshProUGUI>().text = x.Result;
		};
		/*
				heroFrameInfo.GetComponent<HeroInfo>().HeroName.GetComponent<TextMeshProUGUI>().text =
					heroesData.dataArray.First(s => s.Id == heroList.Hero[cyrIndexHeroUpdate].HeroName).Name;*/

		heroFrameInfo.GetComponent<HeroInfo>().HeroSkill.GetComponent<Image>().sprite =
			heroList.Hero[cyrIndexHeroUpdate].HeroSkill;

		LocalizedString skillDescription = new LocalizedString("Titles", $"Skill_{heroList.Hero[cyrIndexHeroUpdate].HeroName}");
		skillDescription.GetLocalizedStringAsync().Completed += x =>
		{
			heroFrameInfo.GetComponent<HeroInfo>().SkillDescription.GetComponent<TextMeshProUGUI>().text = x.Result;
		};/*
		heroFrameInfo.GetComponent<HeroInfo>().SkillDescription.GetComponent<TextMeshProUGUI>().text =
			heroesData.dataArray.First(s => s.Id == heroList.Hero[cyrIndexHeroUpdate].HeroName).Abilitydiscription;*/

		heroFrameInfo.GetComponent<HeroInfo>().heroHP.text = Math.Round(heroesData.dataArray.First(s => s.Id == heroList.Hero[cyrIndexHeroUpdate].HeroName).Starthp + (heroList.Level[cyrIndexHeroUpdate] - 1) * progressData.dataArray[0].Plusheroeshp).ToString() + "<color=#76B72A> > " + Math.Round(heroesData.dataArray.First(s => s.Id == heroList.Hero[cyrIndexHeroUpdate].HeroName).Starthp + (heroList.Level[cyrIndexHeroUpdate]) * progressData.dataArray[0].Plusheroeshp).ToString();
		heroFrameInfo.GetComponent<HeroInfo>().heroDamage.text = Math.Round(heroesData.dataArray.First(s => s.Id == heroList.Hero[cyrIndexHeroUpdate].HeroName).Startdamage + (heroList.Level[cyrIndexHeroUpdate] - 1) * progressData.dataArray[0].Plusheroesdamage).ToString() + "<color=#76B72A> > " + Math.Round(heroesData.dataArray.First(s => s.Id == heroList.Hero[cyrIndexHeroUpdate].HeroName).Startdamage + (heroList.Level[cyrIndexHeroUpdate]) * progressData.dataArray[0].Plusheroesdamage).ToString();

		LocalizedString attackSpeed = new LocalizedString("Titles", heroesData.dataArray.First(s => s.Id == heroList.Hero[cyrIndexHeroUpdate].HeroName).Attackrate);
		attackSpeed.GetLocalizedStringAsync().Completed += x =>
		{
			heroFrameInfo.GetComponent<HeroInfo>().attackSpeed.text = x.Result;
		};/*
		heroFrameInfo.GetComponent<HeroInfo>().attackSpeed.text = heroesData.dataArray.First(s => s.Id == heroList.Hero[cyrIndexHeroUpdate].HeroName).Attackrate;*/
		heroFrameInfo.GetComponent<HeroInfo>().timeSkill.text = heroesData.dataArray.First(s => s.Id == heroList.Hero[cyrIndexHeroUpdate].HeroName).Abilitycooldown.ToString();
		//yield return null;
	}

	// Кнопка герои
	public void ButtonOn()
	{
		AudioLibrarySO.instance.PlayButtonSound();
		//StartCoroutine(UpdateHeroData());
		UpdateHeroData();

		foreach (var cyr in heroFrame)
		{
			cyr.GetComponent<OnFrame>().RotateFrame = false;
		}

		bBattle.SetActive(false);
		bHero.SetActive(false);
		heroFrameList.SetActive(true);
		//heroFrameInfoAdd.SetActive(false);
		//heroFrameInfoRemove.SetActive(false);
		placeFrameActivate(true);
		gotoFramePoint?.Invoke();
	}

	public void ButtonBack()
	{
		AudioLibrarySO.instance.PlayCollapseSound();
		RotateListFrame(false);
		RotateFrame(false);

		cyrIndexCheck = -2;

		heroFrameList.SetActive(true);
		heroFrameInfo.SetActive(false);
		heroFrameInfoAdd.SetActive(false);
		heroFrameInfoRemove.SetActive(false);
	}

	public void ButtonOff()
	{
		AudioLibrarySO.instance.PlayCollapseSound();
		RotateFrame(false);
		RotateListFrame(false);

		bBattle.SetActive(true);
		bHero.SetActive(true);
		heroFrameList.SetActive(false);
		heroFrameInfo.SetActive(false);
		heroFrameInfoAdd.SetActive(false);
		heroFrameInfoRemove.SetActive(false);
		placeFrameActivate(false);
		if (hero.Count <= 0) bBattle.SetActive(false);
		cyrIndexCheck = -1;
		gotoStartPoint?.Invoke();
	}

	// Update is called once per frame
	void Update()
	{
		tavernFSM.CurrentState.Update();
		if (tavernState == State.LoadData)
		{
			SetState(State.Wait);
			//StartCoroutine(LoadData());
			LoadData();
		}
	}

	private void SetState(State state)
	{
		tavernState = state;
		switch (state)
		{
			case State.Wait:
				break;
			default: break;
		}

	}
}
