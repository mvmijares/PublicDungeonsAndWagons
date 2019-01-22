using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInventoryInput : MonoBehaviour {

    Entity.Player player;
    public Item tryItem;
    private void Start() {
        player = FindObjectOfType<Entity.Player>();
    }
    // Update is called once per frame
    void Update () {
        if (Input.GetKeyUp(KeyCode.P)) {
            player.inventory.AddItem(tryItem);
        }

        if (Input.GetKeyUp(KeyCode.O)) {

        }
	}
}
