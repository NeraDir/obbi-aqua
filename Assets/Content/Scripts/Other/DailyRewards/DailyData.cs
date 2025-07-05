using System;
using UnityEngine;

[CreateAssetMenu(fileName = "new Daily Rewards", menuName = "Daily Rewards")]
public class DailyData : ScriptableObject
{
    public Week[] weekRewards;
}

[Serializable]
public struct Week
{
    public DayR[] dayRs;
}

[Serializable]
public struct DayR
{
    public int Day;
    public DayRewards[] dailyItemDatas;
}

[Serializable]
public struct DayRewards
{
    public Sprite sprite;
    public BuyType type;
    public int amount;
    public bool active;
}
