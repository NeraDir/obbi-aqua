using UnityEngine;
using YG;

public class Coin : Item
{
    public override void OnCollision()
    {
        YG2.saves.coins += Random.Range(1,3);
        YG2.SaveProgress();
        GameController.onUpdateCoins?.Invoke();
        base.OnCollision();
    }
}
