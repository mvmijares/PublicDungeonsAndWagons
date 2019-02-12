using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using UserInterfaceStates;

public class InventoryPage : MonoBehaviour, IPointerClickHandler {
    #region Data
    private UserInterfaceManager userInterfaceManager;
    public InventoryPageState state;
    #endregion

    public void InitializeInventoryPage(UserInterfaceManager userInterfaceManager) {
        this.userInterfaceManager = userInterfaceManager;
    }
	
    public void OnPointerClick(PointerEventData pointerEventData) {
        if(pointerEventData.button == PointerEventData.InputButton.Left) {
            userInterfaceManager.OnSwitchPage(state);
        }
    }
}
