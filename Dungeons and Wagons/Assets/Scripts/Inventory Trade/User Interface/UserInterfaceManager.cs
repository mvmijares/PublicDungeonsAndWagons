using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using UserInterfaceStates;

/// <summary>
/// Used to dictate which part of the inventory the player is on
/// </summary>

/// <summary>
/// This class handles all logic regarding user interface
/// </summary>
public class UserInterfaceManager : MonoBehaviour {

    #region Data
    GameManager _gameManager;
    private Entity.Player _player;
    private Inventory _wagon;

    [SerializeField]
    private LootInterface _lootInterface;
    public LootInterface lootInterface { get { return _lootInterface; } }

    private InventoryInterface _inventoryInterface;
    public InventoryInterface inventoryInterface { get { return _inventoryInterface; } }
    [SerializeField]
    private PlayerShopInterface _playerShopInterface;
    public PlayerShopInterface playerShopInterface { get { return _playerShopInterface; } }

    private HelpInterface _helpInterface;
    public HelpInterface helpInterface { get { return _helpInterface; } }

    private StackInterface _stackInterface;
    public StackInterface stackInterface { get { return _stackInterface; } }

    private SideMenuInterface _sideMenuInterface;
    public SideMenuInterface sideMenuInterface { get { return _sideMenuInterface; } }

    [SerializeField]
    private List<Button> inventoryTabs;

    private bool _canDisplaySpeechBubble;
    public bool canDisplaySpeechBubble; //boolean to control displaying the speech bubble.
    private GameObject _dragObject; //Object we are currently dragging
    public GameObject dragObject { get { return _dragObject; } }
    public bool canDrag; // If we are doing something else.
    public bool calculateItemCount; //Pause inventory state. Must enter a value or close window to reset state.
    /// <summary>
    /// Used for stackable items
    /// </summary>
    public class ItemReference {
        public ItemButton itemButton = null;
        public Item item = null;
        public int slotNum = -1;

        public ItemReference() {
            itemButton = null;
            item = null;
            slotNum = -1;
        }

        public void Clear() {
            itemButton = null;
            item = null;
            slotNum = -1;
        }
    };
    ItemReference itemReference = null;

