using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    DungeonGenerator dungeonGenerator;
    //public PlayerMovement player;
    
    void Start() {
        dungeonGenerator = GetComponent<DungeonGenerator>();

        dungeonGenerator.InitializeDungeon();
        dungeonGenerator.GenerateDungeon();
    }
}
