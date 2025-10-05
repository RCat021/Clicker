using UnityEngine;
using System;

public static class CoinManager
{
    private const string Key = "Coins";
    private static int _coins;

    // 🔔 Событие, которое вызывается при изменении количества монет
    public static event Action<int> OnCoinsChanged;

    static CoinManager()
    {
        Load();
    }

    public static int Coins => _coins;

    public static void AddCoins(int amount)
    {
        _coins = Mathf.Max(0, _coins + amount);
        Save();
        OnCoinsChanged?.Invoke(_coins);
    }

    public static bool SpendCoins(int amount)
    {
        if (_coins >= amount)
        {
            _coins -= amount;
            Save();
            OnCoinsChanged?.Invoke(_coins);
            return true;
        }
        return false;
    }

    public static void SetCoins(int value)
    {
        _coins = Mathf.Max(0, value);
        Save();
        OnCoinsChanged?.Invoke(_coins);
    }

    private static void Save()
    {
        PlayerPrefs.SetInt(Key, _coins);
        PlayerPrefs.Save();
    }

    private static void Load()
    {
        _coins = PlayerPrefs.GetInt(Key, 0);
    }
}
