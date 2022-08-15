using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemPickUp_SO templetIempickup;
    public ItemPickUp_SO itemDefination;

    public CharacterStats charStats;
    CharacterInventory charInventory;

    GameObject foundStats;
   

    #region Constructors
    public ItemPickUp()
    {
        charInventory = CharacterInventory.instance;
    }
    #endregion

    private void Start()
    {
        if(charStats == null)
        {
            foundStats = GameObject.FindGameObjectWithTag("Player");
            charStats = foundStats.GetComponent<CharacterStats>();
        }
    }

    void StoreItemInventory()
    {
        charInventory.StoreItem(this);
    }

    public void UseItem()
    {
        switch (itemDefination.itemType)
        {
            case ItemTypeDefinations.HEALTH:
                {
                    charStats.ApplyHealth(itemDefination.itemAmount);
                    break;
                }
            case ItemTypeDefinations.MANA:
                {
                    charStats.ApplyHealth(itemDefination.itemAmount);
                    break;
                }
            case ItemTypeDefinations.WEALTH:
                {
                    charStats.ApplyHealth(itemDefination.itemAmount);
                    break;
                }
            case ItemTypeDefinations.WEAPON:
                {
                    charStats.ChangeWeapon(this);
                    break;
                }
            case ItemTypeDefinations.ARMOR:
                {
                    charStats.ChangeArmor(this);
                    break;
                }
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Player" && !itemDefination.isEquipped)
        {
           if (itemDefination.isStorable)
           {
                StoreItemInventory();
           }
            else
            {
                UseItem();
            }
        }
    }

    
}
