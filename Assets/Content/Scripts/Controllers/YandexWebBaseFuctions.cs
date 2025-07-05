using System;
using System.Collections.Generic;
using UnityEngine;
using YG;
using YG.Insides;
using YG.Utils.LB;

public enum Device
{
    PC,
    Mobile,
}

public class YandexWebBaseFuctions : MonoBehaviour
{
    public static YandexWebBaseFuctions instance;

    public static Action onLaunchInteractive;
    public static Action<bool> onLaunchGameState;
    public static Action onShowInterstitial;
    public static Action<string, Action> onShowRewardedAd;
    public static Action<Action> onGameQuit;
    public static Action onLoadLeaderBoards;
    public static Action onLoadStorageData;
    public static Action onSaveStorageData;
    public static Action<Device> onSetOrientationByDevice;

    public YandexWebSettings settings;

    public List<LBData> leaderBoards = new List<LBData>();

    public Device device { get; private set; }

    private void OnEnable()
    {
        if (instance == null)
        {
            YG2.onGetLeaderboard += OnSetupLeaderBoardData;
            onShowInterstitial += OnShowInterstitial;
            onLaunchInteractive += OnLaunchInteractive;
            onLaunchGameState += OnLaunchGameState;
            onShowRewardedAd += OnShowRewardedAd;
            onGameQuit += OnGameQuit;
            onLoadLeaderBoards += OnLoadLeaderBoards;
            onLoadStorageData += OnLoadStorageData;
            onSaveStorageData += OnSaveStorageData;
            YG2.lang = YG2.saves.LanguageCode;
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        YG2.onGetLeaderboard -= OnSetupLeaderBoardData;
        onShowInterstitial -= OnShowInterstitial;
        onLaunchInteractive -= OnLaunchInteractive;
        onLaunchGameState -= OnLaunchGameState;
        onShowRewardedAd -= OnShowRewardedAd;
        onGameQuit -= OnGameQuit;
        onLoadLeaderBoards -= OnLoadLeaderBoards;
        onLoadStorageData -= OnLoadStorageData;
        onSaveStorageData -= OnSaveStorageData;
    }

    private void OnLaunchInteractive()
    {
        YG2.GameReadyAPI();
        YGInsides.LoadProgress();
        device = (Device)YG2.envir.device;
        onSetOrientationByDevice?.Invoke(device);
        YG2.saves.LanguageCode = YG2.envir.browserLang;
        YG2.lang = YG2.saves.LanguageCode;
        YG2.onSwitchLang?.Invoke(YG2.lang);
    }

    private void OnLaunchGameState(bool value)
    {
        if (value)
            YG2.GameplayStart();
        else
            YG2.GameplayStop();
    }

    private void OnShowInterstitial()
    {
        if (YG2.isSDKEnabled && YG2.isTimerAdvCompleted)
        {
            YG2.InterstitialAdvShow();
        }
    }

    private void OnShowRewardedAd(string rewardId, Action action)
    {
        YG2.RewardedAdvShow(rewardId, () =>
        {
            action?.Invoke();
        });
    }

    private void OnGameQuit(Action action)
    {
        action?.Invoke();
    }

    private void OnSaveStorageData()
    {
        YG2.SaveProgress();
    }

    private void OnLoadLeaderBoards()
    {
        foreach (var item in settings.leaderBoardIds)
        {
            YG2.GetLeaderboard(item);
        }
    }

    private void OnSetupLeaderBoardData(LBData data)
    {
        leaderBoards.Add(data);
    }

    private void OnLoadStorageData()
    {
        YGInsides.LoadProgress();
    }
}
