using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using YG;

namespace YG
{
    public partial class SavesYG
    {
        public string boughtItemsJson = "{}";
        public string equipedItemsJson = "{\"Suit\":0,\"Middle\":0,\"Bottom\":0,\"Foot\":0,\"Top\":0,\"Pets\":0,\"Trail\":0,\"Wings\":0}";

        public string LanguageCode = "en";

        public float MusicVolume = 0.3f;

        public float SoundVolume = 0.3f;

        public float Sensivity = 3f;

        public bool MusicMute = false;

        public int coins = 10000;

        public int diamonds = 0;

        public int weekIndex = 0;

        public int dayIndex = 0;

        public long lastDailyClaim = 0;

        public Dictionary<string,int> GetEquipedItems()
        {
            if (string.IsNullOrEmpty(equipedItemsJson))
                return new Dictionary<string, int>();

            return JsonConvert.DeserializeObject<Dictionary<string, int>>(equipedItemsJson) ?? new Dictionary<string, int>();
        }

        public Dictionary<string, int> GetBoughtItems()
        {
            if (string.IsNullOrEmpty(boughtItemsJson))
                return new Dictionary<string, int>();

            return JsonConvert.DeserializeObject<Dictionary<string, int>>(boughtItemsJson) ?? new Dictionary<string, int>();
        }

        public void SaveBoughtItems(Dictionary<string, int> items)
        {
            boughtItemsJson = JsonConvert.SerializeObject(items);
            YG2.SaveProgress();
        }

        public void SaveEquiptedItems(Dictionary<string, int> items)
        {
            equipedItemsJson = JsonConvert.SerializeObject(items);
            YG2.SaveProgress();
        }
    }
}
