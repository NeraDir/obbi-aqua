using System;
using UnityEngine;
using YG;

public class DailyManager : MonoBehaviour
{
    [SerializeField] private Window _dailyWindow;
    [SerializeField] private Window _viewWindow;
    [SerializeField] private DailyData _data;
    [SerializeField] private DailyContentItem[] _items;


    private void Awake()
    {

        if (YG2.saves.lastDailyClaim == 0)
        {
            MobileCamController.activated = false;
            Cursor.visible = true;
            Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
            _viewWindow.Hide();
            _dailyWindow.Show();

            UpdateWindow(true);
        }
        else
        {
            DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(YG2.ServerTime()).DateTime;
            DateTime lastTime = DateTimeOffset.FromUnixTimeMilliseconds(YG2.saves.lastDailyClaim).DateTime;
            if (dateTime.Date > lastTime.Date)
            {
                MobileCamController.activated = false;
                Cursor.visible = true;
                Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
                _viewWindow.Hide();
                _dailyWindow.Show();

                UpdateWindow(true);
            }
            else
            {
                UpdateWindow(false);
                MobileCamController.activated = true;
                Cursor.visible = false;
                Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
            }
        }
        
    }

    private void UpdateWindow(bool value)
    {
        var days = _data.weekRewards[YG2.saves.weekIndex].dayRs;
        for (int i = 0; i < _items.Length; i++)
        {
            if (i == YG2.saves.dayIndex)
            {
                for (int k = 0; k < days[i].dailyItemDatas.Length; k++)
                {
                    days[i].dailyItemDatas[k].active = value;
                }
            }
            _items[i].Init(days[i].dailyItemDatas);
        }
    }
}
