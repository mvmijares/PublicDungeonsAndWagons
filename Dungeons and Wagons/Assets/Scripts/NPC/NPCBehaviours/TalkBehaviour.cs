using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entity.AI;
using Entity;

public class TalkBehaviour : NPCBehaviour {
    #region Data
    public bool talkBoxActive;
    private float currentTalkTime = Mathf.Infinity;
    public float talkWaitTime = 2f;
    #endregion



    public override void InitializeBehaviour(GameManager gameManager, NPC npc) {
        base.InitializeBehaviour(gameManager, npc);

        _gameManager.speechBubbleManager.InstantiateNewSpeechBubble(_npc, talkWaitTime);
    }
    /// <summary>
    /// Coroutine loop for our talking behaviour
    /// </summary>
    /// <returns></returns>
    IEnumerator TalkBehaviourCoroutine() {
        yield return new WaitForSeconds(talkWaitTime);
        PlayTalkAnimation();
        StopCoroutine(TalkBehaviourCoroutine());
    }
    /// <summary>
    /// Talk Behaviour logic
    /// </summary>
    private void PlayTalkAnimation() {
        PlayControllerAnimation();
    }
    /// <summary>
    /// Talk behaviour for our NPCs
    /// </summary>
    private void Talking() {
        if (GetAnimationState() == AnimationController.AnimationState.HasCompleted ||
            GetAnimationState() == AnimationController.AnimationState.None) {
            StartCoroutine(TalkBehaviourCoroutine());
        }
    }


}
