using System;
using System.Collections;
//using System.Diagnostics;
using System.Linq;
using Spine;
using Spine.Unity;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnitControllerTutorial : MonoBehaviour
{
	[SerializeField] private AudioClip attackSound;
	[SerializeField] private AudioClip deathSound;
	[SerializeField] private AudioClip skillSound;
	public GameObject TextGuy;
	[SerializeField] private GameObject bFight;
	public bool startBattle = false;
	[SerializeField] private GameObject unitTarget2;
	private Vector3 startPosTransform;

	private float attackSoundDelay;
	private float attackTimer;

	#region Свойства

	public float UnitHP
	{
		get => unitHP;
	}

	public float UnitCyrHP
	{
		get => unitCyrHP;
		set
		{
			unitCyrHP = value;

			if (unitCyrHP <= 0f)
			{
				unitCyrHP = 0;
				if (meshRenderer.enabled) SetDie();
			}
			else hpValueBar.SetHP = unitCyrHP;
		}
	}

	public Transform UnitTarget
	{
		get => unitTarget;
		set => unitTarget = value;
	}

	public Transform Teleport
	{
		get => teleport;
		private set => teleport = value;
	}

	public Transform Death
	{
		get => death;
		private set => death = value;
	}

	public MeshRenderer MeshRenderer
	{
		get => meshRenderer;
		set => meshRenderer = value;
	}

	public AnimationController AnimationController
	{
		get => animationController;
		private set => animationController = value;
	}

	public float UnitSpeed
	{
		get => unitSpeed;
		private set => unitSpeed = value;
	}

	public float UnitDamage
	{
		get => unitDamage;
		set => unitDamage = value;
	}

	public float Accuracy
	{
		get => accuracy;
		private set => accuracy = value;
	}

	#endregion


	#region Поля


	[SerializeField] public float unitHP;

	[SerializeField] private float unitSpeed;
	[SerializeField] private float unitDamage;

	[SerializeField] private float accuracy;



	[SerializeField] private Transform unitTarget;

	[SerializeField] private MeshRenderer meshRenderer;

	[SerializeField] private AnimationController animationController;


	[SerializeField] private Transform teleport;

	[SerializeField] private Transform death;
	[SerializeField] private Transform skillButton;

	[SerializeField] private Transform hpBar;

	[SerializeField] private HPSystemTutorial hpValueBar;
	private float unitCyrHP;
	private int indexTutorial = 0;
	[SerializeField] private GameObject winObject;
	[SerializeField] private GameObject blackScreen;

	#endregion

	#region Публичные методы

	public void SetDie()
	{
		StartCoroutine(unitActivate());
		transform.tag = "Die";

		animationController.SetIdle();
		death.GetComponent<SkeletonAnimation>().state.SetAnimation(0, "death", false);

	}

	#endregion

	public void ActivateSkill()
	{
		SkeletonAnimation skeletonAnimation;

		GameObject blackScreen = GameObject.Find("BlackScreen");
		blackScreen.GetComponent<BlackScreen>().timer += 1f - blackScreen.GetComponent<BlackScreen>().timer;

		skeletonAnimation = GetComponent<SkeletonAnimation>();

		GetComponent<MeshRenderer>().sortingLayerName = "UI";
		GetComponent<MeshRenderer>().sortingOrder = 6;

		// Цель жива и Если расстояние до цели меньше чем accyracy иначе меняем состояние на движение к цели
		AnimationController.SetSkill();
		skeletonAnimation.AnimationState.Complete += SkillComplete;
		AudioLibrarySO.instance.PlayClip(skillSound, 2);
	}

	private void SkillComplete(TrackEntry trackentry)
	{
		// Иначе переключаемся на поиск ближайших целей
		if (trackentry.ToString() == "ability_active")
		{
			if (unitTarget != null &&
				Vector2.Distance(transform.position, unitTarget.transform.position) <=
				Accuracy)
			{
				unitTarget.GetComponent<UnitControllerTutorial>().UnitCyrHP -=
					unitDamage * 500;
			}

			GetComponent<AnimationController>().SetIdle();
			StartCoroutine(unitTutorialStateHide());
		}
	}


	#region Приватные методы

	private void Awake()
	{
		attackSoundDelay = attackSound.length;
		if (CompareTag("Enemy"))
		{
			var unitCanvas = GameObject.Find("Enemy Canvas").transform;
			hpBar.SetParent(unitCanvas);
			hpBar.transform.localScale = Vector3.one;
		}
	}

	private void Start()
	{
		startPosTransform = transform.position;
		TutorialGameManager.UnitCommand += unitCommand;
		GetComponent<SkeletonAnimation>().AnimationState.Event += HandleEvent;
		unitCyrHP = unitHP;
	}

	void unitCommand(int command)
	{
		switch (command)
		{
			case 0:
				StartCoroutine(unitTutorial_1());
				break;
			case 1:
				indexTutorial = 1;
				StartCoroutine(unitTutorial_2());
				break;
			case 2:
				indexTutorial = 2;
				StartCoroutine(unitTutorial_2());
				break;
			default: break;
		}

	}

	private void HandleEvent(TrackEntry trackEntry, Spine.Event e)
	{
		if (unitTarget != null && Vector2.Distance(transform.position, unitTarget.transform.position) <= accuracy)
		{
			if (e.Data.Name == "shoot")
			{
				unitTarget.GetComponent<UnitControllerTutorial>().UnitCyrHP -= unitDamage;
				if (unitTarget.GetComponent<UnitControllerTutorial>().UnitCyrHP <= 0f)
				{
					var rez = GameObject.FindGameObjectWithTag("Enemy");
					if (rez == null)
					{
						AnimationController.SetIdle();
					}
					else unitTarget = rez.transform;
				}
				if (attackTimer == 0 || Time.time - attackTimer > attackSoundDelay)
				{
					AudioLibrarySO.instance.PlayClip(attackSound, 9);
					attackTimer = Time.time;
				}
			}
		}


	}


	private IEnumerator unitTutorialStateHide()
	{
		yield return new WaitForSeconds(1f);
		Teleport.GetComponent<SkeletonAnimation>().state.SetAnimation(0, "teleport", false);
		var enabled = meshRenderer.enabled;
		hpBar.gameObject.SetActive(!enabled);
		meshRenderer.enabled = !enabled;
		skillButton.gameObject.SetActive(!enabled);

		yield return new WaitForSeconds(1.5f);
		if (indexTutorial == 0 && SceneManager.GetActiveScene().name == "BattleLearn")
		{
			skillButton.gameObject.SetActive(false);
			blackScreen.SetActive(true);
			AudioLibrarySO.instance.PlayVictorySound();
			winObject.SetActive(true);
		}
		else skillButton.gameObject.SetActive(false);
	}

	private IEnumerator unitTutorial_1()
	{
		AnimationController.SetIdle();
		yield return new WaitForSeconds(1f);

		if (CompareTag("Hero"))
		{
			AnimationController.SetIdle();
			Teleport.GetComponent<SkeletonAnimation>().state.SetAnimation(0, "teleport", false);
			AudioLibrarySO.instance.PlayTelepInSound();
			yield return new WaitForSeconds(0.5f);
			var enabled = meshRenderer.enabled;
			hpBar.gameObject.SetActive(!enabled);
			meshRenderer.enabled = !enabled;
		}
		else
		{
			yield return new WaitForSeconds(3.0f);
		}

		yield return null;
	}

	private IEnumerator unitTutorial_2()
	{
		AnimationController.SetIdle();
		yield return new WaitForSeconds(0.2f);

		if (CompareTag("Hero"))
		{
			transform.position = startPosTransform;
			AnimationController.SetIdle();
			unitTarget = GameObject.FindWithTag("Enemy").transform;
			Teleport.GetComponent<SkeletonAnimation>().state.SetAnimation(0, "teleport", false);
			yield return new WaitForSeconds(0.5f);
			var enabled = meshRenderer.enabled;
			hpBar.gameObject.SetActive(!enabled);
			meshRenderer.enabled = !enabled;
			startBattle = true;
			GetComponent<AnimationController>().SetMove();
		}
		else
		{
			unitTarget = GameObject.FindWithTag("Hero").transform;
			yield return new WaitForSeconds(0.7f);
			startBattle = true;
			GetComponent<AnimationController>().SetMove();
		}

		yield return null;
	}
	private void Update()
	{

		if (startBattle)
		{

			if (UnitTarget.GetComponent<UnitControllerTutorial>().unitCyrHP > 0f)
			{
				transform.position = Vector2.MoveTowards(transform.position, UnitTarget.position,
					3 * Time.deltaTime);
				transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
				if (Vector2.Distance(transform.position, UnitTarget.position) < accuracy)
				{
					GetComponent<AnimationController>().SetAttack();
					startBattle = false;
				}
			}
		}
	}


	private IEnumerator unitActivate()
	{
		var enabled = meshRenderer.enabled;
		if (!enabled)
		{
			yield return new WaitForSeconds(0.5f);
		}

		hpBar.gameObject.SetActive(!enabled);
		if (CompareTag("Hero"))
		{
			skillButton.gameObject.SetActive(!enabled);
			//Сброс прицела
			foreach (var cyr in GameObject.FindObjectsOfType<ButtonSkill>())
			{
				cyr.restartSkill();
			}
		}
		meshRenderer.enabled = !enabled;
		yield return new WaitForSeconds(0.5f);

		yield return null;
	}
	#endregion

	private void OnDisable()
	{
		TutorialGameManager.UnitCommand -= unitCommand;
		GetComponent<SkeletonAnimation>().AnimationState.Complete -= SkillComplete;
	}
}
