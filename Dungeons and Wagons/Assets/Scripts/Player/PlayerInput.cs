using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entity;

public class PlayerInput : MonoBehaviour {
    #region Data
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Player _player;
    [SerializeField] private bool _lockCursor;
    public bool lockCursor { get { return _lockCursor; } }
    [SerializeField] private bool escapeKey;
    [SerializeField] private bool runKey;
    [SerializeField] private bool actionKey;
    [SerializeField] private bool mouseRightClick;
    [SerializeField] private bool mouseLeftClick;
    [SerializeField] private float mouseScrollWheel;
    [SerializeField] private Vector2 movement;

    public float zoomSensitivity = 1;
    public float mouseSensitivity = 10;

    private bool openInventory;
    public bool inventoryStatus { get { return openInventory; } }

    [Tooltip("Used to determine how far from an item the player can be before opening up loot menu.")]
    public float distFromItem;

    [Tooltip("Distance from NPC before player can start a conversation")]
    public float distFromNPC;

    [Tooltip("Used for which objects we can interact with for trading or looting")]
    public LayerMask checkObjectLayerMask;
    [Tooltip("Distance from loot that the player needs to be from")]
    public float checkObjectRaycastDist;
    #endregion

    #region Input Events
    
    public void OnRunningKeyPressed(bool condition) { runKey = condition; }
    public void OnActionKeyPressed(bool condition) { actionKey = condition; }
    public void OnMovementPressed(Vector2 movement) { this.movement = movement; }
    public void OnMouseRightClickPressed(bool condition) { mouseRightClick = condition; }
    public void OnMouseLeftClickPressed(bool condition) { mouseLeftClick = condition; }
    public void OnMouseScrollWheel(float value) { mouseScrollWheel = value; }

    public void OnEscapeKeyPressed() {
        openInventory = !openInventory;
        _player.accessingInterface = openInventory;
        _gameManager.AccessInventoryInterface(openInventory);
    }

    public void OnInventoryKeyPressed() {
        openInventory = !openInventory;
        _player.accessingInterface = openInventory;
        _gameManager.AccessInventoryInterface(openInventory);
    }
    #endregion

    public void InitializePlayerInput(GameManager gameManager, Player player) {
        _gameManager = gameManager;
        _player = player;
        _lockCursor = true;
        openInventory = false;
    }

    public void UpdatePlayerInput() {
        if (mouseRightClick) 
            _lockCursor = false;
        else 
            _lockCursor = true;

        if (!lockCursor) {
            if (mouseLeftClick) {
                CheckForObject();
            }
        }

        if (mouseLeftClick)
            CheckForObject();

        if(movement != Vector2.zero && _player.selectedEnemy != null) {
            _player.playerPathfinder.SetTarget(null);
            _player.isAttacking = false;
        }
        if (_player.isAttacking) {
            _player.playerAnimationController.SetControllerAnimation(AnimationController.AnimationName.Attacking);
        }
        
        _player.playerController.moveDirection = movement;
        _player.playerController.running = runKey;
    }

    void CheckForObject() {
        RaycastHit[] hits;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        hits = Physics.RaycastAll(ray, checkObjectRaycastDist, checkObjectLayerMask);
        bool found = false; // raycast first object found
        foreach (RaycastHit hit in hits) {
            switch (hit.transform.gameObject.tag) {
                case "Loot": {
                        CheckLootObject(hit.transform);
                        found = true;
                        break;
                    }
                case "NPC": {
                        CheckNPCObject(hit.transform);
                        found = true;
                        break;
                    }
                case "Enemy": {
                        CheckEnemyObject(hit.transform);
                        found = true;
                        break;
                    }
            }
            if (found)
                break;
        }
    }
    /// <summary>
    /// Raycast checks for Objects that we click in the game.
    /// 
    /// </summary>

    /// <summary>
    /// Object detection for Enemies
    /// </summary>
    /// <param name="reference"></param>
    void CheckEnemyObject(Transform reference) {
        if (reference.GetComponent<Enemy>()) {
            _player.clickObject = reference;
            _player.selectedEnemy = reference.GetComponent<Enemy>();
            _player.playerPathfinder.SetTarget(reference);
        }
    }
    /// <summary>
    /// Object detection for loot 
    /// </summary>
    /// <param name="reference"></param>
    void CheckLootObject(Transform reference) {
        if (reference.gameObject.GetComponent<Loot>()) {
            float distance = (_player.transform.position - reference.position).magnitude;
            if (distance < distFromItem) {
                _player.clickObject = reference;
                reference.gameObject.GetComponent<Loot>().AccessLoot();
            }
        }
    }
    /// <summary>
    /// Object detection for NPC
    /// </summary>
    /// <param name="reference"></param>
    void CheckNPCObject(Transform reference) {
        if (reference.gameObject.GetComponent<NPC>()) {
            NPC npc = reference.gameObject.GetComponent<NPC>();
            float distance = (_player.transform.position - reference.position).magnitude;
            if (distance < distFromNPC && npc.canTalkWith) {
                _player.clickObject = reference;
                _player.talkingWithNPC = true;
                _gameManager.level.InteractedWithNPC(npc);
            }
        }
    }

    public float CalculateCharacterZoom(float dstFromTarget) {
        float newDstFromTarget = dstFromTarget;

        if (mouseScrollWheel < 0) {
            newDstFromTarget += zoomSensitivity * Time.deltaTime;
        } else if (mouseScrollWheel > 0)
            newDstFromTarget -= zoomSensitivity * Time.deltaTime;

        return newDstFromTarget;
    }
}
