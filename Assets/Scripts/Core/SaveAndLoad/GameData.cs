using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int GoldCoin;
    public int Soul;
    public string LastCheckPointId;

    public SerializableDictionary<int, int> InventoryItems;
    public SerializableDictionary<string, bool> SkillUnlock;
    public SerializableDictionary<string, bool> CheckPoints;
    public List<int> EquipmentItems;

    public GameData()
    {
        GoldCoin = 0;
        Soul = 0;
        LastCheckPointId = "";

        InventoryItems = new SerializableDictionary<int, int>();
        SkillUnlock = new SerializableDictionary<string, bool>();
        CheckPoints = new SerializableDictionary<string, bool>();
        EquipmentItems = new();
    }
}
