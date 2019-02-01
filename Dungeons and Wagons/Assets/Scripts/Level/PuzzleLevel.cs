using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entity;

public class PuzzleLevel : Level {
    #region Data

    #endregion

    #region Event Data

    #endregion

    public override void InitializeLevel(GameManager gameManager) {
        base.InitializeLevel(gameManager);

        if (player == null) {
            Debug.Log("There is no player in the puzzle level!");
        } else {
            player.transform.position = playerSpawnLocation.position;
        }
    }

    private void OnDestroy() {
        DeregisterEvents();
    }
    public override void RegisterEvents() {
        base.RegisterEvents();
    }
    public override void DeregisterEvents() {
        base.DeregisterEvents();
    }

    public override void UpdateLevel() {
        base.UpdateLevel();

    }

    public override void InteractedWithEnemy(Enemy enemy) {
        base.InteractedWithEnemy(enemy);
    }
}
