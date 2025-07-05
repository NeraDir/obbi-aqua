using System;
using System.Collections.Generic;
using UnityEngine;

public enum CustomItemType
{
    Top,
    Middle,
    Bottom,
    Foot,
    Suit,
    Pets,
    Trail,
    Wings
}

public enum BuyType
{
    Gold,
    Diamond,
    Ad,
    Suit,
    Pets,
    Top
}

[CreateAssetMenu(fileName = "Customization Data", menuName = "Create New Customization Data")]
public class CustomizationData : ScriptableObject
{
    public CItemData[] topItems;
    public CItemData[] middleItems;
    public CItemData[] bottomItems;
    public CItemData[] footItems;
    public CItemData[] suitItems;
    public CItemData[] petsItems;
    public CItemData[] trailItems;
    public CItemData[] wingsItems;

    public List<CItemData[]> GetAllItemsData()
    {
        List<CItemData[]> cItemDatas = new List<CItemData[]>
        {
            topItems,
            middleItems,
            bottomItems,
            footItems,
            suitItems,
            petsItems,
            trailItems,
            wingsItems,
        };
        return cItemDatas;
    }
}

[Serializable]
public struct CItemData
{
    public CustomItemType itemType;
    public BuyType buyType;
    public int index;
    public int price;
    public Sprite sprite;
}
