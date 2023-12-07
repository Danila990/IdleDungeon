using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

//[DefaultExecutionOrder(200)]
public class GameManager : MonoBehaviour
{
	public const string NO_ADS = "no_ads";
	public const string EXCLUSIVE = "exclusive_hero";
	[SerializeField] private NoAdWindow noAdWindow;
	[SerializeField] private ADManager ADManager;
	//AppMetrica
	public int gameLevel;
	public string level_name;
	public int gameLevelCount;
	private int GameTime = 0;
	private DateTime currentFirstStartupDate;

	//public GameObject autoSkill;
	[SerializeField] private int lvlToActivateAutoPlayBTN = 4;
	[SerializeField] private GameObject autoPlayBTN;
	[SerializeField] private TextMeshProUGUI textNumber;

	public TextMeshProUGUI tempLVLText;
	public int cyrRoomNumber = 1;
	//public GameObject butoonGameOver;
	public PopUp PopUpButton;
	// public GameObject buttonReward;
	public GameObject BlackScreen;
	public GameObject butoonWin;
	public GameObject Level;
	public GameObject LevelMas;
	public GameObject LevelBoss;

	public HeroBase HeroDB
	{
		get => heroBase;
		set => heroBase = value;
	}

	public List<GameObject> HeroUnits
	{
		get => heroUnits;
		set => heroUnits = value;
	}

	public List<GameObject> EnemyUnits
	{
		get => enemyUnits;
		set => enemyUnits = value;
	}

	public LevelData LevelDB
	{
		get => levelData;
		set => levelData = value;
	}

	public GameObject CyrLevel
	{
		get => cyrLevel;
		set => cyrLevel = value;
	}

	public GameObject NextLevel
	{
		get => nextLevel;
		set => nextLevel = value;
	}

	public void ChangeState(State state) => SetState(state);

	[SerializeField] private HeroList heroList;

	public HeroList HeroList
	{
		get => heroList;
		set => heroList = value;
	}

	[SerializeField] private heroesStats heroData;
	[SerializeField] private progress progressInfo;

	public heroesStats HeroData
	{
		get => heroData;
		set => heroData = value;
	}

	[SerializeField] private HeroBase heroBase;
	[SerializeField] private List<GameObject> heroUnits;
	[SerializeField] private List<GameObject> enemyUnits;
	[SerializeField] private LevelData levelData;
	[SerializeField] private GameObject cyrLevel;
	[SerializeField] private GameObject nextLevel;


	private GameFSM gameFSM;

	public enum State
	{
		Initialize,
		HeroLoadDB,
		LoadLevel,
		HeroesSet,
		EnemySet,
		StartGame,
		Wait,
		NextLevel,
		MoveLevel,
		LoadNextLevel,
		EndGame
	}

	[SerializeField] private State gameState;

	private int levelNapr = -1;
	private int levelNaprLast = -1;
	public AsyncOperation operation;
	private bool preloadTavern = true;



	private float fixedDeltaTime;
	public int rewardRange;

	private void Awake()
	{
		fixedDeltaTime = Time.fixedDeltaTime;
	}


	IEnumerator AsyncLoadingTavern()
	{
		operation = SceneManager.LoadSceneAsync("Tavern");
		// Предотвращаем автоматическое переключение при завершении загрузки
		operation.allowSceneActivation = false;

		yield return null;
	}

	void Start()
	{
		AudioLibrarySO.instance.PlayGameMusic();

		var gameLevel = SaveSystem.GetGameLevel();

		Debug.Log($"Starting level {gameLevel}");
		if(gameLevel > 3)
		{
			ADManager.ShowInterstitial(ShowNoAdWindow);
		}

		if (SceneManager.GetActiveScene().name != "BattleLearn2")
		{
			ADManager.Initialize();
			tempLVLText.text = "level: " + (gameLevel + 1).ToString();
		}

		if (gameLevel >= lvlToActivateAutoPlayBTN - 1)
		{
			autoPlayBTN.SetActive(true);
		}
		gameFSM = new GameFSM();
		gameFSM.Initialize(new gameInitialize());

		// Текущий уровень на старте
		Level = LevelMas;


		SetState(State.HeroLoadDB);
	}

