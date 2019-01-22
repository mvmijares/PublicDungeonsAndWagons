using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class InventoryInterface : UserInterface {

    #region Data 
    [SerializeField] ImageLibrary _imageLibrary;

    [SerializeField] private GridLayoutGroup gridGroup;
    public GameObject buttonTemplate;

    [SerializeField] List<ItemButton> inventoryButtons;
    public GameObject interfaceObject;

    bool _windowStatus; // is the inventory window currently open?
    public bool windowStatus { get { return _windowStatus; } }

    // Delete items at a specific slot location.
    public event Action<int> DeleteItemEvent;

    #endregion

    public override void InitializeUserInterface(GameManager gameManager, UserInterfaceManager userInterfaceManager) {
        base.InitializeUserInterface(gameManager, userInterfaceManager);
        _imageLibrary = _gameManager.imageLibrary;
        _windowStatus = false;
        DisplayInventoryWindow(false);
        inventoryButtons = new List<ItemButton>();
        GenerateInventoryScreen(_gameManager.player.inventory);
    }
    /// <summary>
    /// Generates the inventory before loading
    /// </summary>
    /// <param name="inventory"></param>
    public void GenerateInventoryScreen(Inventory inventory) {
        int slot = 1;
        foreach (Item i in inventory.inventory) {
            inventoryButtons.Add(CreateNewItemButton(_imageLibrary.GetSpriteReference(i.itemName), slot));
            slot++;
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
            inventoryButtons.Add(CreateNewItemButton(_imageLibrary.GetSpriteReference(i.itemName), slot));
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
    public void DisplayInventoryWindow(bool condition) {
        interfaceObject.SetActive(condition);
        _windowStatus = condition;
    }
    /// <summary>
    /// Add buttons, that reference inventory items,to the user interface.
    /// </summary>
    /// <param name="inventory"></param>
    /// <param name="item"></param>
    public void AddItemToInventoryInterface(Inventory inventory, Item item) {
        int slot = inventory.currentSize + 1;
        inventoryButtons.Add(CreateNewItemButton(_imageLibrary.GetSpriteReference(item.itemName), slot));
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

        newButton.transform.SetParent(buttonTemplate.transform.parent, false);

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
        _gameManager.player.inventory.DeleteItemSlot(slot);
        inventoryButtons.Remove(iButton);
        UpdateSlots();
        iButton.ItemOnRightClickEvent -= ItemDeleteWasCalled;
        Destroy(iButton.gameObject);
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
