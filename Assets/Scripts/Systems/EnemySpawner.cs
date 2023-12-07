using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	private GameManager gameManager;
	public enum SpawnType
	{
		OneForAll,
		Different,
		NoRepeat
	}
	public SpawnType spawnType;
	public Transform[] spawnPoints;
	public List<Transform> lvlSpawnPoints;
	public List<GameObject> enemies;
	public GameObject[] enemiesList;
	public bool LoadComplete = false;
	[SerializeField] public levels levelsInfo;
	[SerializeField] public enemiesStats enemiesInfo;
	[SerializeField] public progress progressInfo;
	[SerializeField] public heroesStats heroData;


	private void AppMetrica_Start(int gameLevel, string level_name)
	{
		var gameLevelCount = SaveSystem.GetLevelsCompleted();
		gameLevelCount++;
		SaveSystem.SetLevelsCompleted(gameLevelCount, true);

		gameManager.level_name = level_name;
		gameManager.gameLevel = gameLevel;
		gameManager.gameLevelCount = gameLevelCount;
	}

	private void Start()
	{
		gameManager = GameObject.FindObjectOfType<GameManager>();

		// Получить текущий уровень из данных на диске
		int cyrLevel = SaveSystem.GetGameLevel();
		int cyrLevelIndex = 0;
		bool trueSeach = false;

		// Ищем значение равное текущему уровню и сохраняем индекс
		for (int index = 0; index < levelsInfo.dataArray.Length; index++)
		{
			if (levelsInfo.dataArray[index].Lvl == cyrLevel)
			{
				cyrLevelIndex = index;
				trueSeach = true;
			}
		}

		// Значения равное текущему уровню нет будем искать максимально приближенное к уровню
		if (!trueSeach)
		{
			int maxValue = 0;
			int maxValueIndex = 0;
			for (int index = 0; index < levelsInfo.dataArray.Length; index++)
			{
				if (levelsInfo.dataArray[index].Lvl < cyrLevel && levelsInfo.dataArray[index].Lvl > maxValue)
				{
					maxValue = levelsInfo.dataArray[index].Lvl;
					maxValueIndex = index;
				}
			}

			cyrLevelIndex = maxValueIndex;
		}

		AppMetrica_Start(cyrLevel, cyrLevelIndex.ToString());

		LoadLVLInfo(cyrLevelIndex, cyrLevel);

		fillEnemyData();

		Spawn();
	}

	void LoadLVLInfo(int lvlIndex, int cyrLvl)
	{
		var gameManager = GameObject.Find("GameManager");
		int numEnemy = 0;
		bool boss = false;

		//Debug.Log("Room="+gameManager.GetComponent<GameManager>().cyrRoomNumber);
		switch (gameManager.GetComponent<GameManager>().cyrRoomNumber)
		{
			case 1:
				numEnemy = levelsInfo.dataArray[lvlIndex].Room1;
				//Debug.Log("Room1="+numEnemy);
				break;
			case 2:
				numEnemy = levelsInfo.dataArray[lvlIndex].Room2;
				//Debug.Log("Room2="+numEnemy);
				break;
			case 3:
				numEnemy = levelsInfo.dataArray[lvlIndex].Room3;

				// Боссы если есть все другие гасятся в сторонке
				for (int index = 12; index < 15; index++)
				{
					//if (enemiesInfo.dataArray[index].Endonlvl > lvlIndex && enemiesInfo.dataArray[index].Startonlvl <= lvlIndex)
					if (enemiesInfo.dataArray[index].Endonlvl > cyrLvl && enemiesInfo.dataArray[index].Startonlvl <= cyrLvl)
					{

						// Попадает ли в интервал появления
						//if (((lvlIndex - enemiesInfo.dataArray[index].Startonlvl) % enemiesInfo.dataArray[index].Intervalsoflvl) == 0)
						if (((cyrLvl - enemiesInfo.dataArray[index].Startonlvl) % enemiesInfo.dataArray[index].Intervalsoflvl) == 0)
						{
							numEnemy = 1;
							boss = true;
							// Добавляем вражину в список для шинковки
							enemies.Add(enemiesList[index]);
						}
					}

				}
				break;
			default: break;
		}


		if (!boss)
		{
			int indexSpawnPoints = 0;
			// Перебираем всех врагов и ставим их в список для загрузки на уровне
			for (int index = 0; index < enemiesInfo.dataArray.Length - 3; index++)
			//foreach (var cyr in enemiesInfo.dataArray)
			{
				// Может ли существовать на текущем уровне
				if (enemiesInfo.dataArray[index].Endonlvl > cyrLvl &&
					enemiesInfo.dataArray[index].Startonlvl <= cyrLvl)
				{
					// Попадает ли в интервал появления
					//Debug.Log("first=" + (lvlIndex + 1 - enemiesInfo.dataArray[index].Startonlvl) %
					//    enemiesInfo.dataArray[index].Intervalsoflvl);
					// Debug.Log("second=" + (lvlIndex + 1 - enemiesInfo.dataArray[index].Startonlvl) %
					//    enemiesInfo.dataArray[index].Intervalsoflvl);
					if ((cyrLvl - enemiesInfo.dataArray[index].Startonlvl) %
						enemiesInfo.dataArray[index].Intervalsoflvl == 0)
					{

						// Добавляем вражину в список для шинковки
						enemies.Add(enemiesList[index]);

						// Изменяем его характеристики
					}
				}
			}
		}

		// Добавляем точки тоявления по количеству врагов
		for (int index = 0; index < numEnemy; index++)
		{
			lvlSpawnPoints.Add(spawnPoints[index]);
			// Debug.Log(index);
		}



		//enemiesInfo.dataArray[0].Enemy
	}


	// Заполняем характеристики врагов, урон, хп, скорость
	private void fillEnemyData()
	{
		for (int index = 0; index < enemiesInfo.dataArray.Length; index++)
		{
			// Вычисление HP по таблице с коэфициэнтами
			int lvlIndex = SaveSystem.GetGameLevel();

			// Получаем первоначальные значения
			float cyrHP = enemiesInfo.dataArray.FirstOrDefault(s => s.Enemy == enemiesList[index].GetComponent<UnitController>().name)!.Starthp;
			float cyrDamage = enemiesInfo.dataArray.FirstOrDefault(s => s.Enemy == enemiesList[index].GetComponent<UnitController>().name)!.Startdamage;

			// Калькулируем значения от текущего уровня
			int lastI = 0;
			for (int i = 2; i <= lvlIndex; i++)
			{
				var cyrIndex = i < (levelsInfo.dataArray.Length - 1) ? i : (levelsInfo.dataArray.Length - 1);

				if ((levelsInfo.dataArray[cyrIndex].Lvl) == i)
				{
					lastI = cyrIndex;
				}

				cyrHP += (index < 12 ? progressInfo.dataArray[0].Plusenemyhp : progressInfo.dataArray[0].Plusenemybosshp) * levelsInfo.dataArray[lastI].Coefenemyhp;
				cyrHP *= (cyrIndex == i ? levelsInfo.dataArray[cyrIndex].Multiplierhp : 1);
				cyrDamage += index < 12 ? progressInfo.dataArray[0].Plusenemydamage : progressInfo.dataArray[0].Plusenemybossdamage;
			}

			enemiesList[index].GetComponent<UnitController>().unitHP = cyrHP;
			enemiesList[index].GetComponent<UnitController>().UnitDamage = cyrDamage;
			enemiesList[index].GetComponent<UnitController>().UnitSpeed = enemiesInfo.dataArray.FirstOrDefault(s => s.Enemy == enemiesList[index].GetComponent<UnitController>().name)!.Velocity;
			enemiesList[index].GetComponent<UnitController>().UnitFreazeTime = heroData.dataArray[8].Abilitystatsec;
		}
	}

	public void Spawn()
	{
		switch (spawnType)
		{
			case SpawnType.OneForAll:
				SpawnSame(lvlSpawnPoints, enemies);
				break;
			case SpawnType.Different:
				//SpawnDiff(spawnPoints, enemies);
				SpawnDiff(lvlSpawnPoints, enemies);
				break;
			case SpawnType.NoRepeat:
				SpawnNoRepeat(lvlSpawnPoints, enemies);
				break;
		}

		LoadComplete = true;
	}

	private void SpawnEnemy(Transform point, GameObject enemy)
	{
		if (enemy) Instantiate(enemy, point.position, Quaternion.identity, this.transform);
	}

	private void SpawnDiff(List<Transform> points, List<GameObject> enemies)
	{
		foreach (Transform point in points)
		{
			GameObject enemy = enemies[Random.Range(0, enemies.Count)];
			SpawnEnemy(point, enemy);
		}
	}

	private void SpawnSame(List<Transform> points, List<GameObject> enemies)
	{
		GameObject enemy = enemies[Random.Range(0, enemies.Count)];

		foreach (Transform point in points)
		{
			SpawnEnemy(point, enemy);
		}
	}


	private void SpawnNoRepeat(List<Transform> points, List<GameObject> enemies)
	{
		List<GameObject> usedAlready = new List<GameObject>();

		while (usedAlready.Count < enemies.Count)
			foreach (Transform point in points)
			{
				while (usedAlready.Count < enemies.Count)
				{
					GameObject enemy = enemies[Random.Range(0, enemies.Count)];
					while (usedAlready.Contains(enemy))
					{
						enemy = enemies[Random.Range(0, enemies.Count)];
					}
					usedAlready.Add(enemy);
					SpawnEnemy(point, enemy);
				}

			}

	}

}
