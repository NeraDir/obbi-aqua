using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AdWillStartAfterTimeWindow : Window
{
    [SerializeField] private Window _clickWindwow;
    [SerializeField] private Text _timerTxt;
    private float _timeToDisplayAd = 2;

    public override void OnEnable()
    {
        MobileCamController.activated = false;
        StartCoroutine(WaitAndShowAd());
        base.OnEnable();
    }

    private IEnumerator WaitAndShowAd()
    {
        _timeToDisplayAd = 2;
        _timerTxt.text = _timeToDisplayAd.ToString("0");
        while (_timeToDisplayAd > 0)
        {
            _timerTxt.text = _timeToDisplayAd.ToString("0");
            yield return new WaitForSeconds(1);
            _timeToDisplayAd -= 1;
        }
        _timerTxt.text = _timeToDisplayAd.ToString("0");
        
        _clickWindwow.Show();
        YandexWebBaseFuctions.onShowInterstitial?.Invoke();
        if (!Cursor.visible)
        {
            Cursor.visible = true;
            Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
        }
        base.Hide();
    }
}
