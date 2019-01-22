using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entity;

public class TownLevel : Level {
    #region Data

    public PlayerShop playerShop;
    public Player player;
    [SerializeField] private bool actionKey;
    [SerializeField] private bool withinPlayerShop;
    PlayerShopData shopData;

    #endregion
    #region Event Data
    public void OnActionKeyPressed(bool condition) { actionKey = condition; }
    public void OnPlayerShopEventCalled(PlayerShopData data) { shopData = data; }
    #endregion

    public override void InitializeLevel(GameManager gameManager) {
        base.InitializeLevel(gameManager);

        if (playerShop == null)
            Debug.Log("There is no Player Shop in the town level!");

        if (player == null)
            Debug.Log("There is no Player in the town level!");
        else {
            player.transform.position = playerSpawnLocation.position;
        }
        
        shopData = new PlayerShopData();

        RegisterEvents();
    }

    private void OnDestroy() {
       
    }
    public override void RegisterEvents() {
        base.RegisterEvents();
        playerShop.OnPlayerShopEvent += OnPlayerShopEventCalled;
        _gameManager.inputHandler.OnActionKeyPressedEvent += OnActionKeyPressed;
    }

    public override void DeregisterEvents() {
        base.DeregisterEvents();
        playerShop.OnPlayerShopEvent -= OnPlayerShopEventCalled;
        
        _gameManager.inputHandler.OnActionKeyPressedEvent -= OnActionKeyPressed;
    }

    public override void UpdateLevel() {
        base.UpdateLevel();
        if (playerShop != null && shopData != null) {
            if (shopData._playerWithinBox) {
                if (actionKey) {
                    playerShop.ActionWasPressed(shopData._playerWithinBox);
                }
            } else {
                playerShop.ActionWasPressed(shopData._playerWithinBox);
            }
        }
    }
    public override void InteractedWithNPC(NPC npc) {
        base.InteractedWithNPC(npc);
       
        npc.SetNPCBehaviour(typeof(TalkBehaviour));
        _gameManager.cam.GetComponent<ThirdPersonCamera>().target = npc.GetBodyPosition();
        _gameManager.dialogueManager.PlayDialogue(npc);
    }
}
