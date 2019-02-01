using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity;

using Entity.AI;

public class NPCManager : MonoBehaviour {
    #region Data
    private GameManager _gameManager;
    public GameObject NPCPrefab;
    private List<NPC> NPCList;
    public int npcListCount {
        get {
            return NPCList.Count;
        }
    }
    public Transform NPCSpawnLocation;
    public Transform shoppingDestination;
    public List<Transform> patrolPoints;
    Dictionary<string, Transform> patrolPointsDictionary;
    public List<ShoppingDecision> shopDecisionList;

    #endregion
    //Initialization
    public void InitializeNPCManager(GameManager gameManager) {
        _gameManager = gameManager;
        InitializeStartingNPCList();
        NPCList = new List<NPC>();
    }
    /// <summary>
    /// Search for all NPCs in the scene and initialize them.
    /// </summary>
    void InitializeStartingNPCList() {
        NPC[] nonPlayerChars = FindObjectsOfType<NPC>();
        foreach (NPC character in nonPlayerChars) {
            character.InitializeCharacter(_gameManager);
            //Will change this later
             Physics.IgnoreCollision(character.GetComponent<Collider>(), _gameManager.level.player.GetComponent<Collider>());
        }
        
        patrolPointsDictionary = new Dictionary<string, Transform>();
        
        if(patrolPoints.Count > 1) {
            int index = 1;
            foreach(Transform point in patrolPoints) {
                string key = "Patrol " + index.ToString();
                patrolPointsDictionary.Add(key, point);
                index++;
            }
        }
    }
    /// <summary>
    /// Spawns a new NPC and gives them a directive
    /// </summary>
    /// <param name="location"></param>
    public void SpawnNewNPC(Vector3 location, AIState behaviour) {
        GameObject clone = Instantiate(NPCPrefab, location, NPCPrefab.transform.rotation);
        NPC npc = clone.GetComponent<NPC>();
        switch (behaviour) {
            case AIState.Shop: {
                    SetupCustomerNPC(npc);
                    break;
                }
        }
        NPCList.Add(npc);
    }
    void SetupPatrolNPC(NPC npc) {
        npc.initialBehaviour = AIState.Patrol;
        npc.InitializeCharacter(_gameManager);
    }
    void SetupCustomerNPC(NPC npc) {
        npc.initialBehaviour = AIState.Shop;
        npc._name = "Bob";
        npc.InitializeCharacter(_gameManager);
        ShoppingBehaviour behaviour = npc.GetBehaviour(typeof(ShoppingBehaviour)) as ShoppingBehaviour;
        behaviour.decisionList = shopDecisionList;
        behaviour.desiredItem = "Crystal"; // TODO : add in a algorithm for buying items
    }
    public void CreateNPCsForTrade() {
        SpawnNewNPC(NPCSpawnLocation.position, AIState.Shop);
    }

    public void UpdateNPCList() {
        if(NPCList.Count > 0) {
            foreach(NPC n in NPCList) {
                n.UpdateCharacter();
            }
        }
    }
}
