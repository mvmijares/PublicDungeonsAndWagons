using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UserInterfaceStates;

/// <summary>
/// This class handles the UI for telling players what to press / do
/// TODO: instead of getting sprite reference for input directly, create a 
/// input sprite directory in UserInterface Manager, and call that
/// </summary>
public class HelpInterface : UserInterface {
    #region Data
    [SerializeField] PlayerShopData shopData;
    [SerializeField] HelpInterfaceState helpInterfaceState;
    public Image helpImage;
    [SerializeField] List<Sprite> buttonSprites; // Reference to sprites used for button input

    [SerializeField] private TextMeshProUGUI _rightText;
    [SerializeField] private TextMeshProUGUI _leftText;

    #endregion
    public override void InitializeUserInterface(GameManager gameManager, UserInterfaceManager userInterfaceManager) {
        base.InitializeUserInterface(gameManager, userInterfaceManager);
        Transform[] children = interfaceObject.GetComponentsInChildren<Transform>();
        foreach(Transform t in children) {
            if (t.name == "Right Text")
                _rightText = t.GetComponent<TextMeshProUGUI>();
            if (t.name == "Left Text")
                _leftText = t.GetComponent<TextMeshProUGUI>();
        }
        if (!_rightText) 
            Debug.Log("There is no text to the right in the Help interface");
        if(!_leftText)
            Debug.Log("There is no text to the left in the Help interface");

        helpInterfaceState = HelpInterfaceState.PlayerShop; // Setup for now
        buttonSprites = new List<Sprite>();
        buttonSprites.Add(gameManager.imageLibrary.GetSpriteReference("Keyboard_White_Space"));
        EnableUserInterface(false);
    }
   
    /// <summary>
    /// Event delegate for player event.
    /// </summary>
    public void OnPlayerShopEventCalled(PlayerShopData data) {
        if (data != null) {
            shopData = data;
            if (helpInterfaceState == HelpInterfaceState.PlayerShop) {
                if (_userInterfaceManager.state == InventoryState.None) {
                    SetHelpInterface(data._playerWithinBox);
                } else {
                    SetHelpInterface(data._playerWithinBox);
                }
            }
        }
    }

    /// <summary>
    /// Enable or disable the graphics for the help interface
    /// </summary>
    /// <param name="condition"></param>
    public void SetGraphics(bool condition) {
        interfaceObject.SetActive(condition);
    }
    /// <summary>
    /// Set the text for the help interface
    /// </summary>
    /// <param name="text"></param>
    void SetText(string text,TextPosition textPos) {
        if(textPos == TextPosition.Right)
            _rightText.text = text;
        if (textPos == TextPosition.Left)
            _leftText.text = text;
    }
    void SetHelpInterface(bool condition) {
        switch (shopData._state) {
            case PlayerShopState.None: {
                    if(shopData._inventoryCount > 0)
                        EnableShopSetup(condition);
                    break;
                }
            case PlayerShopState.Setup: {
                    EnableStartTrade(condition);
                    break;
                }
        }
    }
    /// <summary>
    /// Event for Starting the NPC shop
    /// </summary>
    /// <param name="playerShop"></param>
    public void StartNPCTradeEvent(PlayerShopData data) {
        SetGraphics(true);
        helpImage.sprite = _gameManager.imageLibrary.GetSpriteReference("Keyboard_White_Space");
        SetText("Press", TextPosition.Left);
        SetText("to start trading.", TextPosition.Right);
    }
    private void EnableStartTrade(bool condition) {
        ClearText();
        SetGraphics(condition);
        helpImage.sprite = _gameManager.imageLibrary.GetSpriteReference("Keyboard_White_Space");
        SetText("Press", TextPosition.Left);
        SetText("to start trading.", TextPosition.Right);
    }
    private void EnableShopSetup(bool condition) {
        ClearText();
        SetGraphics(condition);
        helpImage.sprite = _gameManager.imageLibrary.GetSpriteReference("Keyboard_White_Space");
        SetText("Press", TextPosition.Left);
        SetText("to setup shop.", TextPosition.Right);
    }

    private void ClearText() {
        _rightText.text = "";
        _leftText.text = "";
    }
}
