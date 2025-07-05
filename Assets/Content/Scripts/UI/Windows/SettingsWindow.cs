using UnityEngine;
using UnityEngine.UI;
using YG;

public class SettingsWindow : Window
{
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private Slider _sensivitySlider;

    private void Awake()
    {
        _musicSlider.onValueChanged.AddListener((x) =>
        {
            YG2.saves.MusicVolume = x;
        });
        _soundSlider.onValueChanged.AddListener((x) =>
        {
            YG2.saves.SoundVolume = x;
        });
        _sensivitySlider.onValueChanged.AddListener((x) =>
        {
            YG2.saves.Sensivity = x;
        });
        _musicSlider.value = YG2.saves.MusicVolume;
        _soundSlider.value = YG2.saves.SoundVolume;
        _sensivitySlider.value = YG2.saves.Sensivity;
    }

    public void OnChangeLanguage(string value)
    {
        YG2.saves.LanguageCode = value;
        YG2.lang = YG2.saves.LanguageCode;
        YG2.onSwitchLang?.Invoke(YG2.lang);
        YG2.SaveProgress();
    }

    private void OnDisable()
    {
        YG2.SaveProgress();
    }
}
