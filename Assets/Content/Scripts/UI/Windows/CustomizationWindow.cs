using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationWindow : Window
{
    [SerializeField] private CustomizationData _data;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private CustomizationItem _itemPrefab;

    private List<CustomizationItem> _items = new List<CustomizationItem>();

    public static Action onUpdateShop;

    private void Awake()
    {
        onUpdateShop += OnUpdateShop;
    }

    private void OnDestroy()
    {
        onUpdateShop -= OnUpdateShop;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SetupWindow(0);
        OnUpdateShop();
    }

    private void OnUpdateShop()
    {
        foreach (var item in _items)
        {
            item.OnUpdate();
        }
    }

    public void SetupWindow(int index)
    {
        if (_items.Count > 0)
        {
            foreach (var item in _items)
            {
                Destroy(item.gameObject);
            }
        }
        _items.Clear();
        foreach (var item in _data.GetAllItemsData()[index])
        {
            CustomizationItem newItem = Instantiate(_itemPrefab,_spawnPosition);
            newItem.Init(item);
            _items.Add(newItem);
        }
    }
}
