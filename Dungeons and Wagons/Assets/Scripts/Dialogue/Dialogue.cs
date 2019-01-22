using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue {
    Dictionary<int, string> questions;
    Dictionary<int, string[]> answers;

    Dialogue() {
        questions = new Dictionary<int, string>();
        answers = new Dictionary<int, string[]>();
    }
}