	private void ClearDie()
	{
		var nearestTarget = GameObject.FindGameObjectsWithTag("Die");

		foreach (var n in nearestTarget)
		{
			Destroy(n);
		}

		nearestTarget = GameObject.FindGameObjectsWithTag("Hero");
		heroUnits.Clear();
		foreach (var n in nearestTarget)
		{
			heroUnits.Add(n);
		}
	}

	public void ShowNoAdWindow()
	{
		if (SaveSystem.NoAdsBought())
			return;

		Time.timeScale = 0f;
		noAdWindow.gameObject.SetActive(true);
	}

	void ResetTargetTime()
	{
		var fastMoveActive = SaveSystem.GetFastMode();
		if (!fastMoveActive)
		{
			Time.timeScale = 1.0f;
		}
		else
		{
			Time.timeScale = 2.0f;
		}
	}

	void Update()
	{
		gameFSM.CurrentState.Update();

		if (preloadTavern)
		{
			preloadTavern = false;
			var gameLVL = SaveSystem.GetGameLevel();
			//if(SceneManager.GetActiveScene().name!="BattleLearn2" && gameLVL < 4)StartCoroutine(AsyncLoadingTavern());
			if (SceneManager.GetActiveScene().name != "BattleLearn2") StartCoroutine(AsyncLoadingTavern());
		}

		if (gameState == State.MoveLevel)
		{
			var camWidth = Camera.main.pixelWidth;
			var camHeight = Camera.main.pixelHeight;

			var teleportEffects = GameObject.FindGameObjectsWithTag("Effect");
			if (teleportEffects != null)
				foreach (var cyr in teleportEffects)
				{
					cyr.GetComponent<MeshRenderer>().enabled = false;
				}


			var cyrLevelGO = cyrLevel.GetComponent<LevelData>().Room;
			var cyrLevelCanvas = cyrLevel.GetComponent<LevelData>().RoomCanvas;

			switch (levelNapr)
			{
				case 0:
					cyrLevelGO.transform.position = Vector2.MoveTowards(cyrLevelGO.transform.position, new Vector2(-cyrLevel.GetComponent<LevelData>().RoomCanvas.GetComponent<RectTransform>().sizeDelta.x, 0f), 20f * Time.deltaTime);
					break;
				case 1:
					cyrLevelGO.transform.position = Vector2.MoveTowards(cyrLevelGO.transform.position, new Vector2(0f, -cyrLevel.GetComponent<LevelData>().RoomCanvas.GetComponent<RectTransform>().sizeDelta.y), 20f * Time.deltaTime);
					break;
				case 2:
					cyrLevelGO.transform.position = Vector2.MoveTowards(cyrLevelGO.transform.position, new Vector2(0f, cyrLevel.GetComponent<LevelData>().RoomCanvas.GetComponent<RectTransform>().sizeDelta.y), 20f * Time.deltaTime);
					break;
				default: break;
			}
			nextLevel.GetComponent<LevelData>().Room.transform.position = Vector2.MoveTowards(nextLevel.GetComponent<LevelData>().Room.transform.position, new Vector2(0f, 0f), 20f * Time.deltaTime);
			if (Vector2.Distance(nextLevel.GetComponent<LevelData>().Room.GetComponent<RectTransform>().offsetMin, new Vector2(0f, 0f)) <= 0f)
			{
				nextLevel.GetComponent<LevelData>().RoomCanvas.transform.GetComponent<Canvas>().sortingOrder = 1;

				Destroy(cyrLevel);
				ClearDie();
				cyrLevel = nextLevel;

				int number = 0;

				int.TryParse(textNumber.text.Substring(0, textNumber.text.Length - 2), out number);
				number++;
				textNumber.text = number.ToString() + (SceneManager.GetActiveScene().name == "BattleLearn2" ? "/2" : "/3");


				teleportEffects = GameObject.FindGameObjectsWithTag("Effect");
				if (teleportEffects != null)
					foreach (var cyr in teleportEffects)
					{
						if (cyr.transform.name == "Teleport") cyr.GetComponent<MeshRenderer>().enabled = true;
					}




				ChangeState(State.HeroesSet);
			};
		}

		if (gameState == State.StartGame)
		{
			var unit = GameObject.FindGameObjectsWithTag("Enemy");
			if (unit.Length == 0)
			{
				//ResetTargetTime();

				gameState = State.Wait;


				// Выиграли
				int number = 0;

				int.TryParse(textNumber.text.Substring(0, textNumber.text.Length - 2), out number);

				if (number == (SceneManager.GetActiveScene().name == "BattleLearn2" ? 2 : 3))
				{
					if (SceneManager.GetActiveScene().name != "BattleLearn" &&
						SceneManager.GetActiveScene().name != "BattleLearn2")
					{
						StartCoroutine(WaitNagrada(2f));
					}
					else
					{
						StartCoroutine(WaitNagrada(2f));
					}
				}
				else
				{
					// Если нужен босс!!!!!!!!!!!!!!!!!!
					//if (number < 2)
					if (number < 3)
						Level = LevelMas;
					else Level = LevelBoss;

					//foreach (var cyr in GameObject.FindGameObjectsWithTag("Aim"))
					//{
					//    cyr.GetComponent<SpriteRenderer>().enabled=false;
					//}

					// Сбрасываем не применённое умение прицел
					//foreach (var cyr in GameObject.FindObjectsOfType<ButtonSkill>())
					//{
					//    cyr.restartSkill();
					//}



					ChangeState(State.NextLevel);
				}
			}

			unit = GameObject.FindGameObjectsWithTag("Hero");
			if (unit.Length == 0)
			{
				ResetTargetTime();

				gameState = State.Wait;

				if (SceneManager.GetActiveScene().name != "BattleLearn" &&
					SceneManager.GetActiveScene().name != "BattleLearn2")
				{
					int gameLevel = SaveSystem.GetGameLevel();
					StartCoroutine(WaitDefeat(2f));
				}
				else
				{
					StartCoroutine(WaitDefeat(2f));
				}
				//StartCoroutine(Lose()); // Просрали
			}

		}

		//        if (Input.GetKeyDown((KeyCode) '1'))
		//        {
		//            SetState(State.HeroLoadDB);
		//        }

	}

