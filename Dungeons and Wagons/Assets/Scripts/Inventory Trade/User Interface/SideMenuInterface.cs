using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMenuInterface : UserInterface {

    public override void InitializeUserInterface(GameManager gameManager, UserInterfaceManager userInterfaceManager) {
        base.InitializeUserInterface(gameManager, userInterfaceManager);
        EnableUserInterface(false);
    }

    public void DisplaySideMenuWindow(bool condition) {
        EnableUserInterface(condition);
    }

    public bool GetWindowStatus() {
        return interfaceObject.activeSelf;
    }
}
