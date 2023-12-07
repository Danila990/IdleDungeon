using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = System.Object;

public class ButtonSkill : MonoBehaviour
{
	private bool auto = false;
	public bool varvar;
	public GameObject button;
	public GameObject targetGO;
	public Vector3 targetGOLastPos;
	public GameObject anvilGO;

	public void ButtonPress() => buttonPress();
	public Image SkillImage;
	[SerializeField] private float time;
	private float fixedDeltaTime;
	private float cyrTime;

	private void Awake()
	{
		fixedDeltaTime = Time.fixedDeltaTime;
		if (targetGO != null) targetGOLastPos = Vector3.zero;
	}

	public float SetTime
	{
		get => time;
		set => time = value;
	}

	[SerializeField] private UnitController uController;


	public UnitController UController
	{
		get => uController;
		set => uController = value;
	}

	private bool active = false;
	private Vector3 mTarget;

	private void OnEnable()
	{
		//    StartCoroutine(ShowHideButton());
		if (!varvar) active = true;
	}

	/*    IEnumerator ShowHideButton()
		{
			button.GetComponent<Button>().enabled = false;
			SkillImage.fillAmount = 0;

			do
			{
				SkillImage.fillAmount += Time.deltaTime;
				yield return new WaitForSeconds(time);
			} while (SkillImage.fillAmount < 1f);


			button.GetComponent<Button>().enabled = true;


		}*/

	private Touch touch;
	private bool activeTouch = false;

	void EnemyLayerFront()
	{
		if (SaveSystem.GetTutorial() == 0)
		{
			SaveSystem.SetTutorial(1, true);

			var go = GameObject.Find("TutorRangeAbility");
			if (go != null)
			{
				go.GetComponent<TutorRangeAbility>().enemyORhero = true;
				go.GetComponent<TutorRangeAbility>().enabled = true;
				go.GetComponent<Image>().enabled = true;
				go.SetActive(true);
			}
		}

		if (targetGO != null) targetGOLastPos = Vector3.zero;

		foreach (var target in GameObject.FindGameObjectsWithTag("Enemy"))
		{
			target.GetComponent<UnitController>().GetComponent<MeshRenderer>().sortingLayerName = "UI";
			target.GetComponent<UnitController>().GetComponent<MeshRenderer>().sortingOrder = 8;
		}
	}

