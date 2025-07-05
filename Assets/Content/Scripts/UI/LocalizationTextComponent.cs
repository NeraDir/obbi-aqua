using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class LocalizationTextComponent : MonoBehaviour
{
    public bool changeTxt;
    public string ru, en;
    public Font ruf, enf;

    private Text textComponent;

    private void OnEnable()
    {
        if (textComponent == null)
            textComponent = GetComponent<Text>();
        YG2.onSwitchLang += SwitchLanguage;
        SwitchLanguage(YG2.lang);
    }
    private void OnDisable()
    {
        YG2.onSwitchLang -= SwitchLanguage;
    }

    public void SwitchLanguage(string lang)
    {
        switch (lang)
        {
            case "ru":
                if (!changeTxt)
                    textComponent.text = ru;
                textComponent.font = ruf;
                break;
            default:
                if (!changeTxt)
                    textComponent.text = en;
                textComponent.font = enf;
                break;
        }
    }
}
