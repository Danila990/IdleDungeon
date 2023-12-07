using CAS.AdObject;
using System;
using Unity.VisualScripting;
using UnityEngine;
using YG;

public class ADManager : MonoBehaviour
{
	#region Variables

	[SerializeField] private InterstitialAdObject interstitial;
	[SerializeField] private RewardedAdObject rewarded;

	private float cachedTimeScale;

	#endregion

	#region Functions

	/// <summary>
	/// Subscribe on default callbacks and load AD
	/// </summary>
	public void Initialize()
	{
#if !UNITY_WEBGL
		interstitial.OnAdFailedToLoad.RemoveAllListeners();
		interstitial.OnAdFailedToLoad.AddListener((error) =>
		{
			Debug.Log($"Failed to load interstitial ad: {error}");
			EndADShow();
		});

		interstitial.OnAdFailedToShow.RemoveAllListeners();
		interstitial.OnAdFailedToShow.AddListener((error) =>
		{
			Debug.Log($"Failed to show interstitial ad: {error}");
			EndADShow();
		});

		interstitial.OnAdShown.RemoveAllListeners();
		interstitial.OnAdShown.AddListener(StartADShow);

		rewarded.OnAdFailedToLoad.RemoveAllListeners();
		rewarded.OnAdFailedToLoad.AddListener((error) =>
		{
			Debug.Log($"Failed to load rewarded ad: {error}");
			EndADShow();
		});

		rewarded.OnAdFailedToShow.RemoveAllListeners();
		rewarded.OnAdFailedToShow.AddListener((error) =>
		{
			Debug.Log($"Failed to show rewarded ad: {error}");
			EndADShow();
		});
		rewarded.OnAdShown.RemoveAllListeners();
		rewarded.OnAdShown.AddListener(StartADShow);
#else
		YandexGame.ErrorFullAdEvent = () =>
		{
			Debug.Log("Failed to show interstitial ad");
			EndADShow();
		};
		YandexGame.OpenFullAdEvent = StartADShow;

		YandexGame.ErrorVideoEvent = () =>
		{
			Debug.Log("Failed to show rewarded ad");
			EndADShow();
		};
		YandexGame.OpenVideoEvent = StartADShow;
#endif

		LoadAD();
	}

	public void LoadAD()
	{
#if !UNITY_WEBGL
		interstitial.LoadAd();
		rewarded.LoadAd();
#endif
	}

	public void ShowInterstitial(Action onClosed)
	{
		if (SaveSystem.NoAdsBought())
		{
			onClosed?.Invoke();
			return;
		}
#if UNITY_WEBGL
		YandexGame.CloseFullAdEvent = () =>
		{
			EndADShow();
			onClosed?.Invoke();
		};
		YandexGame.FullscreenShow();
#else
		interstitial.OnAdClosed.RemoveAllListeners();
		interstitial.OnAdClosed.AddListener(() =>
		{
			EndADShow();
			onClosed?.Invoke();
		});

		interstitial.Present();
#endif
	}

	public void ShowRewarded(Action onReward, Action onClosed = null)
	{
#if UNITY_WEBGL
		YandexGame.CloseVideoEvent = () =>
		{
			onClosed?.Invoke();
			EndADShow();
		};
		YandexGame.RewardVideoEvent = (x) =>
		{
			onReward?.Invoke();
		};
		YandexGame.RewVideoShow(0);
#else
		rewarded.OnAdClosed.RemoveAllListeners();
		rewarded.OnAdClosed.AddListener(() =>
		{
			EndADShow();
			onClosed?.Invoke();
		});

		rewarded.OnReward.RemoveAllListeners();
		rewarded.OnReward.AddListener(() =>
		{
			onReward?.Invoke();
		});

		rewarded.Present();
#endif
	}

	private void StartADShow()
	{
		cachedTimeScale = Time.timeScale;
		Time.timeScale = 0f;
		AudioListener.pause = true;
	}

	private void EndADShow()
	{
		Time.timeScale = cachedTimeScale <= 0.01 ? 1f : cachedTimeScale;
		AudioListener.pause = false;
	}

	#endregion
}
