using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

using Entity.AI;

namespace Entity {
    public class NPCController :  Controller {
        #region Data

        NPC _npc;
        public float speedFactor;
        #endregion
        /// <summary>
        /// Initialization for controller
        /// </summary>
        /// <param name="gameManager"></param>
        public override void InitializeController(GameManager gameManager, Character character) {
            base.InitializeController(gameManager, character);
            _npc = (NPC)character;
        }
        public void Update() {
            if (running) {
                targetRunSpeed = runSpeed * speedFactor;
            } else {
                targetWalkSpeed = walkSpeed * speedFactor;
            }
        }

        public void SetRunningCondition(bool condition) {
            running = condition;
        }
        
    } 
}
