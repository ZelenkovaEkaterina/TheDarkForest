using System;
using UnityEngine;
public enum Type
{
    Heal,
    Mana,
    Coin
}
[CreateAssetMenu(fileName = "SettingsInventory", menuName = "Scriptable Objects/Inventory")]
public class InventoryType : ScriptableObject
{
    public InventorySlots Slot;
}
[Serializable]
public class InventorySlots
{
    public SlotsSettings[] InvSlots = new SlotsSettings[3];
}

[Serializable]
public class SlotsSettings
{
    public GameObject Loot;
    public Type Type;
    public int Cost;
}
