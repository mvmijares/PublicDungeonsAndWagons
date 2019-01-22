using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity;
using Entity.AI;

/// <summary>
/// Defines the shopping behaviour for our NPC 
/// TODO: Make a base class for behaviours
/// </summary>
public class ShoppingBehaviour : NPCBehaviour {

    #region Data

    public List<ShoppingDecision> decisionList;
    [SerializeField]
    private int decisionIndex; // index for our decision list
    [SerializeField]
    private int shopIndex; // current shop item we are viewing

    public string desiredItem;

    private List<ShopItem> shopItems;

    Vector3 positionOffset = new Vector3(-0.5f, 0f, 0f);
    public float waitForEmotionTime = 1.0f;
    bool waitForEmotionToEnd = false;
    bool isEmotionDone = false;
    bool isBuying = false;
    bool isThinking = false;
    #endregion

    public override void InitializeBehaviour(GameManager gameManager, NPC npc) {
        base.InitializeBehaviour(gameManager, npc);
        shopIndex = 0;
        decisionIndex = 0;
        shopItems = null;

        if (decisionList.Count < 1) {
            decisionList = new List<ShoppingDecision>();
        }
    }
    public override void PlayBehaviour() {
        base.PlayBehaviour();
        if(decisionList.Count > 1 && decisionIndex <= decisionList.Count)
            ShoppingState();
    }
    /// <summary>
    /// Controls the AI for when they are shopping
    /// </summary>
    private void ShoppingState() {
        ShoppingDecision decision = ShoppingDecision.None;

        if (!isBuying)
            decision = decisionList[decisionIndex];
        else
            decision = ShoppingDecision.Buy;


        switch (decision) {
            case ShoppingDecision.None: {
                    SetPauseBehaviour(true, 1.0f);
                    break;
                }
            case ShoppingDecision.Walk: {
                    if (_npcPathfinder._target == null && !reachedEndofPath) {
                        _npcPathfinder.SetTarget(_npcManager.shoppingDestination);
                    }
                    break;
                }
            case ShoppingDecision.Shop: {
                    if (shopItems == null)
                        shopItems = _gameManager.playerShop.shopItems;

                    if (_npcPathfinder._target == null && !reachedEndofPath)
                        _npcPathfinder.SetTarget(shopItems[shopIndex].viewPoint);
                }
                break;
            case ShoppingDecision.Browse: {
                    Vector3 direction = shopItems[shopIndex].transform.position - transform.position;
                    Vector2 lookDirection = new Vector2(direction.x, direction.z);
                    _npcPathfinder.RotateToTarget(lookDirection);

                    if(desiredItem == shopItems[shopIndex].itemName) {
                        SetPauseBehaviour(true, 1.0f);
                    } else {
                        action = AIAction.No;
                        if(!waitForEmotionToEnd)
                            StartCoroutine(SendEmotionCoroutine(1.0f));
                    }
                    break;
                }
            case ShoppingDecision.Talk: {
                    Vector3 direction = _gameManager.player.transform.position - transform.position;
                    Vector2 lookDirection = new Vector2(direction.x, direction.z);
                    _npcPathfinder.RotateToTarget(lookDirection);

                    action = AIAction.Yes;
    
                    if (!waitForEmotionToEnd)
                        StartCoroutine(SendEmotionCoroutine(1.0f));


                    break;
                }
            case ShoppingDecision.Buy: {
                    if (!isThinking) {
                        action = AIAction.Think;
                        if (!waitForEmotionToEnd)
                            StartCoroutine(SendEmotionCoroutine(3.0f));

                        isThinking = true;
                    }
                    break;
                }
            
        }

        if (waitForEmotionToEnd) {
            if (isEmotionDone) {
                if (action != AIAction.Yes)
                    action = AIAction.None;
                else {
                    _npc.canTalkWith = true;
                }
                if (decision == ShoppingDecision.Buy) {
                    isBuying = false;
                    isThinking = false;
                }
                if (decision == ShoppingDecision.Browse) {
                    if (shopIndex < shopItems.Count - 1)
                        shopIndex++;
                } else {
                    if (decisionIndex < decisionList.Count - 1)
                        decisionIndex++;
                }

                reachedEndofPath = false;
                isEmotionDone = false;
                waitForEmotionToEnd = false;
    
            }
        }

        SendEmotion();
    }
    IEnumerator SendEmotionCoroutine(float emotionWaitTime) {
        waitForEmotionToEnd = true;
        yield return new WaitForSeconds(emotionWaitTime);
        StopCoroutine(SendEmotionCoroutine(emotionWaitTime));
        isEmotionDone = true;
    }
    public override void EndOfPathReached(bool condition) {
        base.EndOfPathReached(condition);
        reachedEndofPath = condition;

        if (condition) {
            if (_npcPathfinder._target != null)
                _npcPathfinder.SetTarget(null);

            if (decisionList[decisionIndex] == ShoppingDecision.Walk) {
                if (!waitForEmotionToEnd) {
                    action = AIAction.Curious;
                    StartCoroutine(SendEmotionCoroutine(1.0f));
                }
            }
            if(decisionList[decisionIndex] == ShoppingDecision.Shop) {
                if (!waitForEmotionToEnd) {
                    action = AIAction.Interested;
                    StartCoroutine(SendEmotionCoroutine(1.0f));
                }
            }

            if (!waitForEmotionToEnd) {
                if (decisionIndex < decisionList.Count - 1)
                    decisionIndex++;

                reachedEndofPath = false;
            }
        }
    }
  
    public override void OnPauseBehaviourEnd() {
        base.OnPauseBehaviourEnd();
        if (decisionList[decisionIndex] == ShoppingDecision.Browse) {
            isBuying = true;
        } else {
            if (decisionIndex < decisionList.Count - 1)
                decisionIndex++;
        }
    }
    
    void SetShopTarget() {
        _npcPathfinder.SetTarget(_npcManager.shoppingDestination);
    }
}
