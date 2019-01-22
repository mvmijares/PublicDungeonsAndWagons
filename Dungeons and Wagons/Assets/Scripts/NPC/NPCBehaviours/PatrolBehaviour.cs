using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entity.AI;
using Entity;

public class PatrolBehaviour : NPCBehaviour {
    #region Data

    [SerializeField]
    public Transform[] targets;
    public int targetIndex;
    
    #endregion
    public override void InitializeBehaviour(GameManager gameManager, NPC npc) {
        base.InitializeBehaviour(gameManager, npc);

        reachedEndofPath = false;

        if (targets.Length < 1)
            Debug.Log("There are no targets in the Patrol Behaviour.");

        if (targetIndex > targets.Length)
            targetIndex = 0;
        else if (targetIndex < 0)
            targetIndex = 0;
    }
    public override void EndOfPathReached(bool condition) {
        base.EndOfPathReached(condition);
        if (_npcPathfinder._target != null)
            _npcPathfinder.SetTarget(null);

        targetIndex++;
    }
    public override void PlayBehaviour() {
        base.PlayBehaviour();
        if (targets.Length > 0) {
            if (_npcPathfinder._target == null) {
                _npcPathfinder.SetTarget(targets[targetIndex]);
            }
        }
    }
}

