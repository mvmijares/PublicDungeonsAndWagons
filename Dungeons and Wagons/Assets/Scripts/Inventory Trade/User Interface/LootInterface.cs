using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
///  This class handles functionality for loot drops within a menu
///  TODO   - Maybe, make a parent class that loot user interface inherits from
///         - Inventory User interface has the same functionality
/// </summary>
/// 
public class LootInterface : UserInterface {
    #region Data
    [SerializeField]
    List<ItemButton> itemButtons;
    public Button closeWindowButton;
    //TODO make a button template prefab that we load from 
    public GameObject buttonTemplate;

    public event Action<int> TakeItemInterfaceEvent; // We take item instead of trashing it
    #endregion

    public override void InitializeUserInterface(GameManager gameManager, UserInterfaceManager userInterfaceManager) {
        base.InitializeUserInterface(gameManager, userInterfaceManager);
        _gameManager = gameManager;
        itemButtons = new List<ItemButton>();
        closeWindowButton.onClick.AddListener(OnCloseWindowButtonCalled);
        EnableUserInterface(false);
    }
    
    public void RegisterEventForLootItem(Loot loot) {
        loot.AccessLootInterfaceEvent += OpenLootWindow;
    }
    
    public void DeregisterEventForLootItem(Loot loot) {
        loot.AccessLootInterfaceEvent -= OpenLootWindow;
    }
    public override void EnableUserInterface(bool condition) {
        base.EnableUserInterface(condition);
        if (!condition) {
            CloseLootWindow();
        }
    }
    public void OpenLootWindow(Loot loot) {
        int slot = 1;
        if (loot.inventory.Count > 0) {
            foreach (Item i in loot.inventory) {
                itemButtons.Add(CreateNewItemButton(_gameManager.imageLibrary.GetSpriteReference(i.itemName), slot));
                slot++;
            }
        }
        EnableUserInterface(true);
    }
    void OnCloseWindowButtonCalled() {
        EnableUserInterface(false);
    }
 
    void CloseLootWindow() {
        foreach (ItemButton iButton in itemButtons) {
            iButton.ItemOnRightClickEvent -= ItemRightClickEvent;
            Destroy(iButton.gameObject);
        }
        itemButtons.Clear();
    }
    /// <summary>
    /// Creates a new button 
    /// </summary>
    /// <param name="sprite">Sprite for button</param>
    /// <param name="slot">Reference to slot number</param>
    ItemButton CreateNewItemButton(Sprite sprite, int slot) {
        GameObject newButton = Instantiate(buttonTemplate) as GameObject;
        newButton.SetActive(true);
        newButton.GetComponent<ItemButton>().SetIcon(sprite);
        newButton.GetComponent<ItemButton>().SetInventorySlot(slot);
        newButton.GetComponent<ItemButton>().ItemOnRightClickEvent += ItemRightClickEvent;
        newButton.transform.SetParent(buttonTemplate.transform.parent, false);

        return newButton.GetComponent<ItemButton>();
    }
    public void ItemRightClickEvent(ItemButton iButton, int slot) {
        itemButtons.Remove(iButton);
        Destroy(iButton.gameObject);
        iButton.ItemOnRightClickEvent -= ItemRightClickEvent;
        //Add Item from Loot to Player
        //gameManager.AddItemFromLootInterface(slot);
    }

    public void UpdateinterfaceSlots() {
        int slot = 1;
        foreach (ItemButton i in itemButtons) {
            i.invSlot = slot;
            slot++;
        }
    }

}
