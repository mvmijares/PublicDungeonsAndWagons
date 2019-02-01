using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

using Entity.AI;

namespace Entity {
    public class NPCController :  Controller {
        #region Data
        #endregion
        /// <summary>
        /// Initialization for controller
        /// </summary>
        /// <param name="gameManager"></param>
        public override void InitializeController(GameManager gameManager, Character character) {
            base.InitializeController(gameManager, character);
        }
        public void SetRunningCondition(bool condition) {
            running = condition;
        }
    } 
}
