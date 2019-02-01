using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Entity;

public class EnemyUserInterface : UserInterface {
    #region Data
    public Sprite combatMarker; //sprite for enemy that we are currently fighting
    public Sprite selectedMarker; //sprite for enemy that we have selected

    #endregion

    public override void InitializeUserInterface(GameManager gameManager, UserInterfaceManager userInterfaceManager) {
        base.InitializeUserInterface(gameManager, userInterfaceManager);
        
    }


}
