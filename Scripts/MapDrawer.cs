using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapDrawer : MonoBehaviour
{
    private DungeonGenerator dG;

    public GameObject playerPrefab;
    public GameObject wallPrefab;
    public Transform mapTransform;
    public List<GameObject> floorPrefab;
    public List<GameObject> enemies;
    public List<GameObject> spawnedEnemies;

    public float mapk;
    public void DrawMap()
    {
        dG = GetComponent<DungeonGenerator>();
        mapTransform = new GameObject("Map").transform;


        for (int y = 0; y < dG.mapHeight - 1; y++)
        {
            for (int x = 0; x < dG.mapWidth - 1; x++)
            {
                if (MapManager.map[x, y] == null)
                    continue;
                if (MapManager.map[x, y].type == "Wall")
                {
                    GameObject toInstantiate = wallPrefab;
                    GameObject instance = Instantiate(toInstantiate, new Vector3(x * mapk, y * mapk, 0), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(mapTransform);

                }
                else if (MapManager.map[x, y].type == "Floor")
                {
                    int randomNum = Random.Range(0, floorPrefab.Count - 1);
                    GameObject toInstantiate = floorPrefab[randomNum];
                    GameObject instance = Instantiate(toInstantiate, new Vector3(x * mapk, y * mapk, 0), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(mapTransform);
                }

                if (MapManager.map[x, y].hasPlayer == true)
                {
                    GameObject playerSpawn = GameObject.Instantiate(playerPrefab, new Vector3(x * mapk, y * mapk, 0), Quaternion.identity);
                }

                if (MapManager.map[x, y].hasEnemy)
                {
                    int enemyInd = Random.Range(0, enemies.Count);
                    //Vector2Int spawnPoint = new Vector2Int(x, y);

                    GameObject enemy = enemies[enemyInd];
                    GameObject enemyToInst = Instantiate(enemy, new Vector3(x * mapk, y * mapk, 0), Quaternion.identity);

                    MapManager.map[x, y].enemy = enemyToInst;
                    spawnedEnemies.Add(enemies[enemyInd]);
                    //GameObject enemy = MapManager.map[x, y].enemy;

                }
            }
        }
    }
}
