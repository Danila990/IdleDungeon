using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BonusReward : MonoBehaviour
{
	public GameManager gameManager;
	public upgraderules upgraderulesData;
	public levelsReward levelInfo;
	public progress progressInfo;
	[SerializeField] private GameObject AdRewardIconWin;
	[SerializeField] private GameObject AdRewardIconDef;
	[SerializeField] private GameObject Win;
	[SerializeField] private GameObject WinGO;
	[SerializeField] private GameObject Defeat;
	[SerializeField] private GameObject DefeatGO;
	[SerializeField] private TextMeshProUGUI TextReward;
	public TextMeshProUGUI textMoney;
	public TextMeshProUGUI textMoneyDefeat;
	public bool bWin = false;

	public void CloseReward()
	{
		var gameLevel = SaveSystem.GetGameLevel();
		if (SceneManager.GetActiveScene().name != "BattleLearn")
		{
			SaveSystem.SaveGameLevel(gameLevel + 1, true);
		}
		gameManager.operation.allowSceneActivation = true;
	}

	public void RewardPlay()
	{
		if (IronSource.Agent.isRewardedVideoAvailable())
		{

			IronSource.Agent.showRewardedVideo();
		}
		else
		{
			Debug.Log("unity-script: IronSource.Agent.isRewardedVideoAvailable - False");
		}
	}

	void OnEnable()
	{
		var gameLevel = SaveSystem.GetGameLevel() - 1;

		TextReward.text = ((int)(int.Parse(textMoney.text) * levelInfo.dataArray[0].Bonusreward)).ToString();
	}

	void RewardedBonus(IronSourcePlacement ssp)
	{
		IronSourceEvents.onRewardedVideoAdRewardedEvent -= RewardedBonus;

		AdRewardIconWin.SetActive(enabled);
		AdRewardIconDef.SetActive(enabled);

		if (bWin)
		{
			WinGO.SetActive(true);
		}
		else
		{
			DefeatGO.SetActive(true);
		}

		this.gameObject.SetActive(false);
	}
}
