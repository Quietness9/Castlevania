using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Currency Data",menuName ="GameData/CurrencyData")]
public class PlayerCurrencyData : ScriptableObject
{
    [field: Header("游戏货币")]
    [field:SerializeField] public int GoldCoin { get; private set; }
    [field:SerializeField] public int Soul { get; private set; }

    [SerializeField] int GoldCoinUpperLimit;
    [SerializeField] int SoulUpperLimit;


    public event Action OnGoldCoinUpdateEvent = delegate { };
    public event Action OnSoulUpdateEvent = delegate { };

    /// <summary>
    /// 设置货币
    /// </summary>
    /// <param name="coin"></param>
    /// <param name="soul"></param>
    public void SetCurrency(int coin,int soul)
    {
        GoldCoin= coin;
        Soul= soul;
        OnGoldCoinUpdateEvent();
        OnSoulUpdateEvent();
    }

    /// <summary>
    /// 增加金币
    /// </summary>
    /// <param name="amount"></param>
    public void IncreaseGoldCoin(int amount)
    {
        if (GoldCoin >= GoldCoinUpperLimit)
        {
            Debug.Log("已经到达金币上限");
            return;
        }

        ChangeCurrency(CurrencyType.GoldCoin, amount);
    }

    /// <summary>
    /// 减少金币
    /// </summary>
    /// <param name="amount"></param>
    public void ReduceGoldCoin(int amount)
    {
        if (GoldCoin <= 0)
            return;

        ChangeCurrency(CurrencyType.GoldCoin, -amount);
    }

    /// <summary>
    /// 增加灵魂
    /// </summary>
    /// <param name="amount"></param>
    public void IncreaseSoul(int amount)
    {
        if (Soul >= SoulUpperLimit)
        {
            Debug.Log("已经到达灵魂上限");
            return;
        }

        ChangeCurrency(CurrencyType.Soul, amount);
    }

    /// <summary>
    /// 减少灵魂
    /// </summary>
    /// <param name="amount"></param>
    public void ReduceSoul(int amount)
    {
        if (Soul <= 0)
            return;

        ChangeCurrency(CurrencyType.Soul, -amount);
    }


    /// <summary>
    /// 改变货币
    /// </summary>
    /// <param name="currencyType"></param>
    /// <param name="amount"></param>
    private void ChangeCurrency(CurrencyType currencyType, int amount)
    {
        if (currencyType == CurrencyType.GoldCoin)
        {
            GoldCoin += amount;
            OnGoldCoinUpdateEvent();
        }

        if (currencyType == CurrencyType.Soul)
        {
            Soul += amount;
            OnSoulUpdateEvent();
        }
    }
}
