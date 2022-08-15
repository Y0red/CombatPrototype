using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterStats_SO characterStats_Templet;
    public CharacterStats_SO characterDefination;
    public CharacterInventory charInv;
    public GameObject characterWeaponSlot;
    public GameObject characterShildSlot;

    #region Constructors
    public CharacterStats()
    {
        charInv = CharacterInventory.instance;
    }
    #endregion

    #region initialization

    private void Awake()
    {
        if (characterStats_Templet != null)
            characterDefination = Instantiate(characterStats_Templet);
    }
    private void Start()
    {
        
        if (characterDefination.isHero)
        {
            characterDefination.SetCharacterLevel(0);
        }
    }
    #endregion

    #region stat Increasers
    public void ApplyHealth(int healthAmount)
    {
        characterDefination.ApplyHealth(healthAmount);
    }

    public void ApplyMana(int manaAmount)
    {
        characterDefination.ApplyMana(manaAmount);
    }

    public void GiveWealth(int wealthAmount)
    {
        characterDefination.GiveWealth(wealthAmount);
    }

    public void IncreaseXp(int xp)
    {
        characterDefination.GiveXp(xp);
    }
    #endregion

    #region stat Decreasers
    public void TakeDamage(int amount)
    {
        characterDefination.TakeDamage(amount);
    }

    public void TakeMana(int amount)
    {
        characterDefination.TakeMana(amount);
    }
    #endregion

    #region weapon and armor change
    public void ChangeWeapon(ItemPickUp weaponPickUp)
    {
        if (!characterDefination.UnEquipeWeapon(weaponPickUp, charInv, characterWeaponSlot))
        {
            characterDefination.EquipWeapon(weaponPickUp, charInv, characterWeaponSlot);
        }
    }

    public void ChangeArmor(ItemPickUp armorPickUp)
    {
        if (!characterDefination.UnEquipArmor(armorPickUp, charInv))
        {
            characterDefination.EquipArmor(armorPickUp, charInv);
        }
    }
    #endregion

    #region reporters
    public int GetHealth()
    {
        return characterDefination.currentHealth;
    }

    public Weapon GetCurrentWeapon()
    {
        if (characterDefination.weapon != null)
        {
            return characterDefination.weapon.itemDefination.WeaponSlotObject;
        }
        else
        {
            return null;
        }
    }

    public int GetDamage()
    {
        return characterDefination.currentDamage;
    }

    public float GetResistance()
    {
        return characterDefination.currentResistance;
    }
    #endregion

    #region Udate

    private void Update()
    {
        //if (Input.GetMouseButtonDown(2))
        {
            //characterDefination.saveCharacterData();
        }
    }
    #endregion

    public void SetInitialHealth(int health)
    {
        characterDefination.maxHealth = health;
        characterDefination.currentHealth = health;
    }
    public void SetInitialResistance(int resistance)
    {
        characterDefination.baseResistance = resistance;
        characterDefination.currentResistance = resistance;
    }

    public void SetInitialDamage(int damage)
    {
        characterDefination.baseDameage = damage;
        characterDefination.currentDamage = damage;
    }
}