	void HeroLayerFront()
	{
		if (SaveSystem.GetTutorial() == 0)
		{
			SaveSystem.SetTutorial(1, true);

			var go = GameObject.Find("TutorRangeAbility");
			if (go != null)
			{
				go.GetComponent<TutorRangeAbility>().enemyORhero = false;
				go.GetComponent<TutorRangeAbility>().enabled = true;
				go.GetComponent<Image>().enabled = true;
				go.SetActive(true);
			}
		}

		if (targetGO != null) targetGOLastPos = Vector3.zero;

		foreach (var target in GameObject.FindGameObjectsWithTag("Hero"))
		{
			target.GetComponent<UnitController>().GetComponent<MeshRenderer>().sortingLayerName = "UI";
			target.GetComponent<UnitController>().GetComponent<MeshRenderer>().sortingOrder = 8;
		}
	}
	private void Update()
	{
		if (targetGO != null) targetGO.transform.position = targetGOLastPos;

		// Track a single touch as a direction control.
		if (Input.touchCount > 0)
		{

			touch = Input.GetTouch(0);

			// Handle finger movements based on TouchPhase
			switch (touch.phase)
			{
				//When a touch has first been detected, change the message and record the starting position
				case TouchPhase.Began:
					// Record initial touch position.
					//startPos = touch.position;
					//message = "Begun ";
					break;

				//Determine if the touch is a moving touch
				case TouchPhase.Moved:
					// Determine direction by comparing the current touch position with the initial one
					//direction = touch.position - startPos;
					//message = "Moving ";
					break;

				case TouchPhase.Ended:
					// Report that the touch has ended when it ends
					//message = "Ending ";
					break;
			}
		}



		if (active)
		{
			SkillImage.fillAmount += time * Time.deltaTime;

			if (SkillImage.fillAmount >= 1f)
			{
				active = false;
				button.GetComponent<Button>().enabled = true;
			}
		}


		if (SkillImage.fillAmount == 0)
		{
			if (!auto)
			{
				auto = true;
				var blackScreen = GameObject.Find("BlackTargetFon");
				if (blackScreen != null) blackScreen.GetComponent<Image>().enabled = true;

				switch (uController.UnitSkill)
				{
					case UnitController.Skill.Knight:
						HeroLayerFront();
						break;
					case UnitController.Skill.Varvar:
					case UnitController.Skill.Ice:
						EnemyLayerFront();
						break;

					default:
						break;
				}

				Time.timeScale = 0.25f;
			}

			switch (uController.UnitSkill)
			{
				case UnitController.Skill.Varvar:



#if UNITY_ANDROID && !UNITY_EDITOR
                    if (touch.phase==TouchPhase.Ended && activeTouch)
#else
					if (Input.GetMouseButtonUp(0) && activeTouch)
#endif
					{
						activeTouch = false;
						var muzzleFlashPrefab = Resources.Load("anvil") as GameObject;
						GameObject.Instantiate(muzzleFlashPrefab, targetGO.transform.position, Quaternion.identity);

						//****************************************************/
						Time.timeScale = cyrTime;

						pressTarget();
					}
#if UNITY_ANDROID && !UNITY_EDITOR
                    if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
#else
					if (Input.GetMouseButtonDown(0))
#endif
					{
						activeTouch = true;
#if UNITY_ANDROID && !UNITY_EDITOR
                        mTarget = Camera.main.ScreenToWorldPoint(touch.position);                        
#else
						mTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
#endif
						targetGOLastPos = new Vector3(mTarget.x, mTarget.y, 50f);
					}

					break;
				case UnitController.Skill.Ice:
#if UNITY_ANDROID && !UNITY_EDITOR
                    if (touch.phase==TouchPhase.Ended && activeTouch)
#else
					if (Input.GetMouseButtonUp(0) && activeTouch)
#endif
					{
						activeTouch = false;
						var muzzleFlashPrefab = Resources.Load("NovaFrost") as GameObject;
						GameObject.Instantiate(muzzleFlashPrefab, targetGO.transform.position, Quaternion.identity);

						Time.timeScale = cyrTime;

						pressTarget();
					}

#if UNITY_ANDROID && !UNITY_EDITOR
                    if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
#else
					if (Input.GetMouseButtonDown(0))
#endif
					{
						activeTouch = true;
#if UNITY_ANDROID && !UNITY_EDITOR
                        mTarget = Camera.main.ScreenToWorldPoint(touch.position);
#else
						mTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
#endif
						targetGOLastPos = new Vector3(mTarget.x, mTarget.y, 50f);
					}

					break;

				case UnitController.Skill.Knight:

#if UNITY_ANDROID && !UNITY_EDITOR
                    if (touch.phase==TouchPhase.Ended && activeTouch)
#else
					if (Input.GetMouseButtonUp(0) && activeTouch)
#endif
					{
						activeTouch = false;

						// Узнаём куда ткнули абилкой в какого героя
						var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
						RaycastHit2D hit = Physics2D.Raycast(ray.origin, -Vector2.up);
						if (hit.collider != null)
						{
							hit.transform.GetComponent<UnitController>().SetBlockDamage();
						}
						Time.timeScale = cyrTime;

						pressTarget();
					}

#if UNITY_ANDROID && !UNITY_EDITOR
                    //mTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
#else
					if (Input.GetMouseButtonDown(0) && !activeTouch)
#endif
					{
						activeTouch = true;
#if UNITY_ANDROID && !UNITY_EDITOR
                        mTarget = Camera.main.ScreenToWorldPoint(touch.position);
#else
						mTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
#endif
						targetGOLastPos = new Vector3(mTarget.x, mTarget.y, 50f);
					}

					break;
				default:
					break;
			}

#if !UNITY_ANDROID
			if (activeTouch)
			{
				mTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				targetGOLastPos = new Vector3(mTarget.x, mTarget.y, 50f);
			}
#endif

		}
	}

	private GameObject[] enemyTarget;

