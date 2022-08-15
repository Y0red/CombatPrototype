using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInventory : MonoBehaviour
{
    #region variable Decolarations
    public static CharacterInventory instance;

    public CharacterStats charStats;

    public Image[] hotBarDisplayHolders = new Image[3];

    //public GameObject InventoryDisplaySlot;

    public GameObject InventoryDisplayHolder;

    public Image[] inventoryDisplaySlots ;

   
    GameObject foundStats;

    private bool addedItem = true;

    public Dictionary<int, InventoryEntry> itemsInInventory;
    
    InventoryEntry itemEntry;
    private int inventoryItemCap = 20;
    private int idCount = 1;

    private InputController input;
    #endregion

    #region initialization

    private void Start()
    {
        instance = this;
        itemsInInventory = new Dictionary<int, InventoryEntry>();
        itemEntry = new InventoryEntry(0,null,null);
        //itemEntry = new InventoryEntry(itemEntry.stackSize, itemEntry.invEntry, itemEntry.hbSprite);
        //itemsInInventory.Add(idCount, new InventoryEntry(itemEntry.stackSize, Instantiate(itemEntry.invEntry), itemEntry.hbSprite));
       
        itemsInInventory.Clear();

        inventoryDisplaySlots = InventoryDisplayHolder.GetComponentsInChildren<Image>();

        foundStats = GameObject.FindGameObjectWithTag("Player");

        input = GetComponentInParent<InputController>();
    }
    #endregion

    private void Update()
    {
        #region Watch for Hotbar Keypresses caled by character controller later
        //cheking for a hotbar key to be pressed
        //TODO: add some keypreses

    
        DoHotBarActions();
        #endregion

        //chek to see if the item has already been added - prevent duplicate adds for 1 item
        if (!addedItem)
        {

            TryPickUP(); 
        }
    }
    public void SwordOut()
    {
        TriggerItemUse(0);
    }
    private void DoHotBarActions()
    {

        if (input.alph1)
        {
            TriggerItemUse(0);
        }
        if (input.alph2)
        {
            TriggerItemUse(1);
        }
        if (input.alph3)
        {
            TriggerItemUse(2);
        }
        if (input.alph4)
        {
            TriggerItemUse(3);
        }
        //  if (Input.GetKeyDown(KeyCode.I))
      //  {
           // DisplayInventory();
        //}
    }

    public void StoreItem(ItemPickUp itemToStore)
    {
        addedItem = false;
        if((charStats.characterDefination.currentEmcumbrance + itemToStore.itemDefination.itemWeight) <= charStats.characterDefination.maxEmcumbrance) 
        {
            itemEntry.invEntry = itemToStore;
            itemEntry.stackSize = 1;
            itemEntry.hbSprite = itemToStore.itemDefination.itemIcon;

            addedItem = true;

            itemToStore.gameObject.SetActive(false);
        }
    }

    void TryPickUP()
    {
        bool itsInInv = true;
        //check to see if the item to be stored was properly submitted to the inventory - countinue if
        if (itemEntry.invEntry)
        {
            //chedk to see if any item exists in the inventory already - if not, add this item
            if(itemsInInventory.Count == 0)
            {
                addedItem = AddItemToInv(addedItem);
            }
            //if item exists in inventory
            else
            {
                //check to see if the item is stackable - countinue if stackable
                if (itemEntry.invEntry.itemDefination.isStackable)
                {
                    foreach(KeyValuePair<int, InventoryEntry> ie in itemsInInventory)
                    {
                        //does this item already exists in inventory? continue if yes
                        if(itemEntry.invEntry.itemDefination == ie.Value.invEntry.itemDefination)
                        {
                            //add 1 to stack and destroy the new instance
                            ie.Value.stackSize += 1;
                            AddItemToHotBar(ie.Value);
                            itsInInv = true;
                            DestroyObject(itemEntry.invEntry.gameObject);
                            break;
                        }
                        //if item does not exist already in inventory the conutinue here
                        else
                        {
                            itsInInv = false;
                        }
                    }
                }
                //if item is not stackable then continu here
                else
                {
                    itsInInv = false;

                    // if no space and item is not stackable  say invntory full
                    if(itemsInInventory.Count == inventoryItemCap)
                    {
                        itemEntry.invEntry.gameObject.SetActive(false);
                        Debug.Log("Inventory Full");
                    }
                }
                //check if there is space in inventory - if yes, continue here
                if (!itsInInv)
                {
                    addedItem = AddItemToInv(addedItem);
                    itsInInv = true;
                }
            }
        }
    }

    bool AddItemToInv(bool finishedAdding)
    {
        itemsInInventory.Add(idCount, new InventoryEntry(itemEntry.stackSize, Instantiate(itemEntry.invEntry), itemEntry.hbSprite));

        DestroyObject(itemEntry.invEntry.gameObject);

        //FillInventoryDisplay();
        AddItemToHotBar(itemsInInventory[idCount]);

        idCount = InCreaseID(idCount);

        #region Reset itemEntry

        #endregion

        finishedAdding = true;
        return finishedAdding;
    }

    int InCreaseID(int currentID)
    {
        int newID = 1;
        for(int itemCount = 1; itemCount <= itemsInInventory.Count; itemCount++)
        {
            if (itemsInInventory.ContainsKey(newID))
            {
                newID += 1;
            }
            else return newID;
        }

        return newID;
    }

    private void AddItemToHotBar(InventoryEntry itemForHotBar)
    {
        int hotBarCounter = 0;
        bool increaseCount = false;

        //check for open hotbar slot
        foreach(Image image in hotBarDisplayHolders)
        {
            hotBarCounter += 1;

            if(itemForHotBar.hotBarSlot == 0)
            {
                if(image.sprite == null)
                {
                    //add item to open hotbar slot
                    itemForHotBar.hotBarSlot = hotBarCounter;
                    //change hotbar sprite to show item
                    image.sprite = itemForHotBar.hbSprite;
                    increaseCount = true;
                    break;
                }
               
            }
            else if (itemForHotBar.invEntry.itemDefination.isStackable)
            {
                increaseCount = true;
            }

        }
        if (increaseCount)
        {
            hotBarDisplayHolders[itemForHotBar.hotBarSlot - 1].GetComponentInChildren<Text>().text = itemForHotBar.stackSize.ToString();
        }
        increaseCount = false;
    }

    void DisplayInventory()
    {
        if(InventoryDisplayHolder.activeSelf == false)
        {
            InventoryDisplayHolder.SetActive(true);
        }
        else
        {
            InventoryDisplayHolder.SetActive(false);
        }
    }

    void FillInventoryDisplay()
    {
        int slotCounter = 9;
        foreach (KeyValuePair<int, InventoryEntry> ie in itemsInInventory)
        {
            slotCounter += 1;
            inventoryDisplaySlots[slotCounter].sprite = ie.Value.hbSprite;
            ie.Value.inventorySlot = slotCounter - 9;
        }

        while (slotCounter < 29)
        {
            slotCounter++;
            inventoryDisplaySlots[slotCounter].sprite = null;
        }
    }

    public void TriggerItemUse(int itmeToUseID)
    {
        bool triggerItem = false;

        foreach(KeyValuePair<int, InventoryEntry> ie in itemsInInventory)
        {
            if(itmeToUseID > 100)
            {
                itmeToUseID -= 100;
                
                if(ie.Value.hotBarSlot == itmeToUseID)
                {
                    triggerItem = true;
                }
            }
            else
            {
                if(ie.Value.inventorySlot == itmeToUseID)
                {
                    triggerItem = true;
                }
            }

            if (triggerItem)
            {
                if(ie.Value.hotBarSlot == 1)
                {
                    if (ie.Value.invEntry.itemDefination.isStackable)
                    {
                        if(ie.Value.hotBarSlot != 0)
                        {
                            hotBarDisplayHolders[ie.Value.hotBarSlot - 1].sprite = null;
                            hotBarDisplayHolders[ie.Value.hotBarSlot - 1].GetComponentInChildren<Text>().text = "0x";
                           
                        }

                        ie.Value.invEntry.UseItem();
                        itemsInInventory.Remove(ie.Key);
                        break;
                    }
                    else
                    {
                        ie.Value.invEntry.UseItem();
                        if (!ie.Value.invEntry.itemDefination.isIndestructable)
                        {
                            itemsInInventory.Remove(ie.Key);
                            break;
                        }

                        break;
                    }
                }
                else
                {
                    ie.Value.invEntry.UseItem();
                    ie.Value.stackSize -= 1;
                   //hotBarDisplayHolders[ie.Value.hotBarSlot - 1].GetComponentInChildren<Text>().text = ie.Value.stackSize.ToString();
                    break;
                }
            }
        }

      //  FillInventoryDisplay();
    }
}
