using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMenuInterface : UserInterface {
    public GameObject sideMenuObject;

    public override void InitializeUserInterface(GameManager gameManager, UserInterfaceManager userInterfaceManager) {
        base.InitializeUserInterface(gameManager, userInterfaceManager);
        sideMenuObject.SetActive(false);
    }

    public void DisplaySideMenuWindow(bool condition) {
        sideMenuObject.SetActive(condition);
    }
    public bool GetWindowStatus() {
        return sideMenuObject.activeSelf;
    }
}
