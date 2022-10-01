using System.Collections.Generic;
using UnityEngine;

//script setups the labyrinth on first player entry 
public class InLabyrinth : MonoBehaviour
{ 
    [SerializeField] private CompanionController companionController;
    [SerializeField] private List<GameObject> spawnpointsTreasurechest;
    private int randomIndexTreasurechest;
    [SerializeField] private GameObject treasurechest;
    [SerializeField] private bool spawnLock = false;
    public bool SpawnLock{get => spawnLock; set => spawnLock = value;}

    private void Start()
    {
        companionController = GameObject.FindGameObjectWithTag("NPC").GetComponent<CompanionController>();

        foreach(GameObject spawnpoint in GameObject.FindGameObjectsWithTag("SpawnpointTreasurechest"))
        {
            spawnpointsTreasurechest.Add(spawnpoint);
        }
    }
    //spawns treasure chest on random spawnpoint and blocks multiple chest spawns
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            
            randomIndexTreasurechest = Random.Range(0,6);
            companionController.InLabyrinth = true;
            if(spawnLock == false && companionController.InLabyrinth == true)
            {
                Instantiate(treasurechest, spawnpointsTreasurechest[randomIndexTreasurechest].transform.position, Quaternion.identity);
                spawnLock = true;
           
                companionController.IsHiding = true;
            }
        }
    }
}
