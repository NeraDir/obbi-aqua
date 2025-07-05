using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject _pcPanel;
    [SerializeField] private GameObject _mobilePanel;

    [SerializeField] private float _timeToDisplayAd;

    [SerializeField] private Window _adPrepareWindow;

    [SerializeField] private Image _musicMuteStateImage;
    [SerializeField] private Sprite[] _muscMuteStateSprites;

    [SerializeField] private Text _coinsTxt;
    [SerializeField] private Text _diamondTxt;

    public static Action onUpdateCoins;
    public static Action onUpdateDiamond;

    public static Window currentOpenWindow;

    private void Awake()
    {
        YandexWebBaseFuctions.onSetOrientationByDevice += OnSetOrientation;
        onUpdateCoins += OnUpdateCoins;
        onUpdateDiamond += OnUpdateDiamonds;
        OnUpdateCoins();
        OnUpdateDiamonds();
    }

    private void OnDestroy()
    {
        YandexWebBaseFuctions.onSetOrientationByDevice -= OnSetOrientation;
        onUpdateCoins -= OnUpdateCoins;
        onUpdateDiamond -= OnUpdateDiamonds;
    }

    public void Start()
    {
        YandexWebBaseFuctions.onLaunchInteractive?.Invoke();
        _musicMuteStateImage.sprite = Settings.instance.GetMusicState() ? _muscMuteStateSprites[0] : _muscMuteStateSprites[1];
        StartCoroutine(DisplayAdAfterTime());
    }

    private IEnumerator DisplayAdAfterTime()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(_timeToDisplayAd);
            if (YG2.isTimerAdvCompleted)
            {
                if (!_adPrepareWindow.gameObject.activeInHierarchy)
                {
                    _adPrepareWindow.Show();
                    currentOpenWindow.Hide();
                }
            }
        }
    }

    private void OnUpdateCoins()
    {
        _coinsTxt.text = NumersFormatter.FormatNumber(YG2.saves.coins);
    }

    private void OnUpdateDiamonds()
    {
        _diamondTxt.text = NumersFormatter.FormatNumber(YG2.saves.diamonds);
    }

    public void OnClickMuteMusic()
    {
        Settings.instance.MuteMusic();
        _musicMuteStateImage.sprite = Settings.instance.GetMusicState() ? _muscMuteStateSprites[0] : _muscMuteStateSprites[1];
    }

    private void OnSetOrientation(Device device)
    {
        switch (device)
        {
            case Device.PC:
                _pcPanel.SetActive(true);
                _mobilePanel.SetActive(false);
                break;
            case Device.Mobile:
                _pcPanel.SetActive(false);
                _mobilePanel.SetActive(true);
                break;
        }
    }
}
