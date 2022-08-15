using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypeDefinations { HEALTH, WEALTH , MANA, WEAPON, ARMOR, BUFF, EMPTY};
public enum ItemArmorSubType { None, Head, Chest, Hand, Legs, Boots};

[CreateAssetMenu(fileName = "New Item", menuName = "Spawnable Item/ New Pick Up", order = 1)]
public class ItemPickUp_SO : ScriptableObject
{
    public string itemName = "New Item";
    public ItemTypeDefinations itemType = ItemTypeDefinations.HEALTH;
    public ItemArmorSubType itemArmorType = ItemArmorSubType.None;
    public int itemAmount = 0;
    public int spawnChanceWeight = 0;

    public Material itemMaterial = null;
    public Sprite itemIcon = null;
    public Rigidbody itemSpawnObject = null;
    public Weapon WeaponSlotObject = null;

    public bool isEquipped = false;
    public bool isInteractable = false;
    public bool isStorable = false;
    public bool isUnique = false;
    public bool isIndestructable = false;
    public bool isQuestItem = false;
    public bool isStackable = false;
    public bool destroyOnUse = false;
    public float itemWeight = 0f;
}