	private IEnumerator StartBattle()
	{

		var nearestTarget = GameObject.FindGameObjectsWithTag("Enemy");

		foreach (var n in nearestTarget)
		{
			n.GetComponent<UnitController>().SetTeleport();
		}

		nearestTarget = GameObject.FindGameObjectsWithTag("Hero");

		foreach (var n in nearestTarget)
		{
			n.GetComponent<UnitController>().SetTeleport();
		}

		yield return null;
	}

	private void SetState(State state)
	{
		gameState = state;
		switch (state)
		{
			case State.HeroLoadDB:
				gameFSM.ChangeState(new gameHeroLoadDB(this));
				break;
			case State.LoadLevel:
				gameFSM.ChangeState(new gameLoadLevel(this));
				SetHeroData();
				break;
			case State.HeroesSet:
				gameFSM.ChangeState(new gameHeroesSet(this));
				break;
			case State.EnemySet:
				gameFSM.ChangeState(new gameEnemySet(this));
				break;
			case State.StartGame:
				StartCoroutine(StartBattle());
				break;
			case State.NextLevel:
				cyrRoomNumber++;
				StartCoroutine(Wait(1.5f, State.LoadNextLevel));
				break;
			case State.LoadNextLevel:
				nLevel();
				break;

			case State.MoveLevel:
				break;
			default: break;
		}

	}


