using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SettingsInventory", menuName = "Scriptable Objects/InventoryDB")]
public class InventoryDatabase : ScriptableObject
{
    public InventoryBase IDB;
}
[Serializable]
public class InventoryBase
{
    public DBSettings[] InvDB = new DBSettings[3];
}

[Serializable]
public class DBSettings
{
    public Type Type;
    public int Count;
}