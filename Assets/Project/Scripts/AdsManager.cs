using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static event Action OnRewardedAdCompleted;

    public string androidGameId;
    public string iosGameId;
    [Space]
    public string androidInterstitialAdUnitId;
    public string iosInterstitialAdUnitId;
    public string androidRewardedAdUnitId;
    public string iosRewardedAdUnitId;

    private string interstitialAdUnitId;
    private string rewardedAdUnitId;

    public bool isTestingMode = true;

    string gameId;

    void Awake() {
        InitializeAds();
    }

    void InitializeAds() {


#if UNITY_IOS
        gameId = iosGameId;
#elif UNITY_ANDROID
        interstitialAdUnitId = androidInterstitialAdUnitId;
        rewardedAdUnitId = androidRewardedAdUnitId;
        gameId = androidGameId;
#elif UNITY_EDITOR
        interstitialAdUnitId = iosInterstitialAdUnitId;
        rewardedAdUnitId = iosRewardedAdUnitId;
        gameId = androidGameId;//for testing
#endif

        if (!Advertisement.isInitialized&&Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId, isTestingMode, this);//ONLY ONCE
        }

    }

    public void OnInitializationComplete()
    {
        print("Ads initialized!!");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        print("failed to initialize!!");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        print("interstitial loaded!!");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        print("interstitial failed to load");
    }

    public void ShowRewardedAd()
    {
        Advertisement.Show(rewardedAdUnitId, this);
    }

    public void ShowInterstitialAd() {
        print("showing ad!!");
        Advertisement.Show(interstitialAdUnitId, this);
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        print("interstitial clicked");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == rewardedAdUnitId && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            OnRewardedAdCompleted?.Invoke();
            print("rewarded show complete");
        }
        print("interstitial show complete");

    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        print("interstitial show failure");

    }

    public void OnUnityAdsShowStart(string placementId)
    {
        print("interstitial show start");

    }
}
