using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Entity;
using Entity.AI;

/// <summary>
/// Class handles instantiating speech bubbles on mutliple NPCS in the scene
/// </summary>
public class SpeechBubbleManager : MonoBehaviour {
    #region Data
    private GameManager _gameManager;
    public GameManager gameManager { get { return _gameManager; } }

    UserInterfaceManager _userInterfaceManager;
    public GameObject speechBubblePrefab;
    public Vector3 offset; // offset of the npc

    Dictionary<string, Sprite> emotionLibrary;
    #endregion

    public void InitializeSpeechBubbleManager(GameManager gameManager) {
        _gameManager = gameManager;
        _userInterfaceManager = gameManager.level.userInterfaceManager;

        emotionLibrary = gameManager.imageLibrary.GetEmotionLibrary();
        if(emotionLibrary == null) {
            Debug.Log("There are no emotions in the dictionary!");
        }

    }
    public bool CanDisplaySpeechBubble(SpeechBubble bubble) {
        if (gameManager.ObjectInCameraView(bubble._owner.GetComponent<Collider>()) && _userInterfaceManager.canDisplaySpeechBubble) 
            return true;
        else
            return false;
    }

    public Sprite GetEmotionImage(string name) {
        return emotionLibrary[name];
    }
    /// <summary>
    /// This function is called when the NPC wants to talk, and the player
    /// is within the proximity of the NPC
    /// </summary>
    public SpeechBubble InstantiateNewSpeechBubble(NPC _npc, float talkTime) {
        if (speechBubblePrefab) {
            GameObject clone = Instantiate(speechBubblePrefab, _npc.transform.position + offset, speechBubblePrefab.transform.rotation) as GameObject;
            clone.transform.SetParent(this.transform);
            clone.GetComponent<SpeechBubble>().IntializeSpeechBubble(this, _npc, GetComponent<Canvas>(), talkTime);
            foreach(NPCBehaviour behaviour in _npc.behaviourList) {
                behaviour.SendEmotionEvent += clone.GetComponent<SpeechBubble>().OnTalkEvent;
            }
            return clone.GetComponent<SpeechBubble>();
        }
        return null;
    }
}
