
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

[CreateAssetMenu(fileName = "YANDEX WEB SETTINGS", menuName = "TANDEX WEB SETTINGS", order = 1)]
public class YandexWebSettings : ScriptableObject
{
    [Header("LeaderBoards")]
    public string[] leaderBoardIds;

    [Space(8)]
    [Header("Localization Settings")]
    public List<LocalizedStrings> localizedStrings;

    public LocalizedData GetLocalizedData(string id, string code)
    {
        return YandexWebBaseFuctions.instance.settings.localizedStrings.Find(x => x.id == id).data.Find(x => x.languageCode == code);
    }
}

[Serializable]
public struct LocalizedStrings
{
    public string id;
    public List<LocalizedData> data;
}

[Serializable]
public struct LocalizedData
{
    public string languageCode;
    public TMP_FontAsset fontTMP;
    public Font fontLegacy;
    public string localizedTxt;
}
