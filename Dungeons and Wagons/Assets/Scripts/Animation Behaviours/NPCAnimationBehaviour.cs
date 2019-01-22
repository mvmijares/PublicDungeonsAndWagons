using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationBehaviour : StateMachineBehaviour {

    Entity.AnimationController animationController;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animationController = animator.GetComponent<Entity.AnimationController>();
    }
    /// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Talking")) {
            animationController.state = Entity.AnimationController.AnimationState.IsPlaying;
            if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f) {
                animator.SetBool("Talking", false);
                animationController.state = Entity.AnimationController.AnimationState.HasCompleted;
            }
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Agreeing")) {
            animationController.state = Entity.AnimationController.AnimationState.IsPlaying;
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f) {
                animator.SetBool("Agreeing", false);
                animationController.state = Entity.AnimationController.AnimationState.HasCompleted;
            }
        }
    }
}