    [SerializeField]
    private InventoryState _state;
    public InventoryState state { get { return _state; } }
    [SerializeField]
    UserInterface[] userInterfaces;
    #endregion
    /// <summary>
    /// Used to initialize user interface objects.
    /// Function is called within the game manager to organize execution order.
    /// </summary>
    public void InitializeInterfaces(GameManager gameManager) {
        _gameManager = gameManager;
        _player = _gameManager.player;

        _inventoryInterface = FindObjectOfType<InventoryInterface>();
        _sideMenuInterface = FindObjectOfType<SideMenuInterface>();
        _playerShopInterface = FindObjectOfType<PlayerShopInterface>();
        _lootInterface = FindObjectOfType<LootInterface>();
        _helpInterface = FindObjectOfType<HelpInterface>();
        _stackInterface = FindObjectOfType<StackInterface>();

        userInterfaces = FindObjectsOfType<UserInterface>();

        foreach (UserInterface userInterface in userInterfaces) {
            userInterface.InitializeUserInterface(_gameManager, this);
        }
  
        foreach (Button button in inventoryTabs) {
    
            button.onClick.AddListener(() => SwitchInventoryScreen(button));
            if (button.name == "Shop") {
                button.interactable = false;
                playerShopInterface.SetInventoryTabButton(button);
            }
        }

        _state = InventoryState.None;
        canDrag = true;
        itemReference = new ItemReference();
        RegisterEvents();


    }
    private void OnDestroy() {
        DeregisterEvents(); // Remember to call this for event destruction
    }
    /// <summary>
    /// Register events for the user interface
    /// </summary>
    private void RegisterEvents() {
        if (_gameManager.playerShop) {
            _gameManager.playerShop.OnPlayerShopEvent += _helpInterface.OnPlayerShopEventCalled;
            _gameManager.playerShop.OnPlayerShopEvent += _playerShopInterface.OnPlayerShopEventCalled;
        }

        if (_gameManager.player) {
            _gameManager.player.inventory.AddItemEvent += _inventoryInterface.AddItemToInventoryInterface;
        }
    }
    /// <summary>
    /// Deregister events for the user interface
    /// </summary>
    private void DeregisterEvents() {
        if (_gameManager.playerShop) {
            _gameManager.playerShop.OnPlayerShopEvent -= _helpInterface.OnPlayerShopEventCalled;
            _gameManager.playerShop.OnPlayerShopEvent -= _playerShopInterface.OnPlayerShopEventCalled;
        }

        if (_gameManager.player) {
            _gameManager.player.inventory.AddItemEvent -= _inventoryInterface.AddItemToInventoryInterface;
        }
    }
    public void AccessLootInterface(Loot inventory) {
        lootInterface.GenerateLootInterface(inventory);
        lootInterface.UpdateinterfaceSlots();
    }
    public void AccessShopWindowInterface() {
        _playerShopInterface.GenerateShopScreen();
        _sideMenuInterface.DisplaySideMenuWindow(true);
        _state = InventoryState.Shop;
        canDisplaySpeechBubble = false;
    }
    public void SwitchInventoryScreen(InventoryState state) {
        _state = state;
    }
    void SwitchInventoryScreen(Button button) {
        string buttonName = button.gameObject.name;
        switch (buttonName) {
            case "Player Inventory": {
                    _state = InventoryState.Inventory;
                    break;
                }
            case "Shop": {
                    _state = InventoryState.Shop;
                    AccessShopWindowInterface();
                    break;
                }
            case "Map": {
                    _state = InventoryState.Map;
                    break;
                }
            case "Wagon": {
                    _state = InventoryState.Wagon;
                    break;
                }
            default: {
                    Debug.Log("Button does not reference a inventory state.");
                    break;
                }
        }
        Debug.Log("Inventory is in " + _state.ToString() + " state.");
    }
    /// <summary>
    /// Sets the drag object for inventory
    /// </summary>
    /// <param name="itemButton"></param>
    public void SetDragObject(GameObject itemButton) {
        if (_dragObject == null)
            _dragObject = itemButton;
    }
    /// <summary>
    /// Function called when the user releases the left click.
    /// Also checks what state we are in to determine extra
    /// functionality
    /// </summary>
    public void EndDragWasCalled() {
        if (_state == InventoryState.Shop) {
            _playerShopInterface.EndDragWasCalled();
        }
        if (_dragObject != null) 
            _dragObject = null;
    }
    /// <summary>
    /// Menu for splitting up stack
    /// </summary>
    /// <param name="newItem"></param>
    /// <param name="itemButton"></param>
    /// <param name="destinationSlot"></param>
    public void EnableStackInterface(Item newItem, ItemButton itemButton, int destinationSlot) {
        canDrag = false;
        stackInterface.DisplayStackWindow(true);
        calculateItemCount = true;

        itemReference.itemButton = itemButton;
        itemReference.item = newItem;
        itemReference.slotNum = destinationSlot;
    }

    /// <summary>
    /// Calculates new item value from stack
    /// </summary>
    /// <param name="value"></param>
    public void CalculateNewItemCount(int value) {
        if (value > 0) {
            if (itemReference.item.itemCount - value == 0) {
                _gameManager.DeleteItemFromPlayerInventory(itemReference.itemButton.invSlot);
            }
            itemReference.item.itemCount = value;
            _gameManager.playerShop.AddItemToShopInventory(itemReference.item, itemReference.slotNum);
            _playerShopInterface.DisplayNewButton(itemReference.itemButton);

            stackInterface.DisplayStackWindow(false);
            calculateItemCount = false;
            canDrag = true;
            itemReference.Clear();
        }
    }
    public void DisplayInventoryWindow(bool condition) {
        if (condition) {
            inventoryInterface.DisplayInventoryWindow(condition);
            canDisplaySpeechBubble = false;
        } else {
            if (inventoryInterface.GetWindowStatus())
                inventoryInterface.DisplayInventoryWindow(condition);
            if (stackInterface.GetWindowStatus())
                stackInterface.DisplayStackWindow(condition);
            if (sideMenuInterface.GetWindowStatus())
                sideMenuInterface.DisplaySideMenuWindow(condition);

            canDisplaySpeechBubble = true;
            ResetInventoryVariables();
        }
    }
    void ResetInventoryVariables() {
        canDrag = true;
        calculateItemCount = false;
        _dragObject = null;
        if(itemReference != null)
            itemReference.Clear();
    }
}
   
