﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using UserInterfaceStates;

public class InventoryInterface : UserInterface {

    #region Data 
    [SerializeField] private GridLayoutGroup gridGroup;
    public GameObject buttonTemplate;
    [SerializeField] private GameObject inventoryContent = null; //Where items in the inventory will be placed
    [SerializeField] List<ItemButton> inventoryButtons;

    [SerializeField] List<Transform> inventoryPages;
    [SerializeField] int lastPageIndex = 0;
    // Delete items at a specific slot location.
    public event Action<int> DeleteItemEvent;

    #endregion

    public override void InitializeUserInterface(GameManager gameManager, UserInterfaceManager userInterfaceManager) {
        base.InitializeUserInterface(gameManager, userInterfaceManager);

        inventoryPages = new List<Transform>();
        Transform[] children = interfaceObject.GetComponentsInChildren<Transform>();
        foreach(Transform t in children) {
            if(t.name == "Inventory Content") {
                inventoryContent = t.gameObject;
            }
            if (t.tag == "Page") {
                inventoryPages.Add(t);
                if (t.GetSiblingIndex() > lastPageIndex)
                    lastPageIndex = t.GetSiblingIndex();
            }
        }
        if (inventoryContent == null)
            Debug.Log("There is no scroll view in inventory interface");

        _windowStatus = false;
        inventoryButtons = new List<ItemButton>();
        GenerateInventoryScreen(_gameManager.level.player.inventory);
        EnableUserInterface(false);
    }
    /// <summary>
    /// Generates the inventory before loading
    /// </summary>
    /// <param name="inventory"></param>
    public void GenerateInventoryScreen(Inventory inventory) {
        int slot = 0;
        foreach (Item i in inventory.inventory) {
            inventoryButtons.Add(CreateNewItemButton(_gameManager.imageLibrary.GetSpriteReference(i.itemName), slot));
            slot++;
        }
    }
    public void SwitchPage(InventoryPageState state) {
        foreach(Transform t in inventoryPages) {
            if(t.name == state + " Page") {
                t.SetSiblingIndex(lastPageIndex);
            }
        }
    }
    /// <summary>
    /// Called whenever we want to update the inventory.
    /// </summary>
    /// <param name="inventory"></param>
    public void UpdateInventoryScreen(Inventory inventory) {
        int slot = 1;
        ClearInventory();
        foreach(Item i in inventory.inventory) {
            inventoryButtons.Add(CreateNewItemButton(_gameManager.imageLibrary.GetSpriteReference(i.itemName), slot));
            slot++;
        }
    }
    /// <summary>
    /// Used to reset the inventory to a fresh state before updating.
    /// </summary>
    void ClearInventory() {
        foreach(ItemButton inventoryButton in inventoryButtons) {
            Destroy(inventoryButton.gameObject);
        }
        inventoryButtons.Clear();
    }

    public bool GetWindowStatus() {
        return interfaceObject.activeSelf;
    }
    public override void OnEscapeKeyPressed() {
        base.OnEscapeKeyPressed();
        EnableUserInterface(false);
    }
    public void DisplayInventoryWindow(bool condition) {
        interfaceObject.SetActive(condition);
        _windowStatus = condition;
    }
    /// <summary>
    /// Add buttons, that reference inventory items,to the user interface.
    /// </summary>
    /// <param name="inventory"></param>
    /// <param name="item"></param>
    public void AddItemToInventoryInterface(Item item, int slot) {
        ItemButton newButton = CreateNewItemButton(_gameManager.imageLibrary.GetSpriteReference(item.itemName), slot);
        if(newButton)
            inventoryButtons.Add(newButton);
    }
    /// <summary>
    /// Creates a new button 
    /// </summary>
    /// <param name="sprite">Sprite for button</param>
    /// <param name="slot">Reference to slot number</param>
    ItemButton CreateNewItemButton(Sprite sprite, int slot) {
        ItemButton newButton = Instantiate(buttonTemplate).GetComponent<ItemButton>();

        newButton.gameObject.SetActive(true);
        newButton.InitializeItemButton(_gameManager, _canvas);
        newButton.SetIcon(sprite);
        newButton.SetInventorySlot(slot);
        newButton.ItemOnRightClickEvent += ItemDeleteWasCalled;

        newButton.transform.SetParent(inventoryContent.transform, false);

        return newButton;
    }
    /// <summary>
    /// Function to find a specific button.
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    ItemButton GetItemButton(int slot) {
        foreach (ItemButton button in inventoryButtons) {
            if (button.invSlot == slot) {
                return button;
            }
        }
        return null;
    }

    //Event handling on individual buttons
    //Deregister after all logic is handle, then destroy button
    public void ItemDeleteWasCalled(ItemButton iButton, int slot) {
        _gameManager.level.player.inventory.DeleteItemSlot(slot);
        inventoryButtons.Remove(iButton);
        UpdateSlots();
        iButton.ItemOnRightClickEvent -= ItemDeleteWasCalled;
        Destroy(iButton.gameObject);
    }
    public void DeleteItemEventCalled(int slot) {
        if (GetItemButton(slot)) {
            Destroy(GetItemButton(slot).gameObject);
            UpdateSlots();
        }
    }
    //Update slot numbers after deleting or sorting
    void UpdateSlots() {
        int slot = 1;
        foreach (ItemButton i in inventoryButtons) {
            i.invSlot = slot;
            slot++;
        }
    }
}