	private void buttonPress()
	{
		button.GetComponent<Button>().enabled = false;

		var autoPlayActive = SaveSystem.GetAutoPlayActive();
		if (!autoPlayActive)
		{
			cyrTime = 1.0f;
		}
		else
		{
			if (autoPlayActive)
			{
				cyrTime = 2.0f;
			}
			else
			{
				cyrTime = 1.0f;
			}

		}

		bool key = false;
		enemyTarget = GameObject.FindGameObjectsWithTag("Enemy");
		var autoPB = GameObject.Find("Auto Play Button");

		switch (uController.UnitSkill)
		{
			case UnitController.Skill.Varvar:
				targetGO.transform.position = uController.transform.position;
				targetGO.GetComponent<SpriteRenderer>().enabled = true;
				key = true;
				break;
			case UnitController.Skill.Ice:
				targetGO.transform.position = uController.transform.position;
				targetGO.GetComponent<SpriteRenderer>().enabled = true;
				key = true;
				break;
			case UnitController.Skill.Knight:
				targetGO.transform.position = uController.transform.position;
				targetGO.GetComponent<SpriteRenderer>().enabled = true;
				key = true;
				break;

			// Проверка что скил можно применить
			case UnitController.Skill.Asterix:
			case UnitController.Skill.FishMan:
			case UnitController.Skill.SimpleFemale:
			case UnitController.Skill.SimpleMale:
			case UnitController.Skill.SpearMan:
			case UnitController.Skill.Viking:

				if (SceneManager.GetActiveScene().name != "BattleLearn" && SceneManager.GetActiveScene().name != "BattleLearn2" && autoPB != null && autoPB.GetComponent<AutoSkill>().SkillAvtoTimerEnable)
				{
					if (TargetBool())
					{
						auto = true;
						active = true;
						uController.ActivateSkill();
						key = true;
					}
				}
				else
				{
					active = true;
					uController.ActivateSkill();
					key = true;
				}

				break;

			default:
				//targetGO.GetComponent<SpriteRenderer>().enabled=true;
				active = true;
				uController.ActivateSkill();
				key = true;
				break;
		}

		if (key) SkillImage.fillAmount = 0;
		else button.GetComponent<Button>().enabled = true;

		//if (!varvar)
		//{
		//    active = true;
		//    uController.ActivateSkill();
		//}else targetGO.GetComponent<SpriteRenderer>().enabled=true;}
		//StartCoroutine(ShowHideButton());    
	}

	private bool TargetBool()
	{

		Vector3 position = uController.transform.position;
		Vector3 rightDir = uController.transform.right;

		foreach (var n in enemyTarget)
		{


			Vector3 target = n.transform.position;
			var dir = (target - position).normalized;
			var dot = Vector3.Dot(dir, rightDir);

			if (uController.transform.localScale.x >= 0 && dot >= 0 ||
				uController.transform.localScale.x <= 0 && dot <= 0)
			{
				if (Vector2.Distance(uController.transform.position, n.transform.position) < 1f)
				{
					return true;
				}
			}
		}

		return false;
	}


	public void pressTarget()
	{
		//targetGO.GetComponent<Collider2D>().enabled=true;

		//var nearestTarget = GameObject.FindGameObjectsWithTag("Hero");

		//foreach (var n in nearestTarget)
		//{
		//    n.GetComponent<Collider2D>().enabled = true;
		//}
		var blackScreen = GameObject.Find("BlackTargetFon");
		if (blackScreen != null) blackScreen.GetComponent<Image>().enabled = false;

		auto = true;
		active = true;
		uController.ActivateSkill();
		targetGO.GetComponent<SpriteRenderer>().enabled = false;
		//targetGO.GetComponent<Collider2D>().enabled=false;
		//if(uController.UnitSkill!=UnitController.Skill.Knight)targetGO.GetComponent<SpriteRenderer>().enabled=false;
	}

	public void restartSkill()
	{
		auto = false;
		var autoPlayActive = SaveSystem.GetAutoPlayActive();
		if (!autoPlayActive)
		{
			Time.timeScale = 1.0f;
		}
		else
		{
			if (autoPlayActive)
			{
				Time.timeScale = 2.0f;
			}
			else
			{
				Time.timeScale = 1.0f;
			}

		}

		// Сбрасываем прицел
		switch (uController.UnitSkill)
		{
			case UnitController.Skill.Varvar:
			case UnitController.Skill.Ice:
			case UnitController.Skill.Knight:
				var blackScreen = GameObject.Find("BlackTargetFon");
				if (blackScreen != null) blackScreen.GetComponent<Image>().enabled = false;

				active = true;
				activeTouch = false;
				targetGO.GetComponent<SpriteRenderer>().enabled = false;
				//EnemyLayerBack();
				break;
			default:
				break;
		}


	}

}
