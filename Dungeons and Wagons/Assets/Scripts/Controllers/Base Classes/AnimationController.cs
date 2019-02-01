using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity {
    /// <summary>
    /// Dictates our animations for our Human controllers
    /// </summary>
    public class AnimationController : MonoBehaviour {

        #region Data
        public enum AnimationName {
            None, Talking, Agreeing, Attacking
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
        /// </summary>
        /// <param name="name"></param>
        public virtual void SetControllerAnimation(AnimationName name) {
            if (anim) {
                switch (name) {
                    case AnimationName.Agreeing: {
                            if (!anim.GetBool("Agreeing")) {
                                anim.SetBool("Agreeing", true);
                                state = AnimationState.HasStarted;
                            }
                            break;
                        }
                    case AnimationName.Talking: {
                            if (!anim.GetBool("Talking")) {
                                anim.SetBool("Talking", true);
                                state = AnimationState.HasStarted;
                            }
                            break;
                        }
                    case AnimationName.Attacking: {
                            LoopAttackAnimations();
                            break;
                        }
                }
            }
        }
        /// <summary>
        /// If we have multiple attack animations, we should loop through them
        /// </summary>
        private void LoopAttackAnimations() {
            if (!anim.GetBool("Attacking") && state != AnimationState.HasStarted) {
                anim.SetBool("Attacking", true);
                state = AnimationState.HasStarted;
            }
        }
    }
}
