using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class AutoSkill : MonoBehaviour
{
	[SerializeField] private ADManager googleRewAd;
	[SerializeField] private int autoBattleStartNum = 3;
	private ButtonSkill[] nearestTarget = new ButtonSkill[0];
	[SerializeField] private Canvas canvas_layer;
	private float SkillAvtoTimer = 20f;
	private float cyrTimer = 0f;
	public bool SkillAvtoTimerEnable = false;
	private float fixedDeltaTime;
	private bool activeIEnumerator = false;
	private bool useButtonOn = false;
	private bool unlimAuto = false;

	private Image switchImage;
	[SerializeField] private Sprite[] switchSprites;
	private int buttonState;
	[SerializeField] private TextMeshProUGUI textUSEBUtton;
	[SerializeField] private TextMeshProUGUI textNumber;
	[SerializeField] private GameObject frameAutoBattle;
	[SerializeField] private GameObject useAutoBattle;
	[SerializeField] private GameObject blackScreen;
	private int timeToRestoreAuto;
	private DateTime lastTimeToRestoreAuto;
	public float cyrTimeScale, cyrFixedDeltaTime;
	private bool initDataB = false;
	[SerializeField] private ADManager adManager;



	// Start is called before the first frame update
	void Awake()
	{
		this.fixedDeltaTime = Time.fixedDeltaTime;
	}

	private void CheckRestoreAuto()
	{
		timeToRestoreAuto = OffLineTime();
		if (timeToRestoreAuto >= 12)
		{
			int num = SaveSystem.GetAutoPlayCount();

			if (num >= 0)
			{
				if (num <= (autoBattleStartNum - 1))
				{
					SaveSystem.SetAutoPlayCount(autoBattleStartNum, false);
					textNumber.text = autoBattleStartNum.ToString();
				}
			}
			lastTimeToRestoreAuto = SaveSystem.GetAutoPlayLastRestore();
			SaveSystem.SetAutoPlayLastRestore(DateTime.Now, true);
		}
	}

	private void initData()
	{
		int num = SaveSystem.GetAutoPlayCount();
		if (num < 0)
		{
			unlimAuto = true;
			textNumber.fontSize = 60;
			textNumber.text = "∞";
		}
		else
		{
			textNumber.text = num.ToString();
		}

		CheckRestoreAuto();
	}

	private int OffLineTime()
	{
		var ts = DateTime.Now - lastTimeToRestoreAuto;
		return ts.Hours;

	}

	private void Start()
	{
		buttonState = 0;

		lastTimeToRestoreAuto = SaveSystem.GetAutoPlayLastRestore();
		timeToRestoreAuto = OffLineTime();

	}

	public void ButtonPressed()
	{
		AudioLibrarySO.instance.PlayButtonSound();

		canvas_layer.sortingOrder = 10;
		frameAutoBattle.SetActive(true);
		GetComponent<Button>().interactable = false;

		blackScreen.SetActive(true);

		cyrTimeScale = Time.timeScale;
		cyrFixedDeltaTime = Time.fixedDeltaTime;

		Time.timeScale = 0f;
	}

	public void UseButtonOn()
	{
		AudioLibrarySO.instance.PlayButtonSound();

		switchImage = GetComponent<Button>().image;

		if (!useButtonOn && SaveSystem.GetAutoPlayCount() != 0)
		{
			ActivateAutoSkill();
		}
		else
		{
			DisactivateAutoSkill();
		}
	}

	public void AddAutoPlayNumUnlim()
	{
#if UNITY_WEBGL
		YandexGame.postPurchaseEvent = () =>
		{
			unlimAuto = true;
			textNumber.fontSize = 60;
			textNumber.text = "∞";
			SaveData();
		};
		YandexGame.BuyPayments("ulimited_autoplay");
#endif
	}


	public void AddAutoPlayNum()
	{
		if (!unlimAuto)
		{
			adManager.ShowRewarded(() => { int num = SaveSystem.GetAutoPlayCount(); num += 3; SaveSystem.SetAutoPlayCount(num, true); initData(); });
		}
	}

	public void Close()
	{
		AudioLibrarySO.instance.PlayButtonSound();
		//IronSourceEvents.onRewardedVideoAdRewardedEvent -= RewardedVideoAdRewardedEvent;
		blackScreen.SetActive(false);
		canvas_layer.sortingOrder = 6;
		Time.timeScale = cyrTimeScale;
		frameAutoBattle.SetActive(false);
		GetComponent<Button>().interactable = true;
	}


	private void ActivateAutoSkill()
	{
		useButtonOn = true;
		blackScreen.SetActive(false);
		Time.timeScale = cyrTimeScale;

		frameAutoBattle.SetActive(false);
		GetComponent<Button>().interactable = true;

		int number = 0;
		if (!unlimAuto) int.TryParse(textNumber.text, out number);
		//Debug.Log();
		//Debug.Log(number - 1);
		if (number > 0 || unlimAuto)
		{
			if (!unlimAuto)
			{
				number--;
				textNumber.text = number.ToString();
				SaveData();

			}

			SkillAvtoTimerEnable = true;
			cyrTimer = SkillAvtoTimer;
			useAutoBattle.GetComponent<Image>().color = Color.red;

			buttonState = 1 - buttonState;
			switchImage.sprite = switchSprites[buttonState];
		}
	}

	private void DisactivateAutoSkill()
	{
		useButtonOn = false;
		blackScreen.SetActive(false);
		Time.timeScale = cyrTimeScale;

		frameAutoBattle.SetActive(false);
		GetComponent<Button>().interactable = true;

		//GetComponent<Button>().enabled = true;
		//GetComponent<Image>().color= new Color(0.5f,0.5f,0.5f);

		SkillAvtoTimerEnable = false;
		cyrTimer = 0;

		StopCoroutine("pressSkill");
		activeIEnumerator = false;

		useAutoBattle.GetComponent<Image>().color = Color.white;

		if(SaveSystem.GetAutoPlayCount() != 0)
		{
			buttonState = 1 - buttonState;
			switchImage.sprite = switchSprites[buttonState];
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (!initDataB && textNumber)
		{
			initDataB = true;
			initData();
		}
		CheckRestoreAuto();
		if (SkillAvtoTimerEnable)
		{
			// Убрать если нужен таймер
			//cyrTimer -= Time.deltaTime;
			if (cyrTimer <= 0f)
			{
				//GetComponent<Image>().color= new Color(1f,1f,1f);
				GetComponent<Button>().enabled = true;
				SkillAvtoTimerEnable = false;
			}

			if (!activeIEnumerator)
			{
				activeIEnumerator = true;
				nearestTarget = GameObject.FindObjectsOfType<ButtonSkill>();
				StartCoroutine("pressSkill");
			}// Ищем все кнопки со скилами

			//Debug.Log(nearestTarget.Length);

		}

	}

	private void SaveData()
	{
		int number;
		if (!unlimAuto) int.TryParse(textNumber.text, out number);
		else number = -1;

		SaveSystem.SetAutoPlayCount(number, true);
	}

	private IEnumerator pressSkill()
	{
		// Перебираем кнопки и если скилл доступен активируем
		foreach (var variable in nearestTarget)
		{
			if (variable.button.GetComponent<Button>().enabled)
				if (!variable.varvar)
				{

					if (variable.targetGO == null)
					{
						variable.button.GetComponent<Button>().onClick.Invoke();
						//Debug.Log("Кнопка");
						//variable.pressTarget();
						yield return new WaitForSeconds(1.0f);
					}
					else
					{

						GameObject[] nearestTarget;
						switch (variable.UController.UnitSkill)
						{
							case UnitController.Skill.Knight:
								if (variable.button != null)
								{
									nearestTarget = GameObject.FindGameObjectsWithTag("Hero");
									if (nearestTarget != null)
									{
										variable.button.GetComponent<Button>().onClick.Invoke();
										variable.pressTarget();

										float HP = 10000;
										GameObject GO = null;
										foreach (var n in nearestTarget)
										{
											var cyrHP = n.GetComponent<UnitController>().UnitHP;
											if (HP > cyrHP)
											{
												HP = cyrHP;
												GO = n;
											}
										}

										GO.GetComponent<UnitController>().OnCollider();
									}
								}

								break;

							default: break;
						}
						yield return new WaitForSeconds(1.0f);
					}

				}
				else
				{
					//if (variable.asterix)
					//{
					// Если это ближний бой
					/*var nearestTarget = GameObject.FindGameObjectsWithTag("Enemy");

					foreach (var n in nearestTarget)
					{

						Vector3 target = n.transform.position;
						var dir = (target - position).normalized;
						var dot = Vector3.Dot(dir, rightDir);

						if(unitController.transform.localScale.x>=0&&dot >= 0 || unitController.transform.localScale.x<=0 && dot <= 0)
							if(Vector2.Distance(unitController.transform.position,n.transform.position)<distance)
							{
								n.GetComponent<UnitController>().UnitCyrHP -= unitController.UnitDamage*3f;
							}
					}*/
					//}

					GameObject nearestTarget;
					switch (variable.UController.UnitSkill)
					{
						case UnitController.Skill.Ice:
							if (variable.button != null)
							{
								nearestTarget = GameObject.FindGameObjectWithTag("Enemy");
								if (nearestTarget != null)
								{
									variable.button.GetComponent<Button>().onClick.Invoke();
									variable.targetGO.transform.position = nearestTarget.transform.position;

									var muzzleFlashPrefab = Resources.Load("NovaFrost") as GameObject;
									GameObject.Instantiate(muzzleFlashPrefab, variable.targetGO.transform.position,
										Quaternion.identity);

									variable.pressTarget();
								}
							}

							break;
						case UnitController.Skill.Varvar:
							if (variable.button != null)
							{
								nearestTarget = GameObject.FindGameObjectWithTag("Enemy");
								if (nearestTarget != null)
								{
									variable.button.GetComponent<Button>().onClick.Invoke();
									variable.targetGO.transform.position = nearestTarget.transform.position;
									var muzzleFlashPrefab1 = Resources.Load("anvil") as GameObject;
									GameObject.Instantiate(muzzleFlashPrefab1, variable.targetGO.transform.position,
										Quaternion.identity);

									variable.pressTarget();
								}
							}

							break;
						default: break;
					}

					yield return new WaitForSeconds(1.0f);

				}

		}

		activeIEnumerator = false;
		yield return null;
	}

	// Закомитеть если не нужна реклама
	void OnEnable()
	{
		//Add Rewarded Video Events

	}

	private void OnDisable()
	{
		//IronSourceEvents.onRewardedVideoAdRewardedEvent -= RewardedVideoAdRewardedEvent;
	}
	void OnApplicationPause(bool isPaused)
	{
		//#IronSource.Agent.onApplicationPause(isPaused);
	}


	void RewardedVideoAdRewardedEvent(IronSourcePlacement ssp)
	{

		int number = 0;

		int.TryParse(textNumber.text, out number);
		if (number < 100)
		{
			number += 3;
		}

		textNumber.text = number.ToString();
		SaveData();
	}

	public void RewardedVideoAdRewarded()
	{
		int number = 0;

		int.TryParse(textNumber.text, out number);
		if (number < 100)
		{
			number += 3;
		}

		textNumber.text = number.ToString();
		SaveData();
	}
}
