using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StackInterface : UserInterface {
    #region Data
    InputField inputfield;
    [SerializeField]
    Button confirmButton;

    #endregion
    public override void InitializeUserInterface(GameManager gameManager, UserInterfaceManager userInterfaceManager) {
        base.InitializeUserInterface(gameManager, userInterfaceManager);

        inputfield = interfaceObject.GetComponentInChildren<InputField>();
        confirmButton = interfaceObject.GetComponentInChildren<Button>();
        confirmButton.onClick.AddListener(() => ConfirmButtonWasPressed());
        EnableUserInterface(false);
    }
    public bool GetWindowStatus() {
        return interfaceObject.activeSelf;
    }
    public void DisplayStackWindow(bool condition) {
        if(!interfaceObject.activeSelf && condition)
            inputfield.text = "0";

        EnableUserInterface(condition);
    }
    void ConfirmButtonWasPressed() {
        //TODO: Error throwing for user input
        _userInterfaceManager.CalculateNewItemCount(Convert.ToInt32(inputfield.text));
    }
}
