using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entity;

[CreateAssetMenu(fileName = "New NPC Dialogue", menuName = "Dialogue/NPC Dialogue", order = 1)]
public class NPCDialogue : ScriptableObject {

    [Tooltip("This is the NPC name.")]
    public string npcName;

    [TextArea(3, 10), Tooltip("Add a <question> at the end if sentence requires an answer to continue with dialogue.")]
    public string[] sentences;

    [TextArea(2, 10), Tooltip("Add a <any_number> at the end that references the question to answer. For example <1> references the first question.")]
    public string[] answers;
}
