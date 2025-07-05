using UnityEngine;
using UnityEngine.UI;
using YG;
using System;

public class DailyContentItem : MonoBehaviour
{
    [SerializeField] private Text[] _howMuchGetTxt;
    [SerializeField] private Image[] _itemImage;

    [SerializeField] private Image _contentImage;
    [SerializeField] private Color _active;
    [SerializeField] private Color _inactive;

    private DayRewards[] _itemDataArray;

    private CustomButton _customButton;

    public void Init(DayRewards[] dataArray)
    {
        _itemDataArray = dataArray;
        _customButton = GetComponent<CustomButton>();
        _customButton.SetAction(OnClick);
        UpdateVisual();
    }

    private void OnClick()
    {
        foreach (var item in _itemDataArray)
        {
            if (!item.active) continue;

            switch (item.type)
            {
                case BuyType.Gold:
                    YG2.saves.coins += item.amount;
                    break;
                case BuyType.Diamond:
                    YG2.saves.diamonds += item.amount;
                    break;
                default:
                    var boughtItems = YG2.saves.GetBoughtItems();
                    boughtItems[$"{item.type}{item.amount}"] = item.amount;
                    YG2.saves.SaveBoughtItems(boughtItems);
                    break;
            }
        }

        YG2.saves.lastDailyClaim = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
        YG2.saves.dayIndex += 1;
        if (YG2.saves.dayIndex >= 6)
        {
            YG2.saves.weekIndex += 1;
            YG2.saves.dayIndex = 0;
        }
        for (int i = 0; i < _itemDataArray.Length; i++)
        {
            _itemDataArray[i].active = false;
        }
        YG2.SaveProgress();

        UpdateVisual();
    }

    private void UpdateVisual()
    {
        
        bool isActive = false;
        for (int i = 0; i < _itemDataArray.Length; i++)
        {
            _itemImage[i].sprite = _itemDataArray[i].sprite;
            if (_itemDataArray[i].active)
            {
                isActive = true;
                break;
            }
        }

        _contentImage.color = isActive ? _active : _inactive;
    }
}
