using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entity.AI;

namespace Entity {
    /// <summary>
    /// Used to control multiple NPCs with different animation times / speeds
    /// </summary>
    public class ConversationBox : MonoBehaviour {
        public NPCBehaviour[] npcBehaviours;

        public float waitTime;
        private float currentWaitTime = 0f;

        private bool animationCheck;

        private void Update() {
            animationCheck = false;
            foreach (NPCBehaviour behaviour in npcBehaviours) {
                if(behaviour.GetAnimationState() == AnimationController.AnimationState.HasCompleted) {
                    animationCheck = true;
                } else {
                    break;
                }
            }
            if (animationCheck) {
                currentWaitTime += Time.deltaTime;
                if (currentWaitTime > waitTime) {
                    currentWaitTime = 0;
                    foreach (NPCBehaviour behaviour in npcBehaviours) {
                        behaviour.PlayControllerAnimation();
                    }
                    animationCheck = false;
                }
            }
        }
    }
}
