using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using UserInterfaceStates;
using Entity;
using Entity.AI;

/// <summary>
/// Class that contains game logic, and other references to class.
/// </summary>

/// TODO:   Seperate persistent data with per level data
///         Make a Level Manager that handles data that stays within the scene.
///         
/// TODO:   Make Game Manager class handle destruction order. 
///         This is for event handling that needs to be processed in the correct order.
///         
/// TODO:   Make event handling for input presses.

public class GameManager : MonoBehaviour {
    #region Data
    public static GameManager gameManager = null;

    [SerializeField] private ItemDatabase _itemDatabase;
    public ItemDatabase itemDatabase { get { return _itemDatabase; } }

    [SerializeField] private ImageLibrary _imageLibrary;
    public ImageLibrary imageLibrary { get { return _imageLibrary; } }

    [SerializeField] private InputHandler _inputHandler;
    public InputHandler inputHandler { get { return _inputHandler; } }
    private KeyboardAction input;

    [SerializeField] private UserInterfaceManager _userInterfaceManager;
    public UserInterfaceManager userInterfaceManager { get { return _userInterfaceManager; } }

    [SerializeField] private SpeechBubbleManager _speechBubbleManager;
    public SpeechBubbleManager speechBubbleManager { get { return _speechBubbleManager; } }

    [SerializeField] private NPCManager _npcManager;
    public NPCManager npcManager { get { return _npcManager; } }

    [SerializeField] private DialogueManager _dialogueManager;
    public DialogueManager dialogueManager { get { return _dialogueManager; } }

    [SerializeField] private ThirdPersonCamera _cam;
    public ThirdPersonCamera cam { get { return _cam; } }

    [SerializeField] private Player _player;
    public Player player { get { return _player; } }

    [SerializeField] private PlayerShop _playerShop; // reference to the shop once it is setup.
    public PlayerShop playerShop { get { return _playerShop; } }

    private bool _accessingLoot; //Player can only access one loot item at a time.
    public bool accessingLoot { get { return _accessingLoot; } }

    [SerializeField] private Loot _currentLootObject; // reference to current loot object
    public Loot currentLootObject { get { return _currentLootObject; } }

    [SerializeField] private bool _accessingPlayerInv; //Player inventory
    public bool accessingPlayerInv { get { return _accessingPlayerInv; } }

    [SerializeField]
    private bool _accessingNPCInv; //NPC inventory.
    public bool accessingNPCInv { get { return _accessingNPCInv; } }

    
    [Tooltip("Allows debug log calls within run time")]
    public bool allowDebugLog;

    private bool _talkingWithNPC;
    public bool talkingWithNPC { get { return _talkingWithNPC; } }
    private float _mouseZoomVal;
    public float mouseZoomVal { get { return _mouseZoomVal; } }

    //Used for when the player sets up shop
    private bool _setupShop;
    public bool setupShop { get { return _setupShop; } }

    private bool _canMove;
    public bool canMove { get { return _canMove; } }
    
   

    public bool withinShopTrigger; // Use this to figure out if player is within the shop wireframe box.
    //Used to determine if an object is within view of camera
    private Plane[] planes;


    private EnemyManager _enemyManager;
    public EnemyManager enemyManager;

    [SerializeField] private Character selectedObject;
    public Level level;

    #endregion
    
    private void Awake() {
        //singleton
        if(gameManager == null) {
            gameManager = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }

        _imageLibrary = GetComponent<ImageLibrary>();
        _itemDatabase = GetComponent<ItemDatabase>();
        _inputHandler = FindObjectOfType<InputHandler>();
        _imageLibrary.InitializeImageLibrary();
        _itemDatabase.InitializeItemDatabase();
        _inputHandler.InitializeInputHandler(this);

        _player = FindObjectOfType<Player>();
        _npcManager = FindObjectOfType<NPCManager>();
        
        _speechBubbleManager = FindObjectOfType<SpeechBubbleManager>();
        _userInterfaceManager = FindObjectOfType<UserInterfaceManager>();
        _dialogueManager = FindObjectOfType<DialogueManager>();
        _playerShop = FindObjectOfType<PlayerShop>();

        level = FindObjectOfType<Level>();
        level.InitializeLevel(this);

        _cam = Camera.main.GetComponent<ThirdPersonCamera>();
        _cam.InitializeCamera(this);

        if (_inputHandler)
            input = _inputHandler.input;
        else {
            if (allowDebugLog)
                Debug.Log("There is no input handler in the scene!");
        }
        if (_npcManager)
            _npcManager.InitializeNPCManager(this);
        else {
            if (allowDebugLog)
                Debug.Log("There is no NPC Manager in the scene");
        }
        if (_speechBubbleManager)
            _speechBubbleManager.InitializeSpeechBubbleManager(this);
        else {
            if (allowDebugLog)
                Debug.Log("There is no Speech Bubble Manager within the scene!");
        }
        if (_dialogueManager)
            _dialogueManager.InitializeDialogueManager(this);
        else {
            if (allowDebugLog)
                Debug.Log("There is no Dialogue Manager within the scene!");
        }
        if (_player) {
            _player.InitializeCharacter(this);
        } else {
            if (allowDebugLog)
                Debug.Log("There is no Player within the scene!");
        }
        if (_playerShop) {
            _playerShop.InitializePlayerShop(this, _userInterfaceManager);
        }
        else {
            if (allowDebugLog) {
                Debug.Log("There is no Player Shop within the scene!");
            }
        }
        if (_userInterfaceManager)
            _userInterfaceManager.InitializeInterfaces(this);
        else {
            if (allowDebugLog)
                Debug.Log("There is no User Interface Manager within the scene!");
        }

        
        _setupShop = false;
        _accessingLoot = false;
        _accessingPlayerInv = false;
        _currentLootObject = null;
    
        Cursor.visible = true;
        _canMove = true;
        withinShopTrigger = false;
        _talkingWithNPC = false;
        RegisterEvents();
    }

