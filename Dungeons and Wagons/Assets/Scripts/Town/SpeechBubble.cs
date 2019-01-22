using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Entity;
using Entity.AI;
/// <summary>
/// Class handles speech bubble functionality
/// </summary>
/// 
public class SpeechBubble : MonoBehaviour {
    #region Data
    public Entity.NPC _owner;
    private Canvas _canvas;
    private SpeechBubbleManager _speechBubbleManager;
    public Vector3 offset;
    private AIAction _action;
    public List<string> speechList;
    //returns how many sentences we have for speech
    public int speechCount { get { return speechList.Count; } }
    [SerializeField]
    private int speechIndex;
    private float _talkTime;
    private string targetText;
    private bool _condition;
    [SerializeField]
    private GameObject imageObject;
    private Image image;
    private GameObject textObject;
    TextMeshProUGUI tmpProComponent;
    public float textDisplayTime;
    [SerializeField]
    Sprite[] talkSprites;
    [SerializeField]
    int talkSpriteIndex;
    bool isThinking = false;
    bool isThinkCoroutineRunning = false;
    #endregion

    public void IntializeSpeechBubble(SpeechBubbleManager speechBubbleManager, Entity.NPC npc, Canvas canvas, float talkTime) {
        _owner = npc;
        _canvas = canvas;
        _speechBubbleManager = speechBubbleManager;
        if(speechList.Count < 1) {
            speechList = new List<string>();
        }
        _talkTime = talkTime;
        imageObject = GetComponentInChildren<Image>().gameObject;
        image = imageObject.GetComponent<Image>();
        EnableSpeechBubble(false);

        talkSprites = new Sprite[3];
        talkSpriteIndex = 0;
        talkSprites[0] = _speechBubbleManager.GetEmotionImage("Thinking_1");
        talkSprites[1] = _speechBubbleManager.GetEmotionImage("Thinking_2");
        talkSprites[2] = _speechBubbleManager.GetEmotionImage("Thinking_3");
    }

    public void OnDestroy() {
        //Deregister Event
        //_owner.npcBehaviour.NPCTalkEvent -= OnTalkEvent; 
    }
   
    public void AddTextToList(string text) {
        speechList.Add(text);
    }

    void EnableSpeechBubble(bool condition) {
        imageObject.SetActive(condition);
    }
    public void OnTalkEvent(NPC npc, AIAction action) {
        if (npc == _owner) {
            if(_speechBubbleManager.CanDisplaySpeechBubble(this) && action != AIAction.None) {
                EnableSpeechBubble(true);
                if (action == AIAction.Yes)
                    image.sprite = _speechBubbleManager.GetEmotionImage("Happy");
                if(action == AIAction.No)
                    image.sprite = _speechBubbleManager.GetEmotionImage("Sad");

                if(action == AIAction.Interested)
                    image.sprite = _speechBubbleManager.GetEmotionImage("Idea");

                if(action == AIAction.Curious)
                    image.sprite = _speechBubbleManager.GetEmotionImage("Curious");

                if (action == AIAction.Think) {
                    if (!isThinking) {
                        isThinking = true;
                    }
                }
            } else {
                EnableSpeechBubble(false);
            }
        }
    }

    IEnumerator ThinkingCoroutine(float waitTime) {
        isThinkCoroutineRunning = true;
        if(talkSpriteIndex < talkSprites.Length)
            image.sprite = talkSprites[talkSpriteIndex];

        yield return new WaitForSeconds(waitTime);
        if (talkSpriteIndex < talkSprites.Length)
            talkSpriteIndex++;

        isThinkCoroutineRunning = false;
    }
    private void Update() {
        if (_owner) {
            Vector2 canvasPoint;
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(_owner.GetHeadPosition().position + offset);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.GetComponent<RectTransform>(), screenPoint, null, out canvasPoint);
            transform.localPosition = canvasPoint;

            if (isThinking) {
                if (!isThinkCoroutineRunning) {
                    StartCoroutine(ThinkingCoroutine(1.0f));
                } else {
                    if (talkSpriteIndex >= talkSprites.Length) {
                        StopCoroutine(ThinkingCoroutine(1.0f));
                        talkSpriteIndex = 0;
                        isThinkCoroutineRunning = false;
                        isThinking = false;
                    }
                }
            }
        }
    }
}
