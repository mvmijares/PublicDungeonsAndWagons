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

    [SerializeField] private LootInterface _lootInterface;
    public LootInterface lootInterface { get { return _lootInterface; } }

    [SerializeField] private InventoryInterface _inventoryInterface;
    public InventoryInterface inventoryInterface { get { return _inventoryInterface; } }

    [SerializeField] private PlayerShopInterface _playerShopInterface;
    public PlayerShopInterface playerShopInterface { get { return _playerShopInterface; } }

    [SerializeField] private HelpInterface _helpInterface;
    public HelpInterface helpInterface { get { return _helpInterface; } }

    [SerializeField] private StackInterface _stackInterface;
    public StackInterface stackInterface { get { return _stackInterface; } }

    [SerializeField] private SideMenuInterface _sideMenuInterface;
    public SideMenuInterface sideMenuInterface { get { return _sideMenuInterface; } }

    [SerializeField] private HealthBarInterface _healthBarInterface;
    public HealthBarInterface healthBarInterface { get { return _healthBarInterface; } }

    public Transform inventoryPageButtons;
    [SerializeField] private Transform bagPage;
    [SerializeField] private Transform wagonPage;
    [SerializeField] private Transform shopPage;
    [SerializeField] private Transform mapPage;
    [SerializeField] private Transform systemPage;


    public UserInterface currentInterface; // current interface we are using

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
    private bool _shopInteractable = false;
    public bool shopInteractable { get { return _shopInteractable; } }
    [SerializeField]
    UserInterface[] userInterfaces;
    #endregion
    /// <summary>
    /// Used to initialize user interface objects.
    /// Function is called within the game manager to organize execution order.
    /// </summary>
    public void InitializeInterfaces(GameManager gameManager) {
        _gameManager = gameManager;

        _inventoryInterface = FindObjectOfType<InventoryInterface>();
        _sideMenuInterface = FindObjectOfType<SideMenuInterface>();
        _playerShopInterface = FindObjectOfType<PlayerShopInterface>();
        _lootInterface = FindObjectOfType<LootInterface>();
        _helpInterface = FindObjectOfType<HelpInterface>();
        _stackInterface = FindObjectOfType<StackInterface>();
        _healthBarInterface = FindObjectOfType<HealthBarInterface>();

        //TODO: Redudant, will have to change initialization
        userInterfaces = FindObjectsOfType<UserInterface>();

        foreach (UserInterface userInterface in userInterfaces) {
            userInterface.InitializeUserInterface(_gameManager, this);
        }
        if (inventoryPageButtons) {
            Transform[] children = inventoryPageButtons.GetComponentsInChildren<Transform>();

            foreach (Transform c in children) {
                if (c.gameObject.layer == LayerMask.NameToLayer("UI")) {
                    switch (c.tag) {
                        case "Bag": {
                                bagPage = c;
                                bagPage.GetComponent<InventoryPage>().InitializeInventoryPage(this);
                                break;
                            }
                        case "Wagon": {
                                wagonPage = c;
                                wagonPage.GetComponent<InventoryPage>().InitializeInventoryPage(this);
                                break;
                            }
                        case "Shop": {
                                shopPage = c;
                                shopPage.GetComponent<InventoryPage>().InitializeInventoryPage(this);
                                break;
                            }
                        case "Map": {
                                mapPage = c;
                                mapPage.GetComponent<InventoryPage>().InitializeInventoryPage(this);
                                break;
                            }
                        case "System": {
                                systemPage = c;
                                systemPage.GetComponent<InventoryPage>().InitializeInventoryPage(this);
                                break;
                            }
                    }
                }
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
    
    private void Update() {
        if (_state == InventoryState.None)
            canDisplaySpeechBubble = true;
        else
            canDisplaySpeechBubble = false;
    }
    /// <summary>
    /// Register events for the user interface
    /// </summary>
    private void RegisterEvents() {
        if (_gameManager.level.player) {
            _gameManager.level.player.playerInput.OnAccessInventoryEvent += _inventoryInterface.EnableUserInterface;
            _gameManager.level.player.playerInput.OnAccessInventoryEvent += OnAccessInventoryEventCalled;
            _gameManager.level.player.inventory.AddItemEvent += _inventoryInterface.AddItemToInventoryInterface;
            _gameManager.level.player.inventory.DeleteItemEvent += _inventoryInterface.DeleteItemEventCalled;
        }
    }

    /// <summary>
    /// Deregister events for the user interface
    /// </summary>
    private void DeregisterEvents() {
        if (_gameManager.level.player) {
            _gameManager.level.player.playerInput.OnAccessInventoryEvent -= _inventoryInterface.EnableUserInterface;
            _gameManager.level.player.playerInput.OnAccessInventoryEvent -= OnAccessInventoryEventCalled;
            _gameManager.level.player.inventory.DeleteItemEvent -= _inventoryInterface.DeleteItemEventCalled;
        }
        foreach (UserInterface u in userInterfaces) {
            u.DeregisterEvents();
        }
    }
    public void OnPlayerShopEventCalled(PlayerShopData data) {
        _shopInteractable = data._playerWithinBox;
    }
    public void OnAccessInventoryEventCalled(bool condition) {
        if (condition) {
            currentInterface = _inventoryInterface; // default interface to be called
            SwitchInventoryScreen(InventoryPageState.Bag);
        } else
            currentInterface = null; 
    }
    /// <summary>
    /// Function to handle logic for switching inventory menus
    /// </summary>
    /// <param name="page"></param>
    public void OnSwitchPage(InventoryPageState page) {
        if(page == InventoryPageState.Shop) {
            if(shopInteractable)
                SwitchInventoryScreen(page);
        } else {
            SwitchInventoryScreen(page);
        }
    }
    /// <summary>
    /// Function to handle switching user interface states in the inventory
    /// </summary>
    /// <param name="page"></param>
    void SwitchInventoryScreen(InventoryPageState page) {
        currentInterface.CloseInterface();
        switch (page) {
            case InventoryPageState.Bag: {
                    currentInterface = _inventoryInterface;
                    _state = InventoryState.Inventory;
                    break;
                }
            case InventoryPageState.Shop: {
                    currentInterface = _playerShopInterface;
                    _state = InventoryState.Shop;
                    break;
                }
            case InventoryPageState.Map: {
                    _state = InventoryState.Map;
                    break;
                }
            case InventoryPageState.Wagon: {
                    _state = InventoryState.Wagon;
                    break;
                }
            case InventoryPageState.System: {
                    _state = InventoryState.System;
                    break;
                }
            default: {
                    Debug.Log("Button does not reference a inventory state.");
                    break;
                }
        }
        inventoryInterface.SwitchPage(page);
        currentInterface.AccessInterface();

        if(_state == InventoryState.Shop)
            _sideMenuInterface.DisplaySideMenuWindow(true);
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
                //TODO: Delete Item from player Inventory after item.value is 0
                //_gameManager.DeleteItemFromPlayerInventory(itemReference.itemButton.invSlot);
            }
            itemReference.item.itemCount = value;
            ((TownLevel)_gameManager.level).playerShop.AddItemToShopInventory(itemReference.item, itemReference.slotNum);
            _playerShopInterface.DisplayNewButton(itemReference.itemButton);

            stackInterface.DisplayStackWindow(false);
            calculateItemCount = false;
            canDrag = true;
            itemReference.Clear();
        }
    }
    /// <summary>
    /// Move logic to displaying inventory window to inventory user interface
    /// </summary>
    /// <param name="condition"></param>
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
   
