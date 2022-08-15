using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Stats", menuName = "Character/Stats", order = 1)]
public class CharacterStats_SO : ScriptableObject
{
    public Events.EventIntegerEvent OnLevelUp;
    public Events.EventIntegerEvent OnHeroDamaged;
    public Events.EventIntegerEvent OnHeroGainedHealth;

    public UnityEvent OnHeroDeath;
    public UnityEvent OnHeroInitialized;

    [System.Serializable]
    public class CharLevelUps
    {
        public int maxHealth;
        public int maxMana;
        public int maxWealth;
        public int baseDamage;
        public float baseResistance;
        public float maxEncumbrance;
        public int requiredXP;
    }

    #region Fields
    // public bool setManually = false;
    //public bool saveDataOnClose = false;
    public bool isHero = false;

    public ItemPickUp weapon { get; private set; }
    public ItemPickUp headArmor { get; private set; }
    public ItemPickUp chestArmor { get; private set; }
    public ItemPickUp handArmor { get; private set; }
    public ItemPickUp legArmor { get; private set; }
    public ItemPickUp footArmor { get; private set; }
    public ItemPickUp bootsArmor { get; private set; }
    public ItemPickUp misc1 { get; private set; }
    public ItemPickUp misc2 { get; private set; }

    public int maxHealth = 0;
    public int currentHealth = 0;

    public int maxMana = 0;
    public int currentMana = 0;

    public int maxWealth = 0;
    public int currentWealth = 0;

    public int baseDameage = 0;
    public int currentDamage = 0;

    public float baseResistance = 0f;
    public float currentResistance = 0f;

    public float currentEmcumbrance = 0f;
    public float maxEmcumbrance = 0f;


    public int charExperiance = 0;
    public int charLevel = 0;

    public CharLevelUps[] charLevels;

    #endregion

    #region Stat Increasers
    public void ApplyHealth(int healthAmount)
    {
        if((currentHealth + healthAmount) > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += healthAmount;
        }

        if (isHero)
        {
            OnHeroGainedHealth.Invoke(healthAmount);
        }
    }

    public void ApplyMana(int manaAmount)
    {
        if((currentMana + manaAmount) > maxMana)
        {
            currentMana = maxMana;
        }
        else
        {
            currentMana += manaAmount;
        }
    }

    public void GiveWealth(int wealthAmount)
    {
        if((currentWealth + wealthAmount) > maxWealth)
        {
            currentWealth = maxWealth;
        }
        else
        {
            currentWealth += wealthAmount;
        }
    }
    
    public void GiveXp(int xp)
    {
        charExperiance += xp;
        if(charLevel < charLevels.Length)
        {
            int levelTarget = charLevels[charLevel].requiredXP;

            if (charExperiance >= levelTarget)
                SetCharacterLevel(charLevel);
        }
    }

    public void EquipWeapon(ItemPickUp weaponPickUp, CharacterInventory charInv, GameObject weaponSlot)
    {
        Rigidbody newWeapon;
        weapon = weaponPickUp;
        charInv.inventoryDisplaySlots[0].sprite = weaponPickUp.itemDefination.itemIcon;
        newWeapon = Instantiate(weaponPickUp.itemDefination.WeaponSlotObject.weaponPrifab, weaponSlot.transform);
        weapon.itemDefination.isEquipped = true;
        currentDamage = baseDameage + weapon.itemDefination.itemAmount;
    }

    public void EquipArmor(ItemPickUp armorPickup, CharacterInventory charInv)
    {
        switch (armorPickup.itemDefination.itemArmorType)
        {
            case ItemArmorSubType.Head:
                headArmor = armorPickup;
                currentResistance += armorPickup.itemDefination.itemAmount;
                break;
            case ItemArmorSubType.Chest:
                chestArmor = armorPickup;
                currentResistance += armorPickup.itemDefination.itemAmount;
                break;
            case ItemArmorSubType.Hand:
                handArmor = armorPickup;
                currentResistance += armorPickup.itemDefination.itemAmount;
                break;
            case ItemArmorSubType.Legs:
                legArmor = armorPickup;
                currentResistance += armorPickup.itemDefination.itemAmount;
                break;
            case ItemArmorSubType.Boots:
                bootsArmor = armorPickup;
                currentResistance += armorPickup.itemDefination.itemAmount;
                break;


        }
    }
    #endregion

