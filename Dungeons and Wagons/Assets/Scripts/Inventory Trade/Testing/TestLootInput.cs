using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLootInput : MonoBehaviour {

    Loot loot;
	// Use this for initialization
	void Start () {
        loot = GetComponent<Loot>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.O)) {
            loot.AccessLootMenu();
        }
	}
}
