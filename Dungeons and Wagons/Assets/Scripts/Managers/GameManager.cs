using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

using UserInterfaceStates;
using Entity;
using Entity.AI;
using Overworld;
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
    private Scene currentScene;

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

    public bool withinShopTrigger; // Use this to figure out if player is within the shop wireframe box.
    //Used to determine if an object is within view of camera
    private Plane[] planes;


    private EnemyManager _enemyManager;
    public EnemyManager enemyManager;

    [SerializeField] private Entity.Character selectedObject;
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
        _imageLibrary.InitializeImageLibrary();
        _itemDatabase.InitializeItemDatabase();

       
    }
    private void OnDestroy() {
        if(level)
            level.DestroyLevel();
    }
    private void OnEnable() {
        RegisterEvents();
    }
    private void OnDisable() {
        DeregisterEvents();
    }

    private void RegisterEvents() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void DeregisterEvents() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        currentScene = scene;
        InitializeObjects();
        switch (scene.name) {
            case "Overworld": {
                    OverworldManager overworldManager = FindObjectOfType<OverworldManager>();
                    if (overworldManager) {
                        overworldManager.InitializeOverworldManager(this);
                    }
                    break;
                }
            case "Town": {
                    break;
                }
            case "Combat": {
                    break;
                }
        }
    }
    void InitializeObjects() {
        _npcManager = FindObjectOfType<NPCManager>();
        _speechBubbleManager = FindObjectOfType<SpeechBubbleManager>();
        _userInterfaceManager = FindObjectOfType<UserInterfaceManager>();
        _dialogueManager = FindObjectOfType<DialogueManager>();
        _inputHandler = FindObjectOfType<InputHandler>();
        _inputHandler.InitializeInputHandler(this);
        if (_inputHandler)
            input = _inputHandler.input;
        else {
            if (allowDebugLog)
                Debug.Log("There is no input handler in the scene!");
        }
       
        level = FindObjectOfType<Level>();
        if (level)
            level.InitializeLevel(this);

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
        _setupShop = false;
        _accessingLoot = false;
        _accessingPlayerInv = false;
        _currentLootObject = null;

        Cursor.visible = true;
        _talkingWithNPC = false;
    }
    private void Update() {
        _inputHandler.UpdateInputControls();
        if (level)
            level.UpdateLevel();
        
    }
    private void LateUpdate() {
        if (level)
            level.LateUpdateLevel();
    }
    public Type GetLevelType() {
        return level.GetType();
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
            }
        } else {
            _accessingPlayerInv = condition;
            _userInterfaceManager.SwitchInventoryScreen(InventoryState.None);
            _userInterfaceManager.DisplayInventoryWindow(condition);
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
    public void SavePlayerData() {

    }
}
