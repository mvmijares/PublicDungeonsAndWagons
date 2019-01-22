using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Entity;

/// <summary>
/// Dialogue Manager for our class
/// Handles printing out sentences to UI
/// </summary>
public class DialogueManager : MonoBehaviour {

    #region Data
    private GameManager _gameManager;
    public List<NPCDialogue> dialogueList;
    public GameObject dialogueBox;

    public Text nameText;
    public Text dialogueText;
    private Queue<string> sentences;

    #endregion

    public void InitializeDialogueManager(GameManager gameManager) {
        _gameManager = gameManager;
        dialogueList = new List<NPCDialogue>();
        sentences = new Queue<string>();
        dialogueBox.SetActive(false);
        LoadAllDialogue();

    }
    /// <summary>
    /// Loads all dialogue for level.
    /// TODO: Add loading for dialogue for each NPC based on which scene we are in.
    /// </summary>
    void LoadAllDialogue() {
        Object[] dialogueArray = Resources.LoadAll("Town/NPC/Dialogue", typeof(NPCDialogue));
        foreach(Object d in dialogueArray) {
            NPCDialogue dialogue = d as NPCDialogue;
            dialogue.npcName = d.name;
            //Parse sentence and check if its a question
            ParseSentences(dialogue.sentences);
            //If it is a question add it to question dictionary with index as key
            //Dictionary<int(index), string(sentence)>
            //Parse answers and check the question references
            //the question string is the key
            //Dictionary<int(index of question), List<string>(answers)>

            dialogueList.Add(dialogue);
        }
    }
    void ParseSentences(string[] sentences) {
        foreach(string sentence in sentences) {
            if (sentence.Contains("<question>")) {

            }
        }
    }
    void ParseQuestions(string sentence) {

    }
    void ParseAnswers(string sentence) {

    }
    public void PlayDialogue(NPC npc) {
        if (FindDialogue(npc._name)) {
            SetDialogueBoxActive(true);
            StartDialogue(FindDialogue(npc._name));
        }
    }

    NPCDialogue FindDialogue(string name) {
        NPCDialogue found = null;
        foreach(NPCDialogue d in dialogueList) {
            if(d.npcName == name) {
                found = d;
                return found;
            }
        }
        return null;
    }
    public void SetDialogueBoxActive(bool condition) {
        dialogueBox.SetActive(condition);
    }

    public void StartDialogue(NPCDialogue dialogue) {
        nameText.text = dialogue.name;

        sentences.Clear();

        foreach(string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        if(sentences.Count == 0) {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence) {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return null;
        }
    }
    void EndDialogue() {

    }
}