    #region Stat Decreasers

    public void TakeDamage(int amountDamage)
    {
        currentHealth -= amountDamage;

        if (isHero)
        {
            OnHeroDamaged.Invoke(amountDamage);
        }

        if(currentHealth <= 0)
        {
            Death();
        }
    }

    public void TakeMana(int amount)
    {
        currentWealth -= amount;
        if(currentMana < 0)
        {
            currentMana = 0;
        }
    }

    public void TakeWealth(int amount)
    {
        currentResistance -= amount;
        if(currentWealth < 0)
        {
            currentWealth = 0;
        }
    }

    public bool UnEquipeWeapon(ItemPickUp weaponPickUp, CharacterInventory charInv, GameObject weaponSlot)
    {
        bool priviousWaponSame = false;

        if(weapon != null)
        {
            if(weapon == weaponPickUp)
            {
                priviousWaponSame = true;
            }
            
            DestroyObject(weaponSlot.transform.GetChild(0).gameObject);
            weapon.itemDefination.isEquipped = false;
            weapon = null;
            currentDamage = baseDameage;
        }
        return priviousWaponSame;
    }

    public bool UnEquipArmor(ItemPickUp armorPickUp, CharacterInventory cahrInv)
    {
        bool priviousArmorSame = false;

        switch (armorPickUp.itemDefination.itemArmorType)
        {
            case ItemArmorSubType.Head:
                if(headArmor != null)
                {
                    if(headArmor == armorPickUp)
                    {
                        priviousArmorSame = true;
                    }
                    currentResistance -= armorPickUp.itemDefination.itemAmount;
                    headArmor = null;
                }
                break;
            case ItemArmorSubType.Chest:
                if (chestArmor != null)
                {
                    if (chestArmor == armorPickUp)
                    {
                        priviousArmorSame = true;
                    }
                    currentResistance -= armorPickUp.itemDefination.itemAmount;
                    chestArmor = null;
                }
                break;
            case ItemArmorSubType.Hand:
                if (handArmor != null)
                {
                    if (handArmor == armorPickUp)
                    {
                        priviousArmorSame = true;
                    }
                    currentResistance -= armorPickUp.itemDefination.itemAmount;
                    handArmor = null;
                }
                break;
            case ItemArmorSubType.Legs:
                if (legArmor != null)
                {
                    if (legArmor == armorPickUp)
                    {
                        priviousArmorSame = true;
                    }
                    currentResistance -= armorPickUp.itemDefination.itemAmount;
                    legArmor = null;
                }
                break;
            case ItemArmorSubType.Boots:
                if (bootsArmor != null)
                {
                    if (bootsArmor == armorPickUp)
                    {
                        priviousArmorSame = true;
                    }
                    currentResistance -= armorPickUp.itemDefination.itemAmount;
                    bootsArmor = null;
                }
                break;
        }
        return priviousArmorSame;
    }

    #endregion

    #region Character Level Up and Deth

    private void Death()
    {
        if (isHero)
        {
            OnHeroDeath.Invoke();
        }
    }

    public void SetCharacterLevel(int newLevel)
    {
        charLevel = newLevel + 1;
        //display level up visualization

        maxHealth = charLevels[newLevel].maxHealth;
        currentHealth = charLevels[newLevel].maxHealth;

        maxMana = charLevels[newLevel].maxMana;
        currentMana = charLevels[newLevel].maxMana;

        maxWealth = charLevels[newLevel].maxWealth;

        baseDameage = charLevels[newLevel].baseDamage;

        if(weapon == null)
        {
            currentDamage = charLevels[newLevel].baseDamage;
        }
        else
        {
            currentDamage = charLevels[newLevel].baseDamage + weapon.itemDefination.itemAmount;
        }
        

        baseResistance = charLevels[newLevel].baseResistance;
        currentResistance = charLevels[newLevel].baseResistance;

        maxEmcumbrance = charLevels[newLevel].maxEncumbrance;

        if(charLevel > 1)
        {
            OnLevelUp.Invoke(charLevel);
        }
    }
    #endregion

    #region SaveCharacterData
    public void saveCharacterData()
    {
       // saveDataOnClose = true;
      //  EditorUtility.SetDirty(this);
    }
    #endregion

}
