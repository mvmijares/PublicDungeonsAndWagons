using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity {
    public class AnimationController : MonoBehaviour {

        #region Data
        public enum AnimationName {
            None, Talking, Agreeing
        }
        public enum AnimationState {
            None, HasStarted, IsPlaying, HasCompleted
        }

        private GameManager _gameManager;
        private Animator anim;

        public AnimationState state;

        #endregion

        /// <summary>
        /// Initilize the animation controller
        /// </summary>
        /// <param name="gameManager"></param>
        public virtual void InitializeAnimationController(GameManager gameManager) {
            _gameManager = gameManager;
            anim = GetComponentInChildren<Animator>();
            state = AnimationState.None;
        }
        /// <summary>
        /// Set the values for our blend tree for locomotion
        /// </summary>
        /// <param name="animationSpeedPercent"></param>
        /// <param name="speedSmoothTime"></param>
        public void SetLocomotion(float animationSpeedPercent, float speedSmoothTime) {
            anim.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
        }
        /// <summary>
        /// Function to set the conditions for our animator.
        /// 
        /// </summary>
        /// <param name="name"></param>
        public virtual void SetControllerAnimation(AnimationName name) {
            if (anim) {
                if (name == AnimationName.Agreeing) {
                    if (!anim.GetBool("Agreeing")) {
                        anim.SetBool("Agreeing", true);
                        state = AnimationState.HasStarted;
                    }
                }
                if (name == AnimationName.Talking) {
                    if (!anim.GetBool("Talking")) {
                        anim.SetBool("Talking", true);
                        state = AnimationState.HasStarted;
                    }
                }
            }
        }
    }
}
