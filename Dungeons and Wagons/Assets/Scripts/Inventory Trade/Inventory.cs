using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entity;
[Serializable]
public class ItemData {
    public string itemName;
    public int itemCount;
}

//Base class for inventories
//Custom classes will inherit from this class
public class Inventory : MonoBehaviour {
    #region Data
    protected Character _character;
    public Character character { get { return _character; } }

    public int size = 6;
    public int currentSize;
    public List<Item> inventory;
    public bool full;

    [SerializeField]
    protected ItemDatabase itemDatabase;
    protected GameManager _gameManager;

    public List<ItemData> loadInventory; //items that players start with
    #endregion

    public event Action<Item, int> AddItemEvent;
    public event Action<int> DeleteItemEvent;
    public virtual void InitializeInventory(GameManager gameManager, Character character) {
        _character = character;
        _gameManager = gameManager;
        inventory = new List<Item>();
    }

    public virtual void Update() {
        currentSize = inventory.Count;
        if (currentSize >= size)
            full = true;
        else
            full = false;
    }

    //Load inventory at start up
    public virtual void LoadInventory() {
        if (loadInventory.Count > 0) {
            foreach (ItemData data in loadInventory) {
                if (_gameManager.itemDatabase.GetItem(data.itemName)) {
                    Item newItem = _gameManager.itemDatabase.GetItem(data.itemName).GetCopy();
                    newItem.itemCount = data.itemCount;
                    inventory.Add(newItem);
                }
            }
        } 
    }
    //Add items and also check if we can stack 
    public virtual void AddItem(Item item) {
        if (!full) {
            //Check if item is stackable
            if (item.isStackable) {
                if (!CheckItem(item)) {
                    inventory.Add(item);
                    AddItemEventCall(item, inventory.Count - 1);
                }
            } else {
                inventory.Add(item);
                AddItemEventCall(item, inventory.Count - 1);
            }
        } else {
            Debug.Log("There is no more room in inventory");
        }
    }
    //Event Calling
    void AddItemEventCall(Item item, int slot) {
        if (AddItemEvent != null)
            AddItemEvent(item, slot);
    }
    
    /// <summary>
    /// returns the item at the slot index
    /// </summary>
    /// <param name="slot"> slot uses a greater than 1 reference, but lists start from 0 index.</param>
    /// <returns></returns>
    public Item GetItem(int slot) {
        return inventory[slot - 1];
    }
    //Delete event handled by button
    //Registered event through game manager
    public virtual void DeleteItemSlot(int slot) {
        if(inventory.Count > 0) {
            Item reference = inventory[slot];
            if (DeleteItemEvent != null)
                DeleteItemEvent(slot);

            inventory.RemoveAt(slot);
        }
    }
    /// <summary>
    /// Checks if we have a smiliar item in our inventory.
    /// </summary>
    /// <param name="item">reference to item to be added</param>
    /// <returns></returns>
    bool CheckItem(Item item) {
        foreach (Item i in inventory) {
            if(i.name == item.name) {
                int total = i.itemCount + item.itemCount;
                if(total < i.stackMax) {
                    i.itemCount = total;
                } else {
                    int difference = total - i.stackMax;
                    item.itemCount = difference;
                    i.itemCount = i.stackMax;
                }
                return true;
            }
        }
        return false;
    }
}