	// Устанавливаем характеристики героев перед боем
	private void SetHeroData()
	{
		int gameLevel = SaveSystem.GetGameLevel();
		//if(gameLevel!=0)
		//if (SceneManager.GetActiveScene().name != "BattleLearn1" &&
		//    SceneManager.GetActiveScene().name != "BattleLearn")tempLVLText.text = gameLevel.ToString() + "/4";

		//Debug.Log("GameLevel="+gameLevel);
		for (int index = 0; index < heroUnits.Count; index++)
		{
			//Debug.Log("1");
			foreach (var cyr in heroData.dataArray)
			{
				//Debug.Log(cyr.Id+"="+heroList.Hero[heroUnits[index].GetComponent<UnitController>().HeroDBIndex].HeroName);
				//enemiesInfo.dataArray.First(s=>s.Enemy==enemiesList[index].GetComponent<UnitController>().name).Starthp;
				if (cyr.Id == heroList.Hero[heroUnits[index].GetComponent<UnitController>().HeroDBIndex].HeroName)
				{
					//Debug.Log(cyr.Id);

					var level = SaveSystem.GetHero(heroUnits[index].GetComponent<UnitController>().HeroDBIndex).level;

					heroUnits[index].GetComponent<UnitController>().unitHP = (float)Math.Round(cyr.Starthp + progressInfo.dataArray[0].Plusheroeshp * (level - 1));
					heroUnits[index].GetComponent<UnitController>().UnitDamage = (float)Math.Round(cyr.Startdamage + progressInfo.dataArray[0].Plusheroesdamage * (level - 1));
					heroUnits[index].GetComponent<UnitController>().UnitSpeed = cyr.Velocity;
					heroUnits[index].GetComponent<UnitController>().UnitAbilityTime = cyr.Abilitystatsec;
					heroUnits[index].GetComponent<UnitController>().UnitFreazeTime = heroData.dataArray[8].Abilitystatsec;
					heroUnits[index].GetComponent<UnitController>().UnitDefTime = heroData.dataArray[1].Abilitystatsec;

					//Debug.Log("C="+(1f/cyr.Abilitycooldown));
					//Debug.Log("T="+heroUnits[index].GetComponent<UnitController>().SkillButton.GetComponent<ButtonSkill>().SetTime);
					heroUnits[index].GetComponent<UnitController>().SkillButton.GetComponent<ButtonSkill>().SetTime = 1f / cyr.Abilitycooldown;
					//heroUnits[index].GetComponent<UnitController>().UnitDamage = (float)Math.Round(cyr.Startdamage * Math.Pow(1.2f, level-1));
					//heroUnits[index].GetComponent<UnitController>().unitHP = (float)Math.Round(cyr.Starthp * Math.Pow(1.2f, level-1));

					//Debug.Log("HP="+heroUnits[index].GetComponent<UnitController>().unitHP);
					//Debug.Log("Damage="+heroUnits[index].GetComponent<UnitController>().UnitDamage);

					//heroUnits[index].GetComponent<UnitController>().UnitSpeed = (float)Math.Round(cyr.Velocity * Math.Pow(1.2f, level - 1));
				}
			}
		}
	}

	private IEnumerator Wait(float time, State state)
	{
		yield return new WaitForSeconds(time);
		ChangeState(state);
		yield return null;
	}

	private IEnumerator WaitNagrada(float time)
	{
		yield return new WaitForSeconds(time);
		BlackScreen.SetActive(true);
		if (SceneManager.GetActiveScene().name == "BattleLearn2")
		{
			AudioLibrarySO.instance.PlayVictorySound();
			butoonWin.SetActive(true);
		}
		else
		{
			PopUpButton.ADManager = ADManager;
			AudioLibrarySO.instance.PlayVictorySound();
			PopUpButton.ActivateWinPopUp();
		}
		yield return null;
	}

