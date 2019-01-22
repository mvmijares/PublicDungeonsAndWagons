using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StackInterface : UserInterface {
    #region Data
    public GameObject stackInterfaceObject;

    InputField inputfield;
    [SerializeField]
    Button confirmButton;

    #endregion
    public override void InitializeUserInterface(GameManager gameManager, UserInterfaceManager userInterfaceManager) {
        base.InitializeUserInterface(gameManager, userInterfaceManager);

        inputfield = stackInterfaceObject.GetComponentInChildren<InputField>();
        confirmButton = stackInterfaceObject.GetComponentInChildren<Button>();
        confirmButton.onClick.AddListener(() => ConfirmButtonWasPressed());
        DisplayStackWindow(false);
    }
    public bool GetWindowStatus() {
        return stackInterfaceObject.activeSelf;
    }
    public void DisplayStackWindow(bool condition) {
        if(!stackInterfaceObject.activeSelf && condition)
            inputfield.text = "0";

        stackInterfaceObject.SetActive(condition);
    }
    void ConfirmButtonWasPressed() {
        //TODO: Error throwing for user input
        _userInterfaceManager.CalculateNewItemCount(Convert.ToInt32(inputfield.text));
    }
}
