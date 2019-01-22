using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entity;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Item", order = 1)]
public class Item : ScriptableObject {

    public Character owner;
    public GameObject prefab; // prefab of object
    public string itemName; //item name
    public int value; //currency value
    public bool isStackable; //can you stack item
    public int stackMax; //if stackable, check maximum number of stack items
    public string itemDesc; // item description
    private int _itemCount; // how many items
    public int itemCount {
        get { return _itemCount; }
        set {
            if (isStackable && value < stackMax) {
                _itemCount = value;
            } else {
                _itemCount = Mathf.Clamp(_itemCount, 0, 1);
            }
        }
    }

    public Item(Item item) {
        this.owner = item.owner;
        this.prefab = item.prefab;
        this.itemName = item.itemName;
        this.value = item.value;
        this.isStackable = item.isStackable;
        this.stackMax = item.stackMax;
        this.itemDesc = item.itemDesc;
        this._itemCount = itemCount;
    }
    //Items can change during run-time
    public Item GetCopy() {
        return (Item)this.MemberwiseClone();
    }
}
