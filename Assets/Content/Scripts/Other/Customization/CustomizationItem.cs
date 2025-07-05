using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class CustomizationItem : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Text _priceTxt;
    [SerializeField] private Image _currencyImage;
    [SerializeField] private CustomButton _customButton;

    [SerializeField] private Image _stateImage;
    [SerializeField] private Sprite _stateSprites;

    [SerializeField] private Sprite[] _currencySprites;

    [SerializeField] private AudioClip _successBuy;
    [SerializeField] private AudioClip _unSuccessBuy;

    private CItemData _itemData;

    private CustomButton _button;

    private bool _equipted
    {
        get
        {
            var equiptedItems = YG2.saves.GetEquipedItems();
            return equiptedItems.TryGetValue(_itemData.itemType.ToString(), out int equippedIndex)
                && equippedIndex == _itemData.index;
        }
    }

    private bool _bought
    {
        get
        {
            var boughtItems = YG2.saves.GetBoughtItems();
            return boughtItems.ContainsKey($"{_itemData.itemType}{_itemData.index}");
        }
    }

    public void Init(CItemData data)
    {
        _button = GetComponent<CustomButton>();
        _button.SetAction(OnClickHandler);
        _itemData = data;
        var boughtItems = YG2.saves.GetBoughtItems();
        if (_itemData.index == 0 && !boughtItems.ContainsKey($"{_itemData.itemType}{_itemData.index}"))
        {
            boughtItems[$"{_itemData.itemType}{_itemData.index}"] = _itemData.index;
            YG2.saves.SaveBoughtItems(boughtItems);
        }
        OnUpdate();
    }

    private void OnClickHandler()
    {
        if (_bought)
        {
            OnEquip();
        }
        else
        {
            OnBuy();
        }
    }

    private void OnEquip()
    {
        var equiptedItems = YG2.saves.GetEquipedItems();
        if (_itemData.itemType == CustomItemType.Suit)
        {
            equiptedItems[CustomItemType.Middle.ToString()] = 0;
            equiptedItems[CustomItemType.Bottom.ToString()] = 0;
            equiptedItems[CustomItemType.Foot.ToString()] = 0;
        }
        else if (_itemData.itemType == CustomItemType.Middle || _itemData.itemType == CustomItemType.Bottom || _itemData.itemType == CustomItemType.Foot)
        {
            equiptedItems[CustomItemType.Suit.ToString()] = 0;
        }


        string key = $"{_itemData.itemType}";

        equiptedItems[key] = _itemData.index;



        YG2.saves.SaveEquiptedItems(equiptedItems);

        CustomizationWindow.onUpdateShop?.Invoke();
        OnUpdate();
    }

    private void OnBuy()
    {
        switch (_itemData.buyType)
        {
            case BuyType.Gold:
                if (YG2.saves.coins >= _itemData.price)
                {
                    YG2.saves.coins -= _itemData.price;
                    CompletePurchase();
                }
                else
                {
                    Settings.playSound?.Invoke(_unSuccessBuy);
                }
                break;

            case BuyType.Diamond:
                if (YG2.saves.diamonds >= _itemData.price)
                {
                    YG2.saves.diamonds -= _itemData.price;
                    CompletePurchase();
                }
                else
                {
                    Settings.playSound?.Invoke(_unSuccessBuy);
                }
                break;

            case BuyType.Ad:
                YandexWebBaseFuctions.onShowRewardedAd?.Invoke("skin", CompletePurchase);
                break;
        }
    }

    private void CompletePurchase()
    {
        Settings.playSound?.Invoke(_successBuy);

        var boughtItems = YG2.saves.GetBoughtItems();
        boughtItems[$"{_itemData.itemType}{_itemData.index}"] = _itemData.index;

        YG2.saves.SaveBoughtItems(boughtItems);
        OnEquip();
    }

    public void OnUpdate()
    {
        if (_itemData.sprite != null)
            _image.sprite = _itemData.sprite;
        if (_bought)
            _priceTxt.gameObject.SetActive(false);
      
        if(_itemData.price > 0)
        {
            switch (_itemData.buyType)
            {
                case BuyType.Gold:
                    _currencyImage.sprite = _currencySprites[0];
                    _priceTxt.text = _itemData.buyType != BuyType.Ad ?
                                     NumersFormatter.FormatNumber(_itemData.price) : "";
                    break;

                case BuyType.Diamond:
                    _currencyImage.sprite = _currencySprites[1];
                    _priceTxt.text = _itemData.buyType != BuyType.Ad ?
                                     NumersFormatter.FormatNumber(_itemData.price) : "";
                    break;

                case BuyType.Ad:
                    _currencyImage.gameObject.SetActive(false);
                    _priceTxt.text = YG2.lang == "ru" ? "¡≈—œÀ¿“ÕŒ" : "FREE";
                    break;
            }
        }
        else
        {
            _currencyImage.gameObject.SetActive(false);
            _priceTxt.text = "";
        }
        
        

        if (_equipted)
            _stateImage.gameObject.SetActive(true);
        else
            _stateImage.gameObject.SetActive(false);
    }
}
