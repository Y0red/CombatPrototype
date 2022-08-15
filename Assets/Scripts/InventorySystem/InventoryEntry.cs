using UnityEngine;

public class InventoryEntry
{
    public ItemPickUp invEntry;
    public int stackSize ;
    //bool isStackable;
    public Sprite hbSprite;

    public int hotBarSlot = 3;
    //public int hotBarslot;
    public int inventorySlot;
    

    public InventoryEntry(int stack, ItemPickUp p1, Sprite p2)
    {
        this.stackSize = stack;
        this.invEntry = p1;
        this.hbSprite = p2;
    }
}