	private IEnumerator WaitDefeat(float time)
	{


		yield return new WaitForSeconds(time);
		BlackScreen.SetActive(true);
		//PopUpButoon.SetActive(true);
		PopUpButton.ADManager = ADManager;
		AudioLibrarySO.instance.PlayDefeatSound();
		PopUpButton.ActivateDefPopUp();
		yield return null;
	}

	/*private IEnumerator WaitReward(float time)
    {
        yield return new WaitForSeconds(time);
        BlackScreen.SetActive(true);
        //buttonReward.SetActive(true);
        PopUpButton.ActivateBonusPopUp();
        yield return null;
    }*/

	private void nLevel()
	{
		//Debug.Log(Random.Range(0, 3));
		switch (levelNapr)
		{
			case 0:
				levelNaprLast = -1;
				break;
			case 1:
				levelNaprLast = 2;
				break;
			case 2:
				levelNaprLast = 1;
				break;
			default: break;
		}

		//levelNaprLast = levelNapr;

		int number;

		do
		{
			levelNapr = Random.Range(0, 3);
		} while (levelNapr == levelNaprLast);


		Transform go;
		var camWidth = Camera.main.pixelWidth;
		var camHeight = Camera.main.pixelHeight;
		//camWidth = 2170;
		//camHeight = 1080;
		//levelNapr = 0;

		nextLevel = Instantiate(Level, transform.position + new Vector3(0f, .0f, 0f), Quaternion.identity, transform);
		switch (levelNapr)
		{
			case 0:
				nextLevel.GetComponent<LevelData>().Room.GetComponent<RectTransform>().offsetMin = new Vector2(nextLevel.GetComponent<LevelData>().RoomCanvas.GetComponent<RectTransform>().sizeDelta.x, 0f);
				nextLevel.GetComponent<LevelData>().Room.GetComponent<RectTransform>().offsetMax = new Vector2(nextLevel.GetComponent<LevelData>().RoomCanvas.GetComponent<RectTransform>().sizeDelta.x, 0f);
				nextLevel.GetComponent<LevelData>().RoomCanvas.transform.GetComponent<Canvas>().sortingOrder = 2;
				break;
			case 1:
				nextLevel.GetComponent<LevelData>().Room.GetComponent<RectTransform>().offsetMin = new Vector2(0f, nextLevel.GetComponent<LevelData>().RoomCanvas.GetComponent<RectTransform>().sizeDelta.y);
				nextLevel.GetComponent<LevelData>().Room.GetComponent<RectTransform>().offsetMax = new Vector2(0f, nextLevel.GetComponent<LevelData>().RoomCanvas.GetComponent<RectTransform>().sizeDelta.y);
				nextLevel.GetComponent<LevelData>().RoomCanvas.transform.GetComponent<Canvas>().sortingOrder = 0;
				break;
			case 2:
				nextLevel.GetComponent<LevelData>().Room.GetComponent<RectTransform>().offsetMin = new Vector2(0f, -nextLevel.GetComponent<LevelData>().RoomCanvas.GetComponent<RectTransform>().sizeDelta.y);
				nextLevel.GetComponent<LevelData>().Room.GetComponent<RectTransform>().offsetMax = new Vector2(0f, -nextLevel.GetComponent<LevelData>().RoomCanvas.GetComponent<RectTransform>().sizeDelta.y);
				nextLevel.GetComponent<LevelData>().RoomCanvas.transform.GetComponent<Canvas>().sortingOrder = 2;
				break;
			default:
				break;
		}



		LevelDB = NextLevel.GetComponent<LevelData>();
		ChangeState(State.MoveLevel); //;StartCoroutine(Wait(0f, State.MoveLevel));
	}

}