    private void OnDestroy() {
        DeregisterEvents();
    }

    private void RegisterEvents() {

    }
    private void DeregisterEvents() {
        player.DeregisterEvents();
        level.DeregisterEvents();
    }
    private void Update() {
        _inputHandler.UpdateInputControls();
        _player.playerInput.UpdatePlayerInput();
        level.UpdateLevel();
    }

    private void LateUpdate() {
        _cam.UpdatePlayerCamera();
    }
    /// <summary>
    /// Function to check if Object is within Camera view
    /// </summary>
    /// <param name="objectCollider"></param>
    /// <returns></returns>
    public bool ObjectInCameraView(Collider objectCollider) {
        planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        if (GeometryUtility.TestPlanesAABB(planes, objectCollider.bounds)) {
            return true;
        } else {
            return false;
        }
    }
    /// <summary>
    /// Called when the user clicks on an object that contains loot.
    /// This will hook up the loot with the loot interface
    /// </summary>
    public void AccessLoot(Loot inventory) {
        if (!_accessingLoot && !_accessingPlayerInv && !_accessingNPCInv) {
            _userInterfaceManager.AccessLootInterface(inventory);
            _currentLootObject = inventory;
            _accessingLoot = true;
            _canMove = false;
        }
    }
    /// <summary>
    /// Access NPC inventory interface for trading
    /// </summary>
    /// <param name="inventory"></param>
    public void AccessNPCInventoryInterface(Inventory inventory) {
        if (!_accessingLoot && !_accessingPlayerInv && !_accessingNPCInv) {
            _accessingNPCInv = true;
        }
    }
    /// <summary>
    /// Called when we want to access our inventory interface
    /// </summary>
    public void AccessInventoryInterface(bool condition) {
        if (condition) {
            if (!_accessingPlayerInv && !_accessingLoot && !_accessingNPCInv) {
                _accessingPlayerInv = condition;
                _userInterfaceManager.SwitchInventoryScreen(InventoryState.Inventory);
                _userInterfaceManager.DisplayInventoryWindow(condition);
                _canMove = false;
            }
        } else {
            _accessingPlayerInv = condition;
            _userInterfaceManager.SwitchInventoryScreen(InventoryState.None);
            _userInterfaceManager.DisplayInventoryWindow(condition);
            _canMove = true;
        }
    }
    /// <summary>
    /// Function is designed to drop items to the world from various sources.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="location"></param>
    public void DropItem(Item item, Vector3 location) {
        //Create Object
        //Instantiate object into our world at location.
        GameObject clone = Instantiate(item.prefab, location, item.prefab.transform.rotation);
        clone.transform.tag = "Item";
    }
    /// <summary>
    /// Function for adding an item from an user interface panel
    /// </summary>
    /// <param name="slot"></param>
    public void AddItemFromLootInterface(int slot) {
        if (currentLootObject != null) {
            AddItemToInventory(currentLootObject.inventory[slot - 1].GetCopy());
            currentLootObject.DeleteItemSlot(slot);
        }else {
            Debug.Log("There is no loot object");
        }
    }
    /// <summary>
    /// Adds item to player inventory, and also updates the user interface
    /// </summary>
    /// <param name="item"></param>
    void AddItemToInventory(Item item) {
        player.inventory.AddItem(item);
        _userInterfaceManager.lootInterface.UpdateinterfaceSlots();
    }
    /// <summary>
    /// Function to call to delete item from player Inventory.
    /// Use this method instead of direct access to player inventory to update user interface elements.
    /// </summary>
    /// <param name="inventorySlot">slot reference</param>
    public void DeleteItemFromPlayerInventory(int inventorySlot) {
        player.inventory.DeleteItemSlot(inventorySlot);
        _userInterfaceManager.inventoryInterface.UpdateInventoryScreen(player.inventory);
    }
    public void DoneAccessingLoot() {
        _currentLootObject = null;
        _accessingLoot = false;
    }

    public void SavePlayerData() {

    }
}
