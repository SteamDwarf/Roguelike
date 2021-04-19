using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonGenerator : MonoBehaviour
{
    private MapDrawer MD;
    //private new CameraScript camera;

    public int mapWidth;
    public int mapHeight;

    public int widthMinRoom;
    public int widthMaxRoom;
    public int heightMinRoom;
    public int heightMaxRoom;

    public int minCorridorLength;
    public int maxCorridorLength;
    public int maxFeatures;
    int countFeatures;

    public int maxEnemies;
    public int maxEnemiesPerRoom;
    public int minEnemiesPerRoom;
    int spawnedEnemiesCount;

    //public bool isASCII;
    //public bool isSprites;
    public float mapk;

    public List<Feature> allFeatures;
    public List<Feature> allRooms;
    public List<GameObject> enemies;
    public List<GameObject> spawnedEnemies;

    public Vector2Int playerSpawnCordinates;

    //public GameObject playerPrefab;
    /*public GameObject playerPrefab;
    public GameObject wallPrefab;
    public Transform mapTransform;
    public List<GameObject> floorPrefab;*/


    public void InitializeDungeon()
    {
        MapManager.map = new Tile[mapWidth, mapHeight];
        MD = GetComponent<MapDrawer>();
        spawnedEnemiesCount = 0;
        mapk = MD.mapk;
        //camera = GetComponent<CameraScript>();
    }

    public void GenerateDungeon()
    {
        GenerateFeature("Room", new Wall(), true);

        for (int i = 0; i < 500; i++)
        {
            Feature originFeature;

            if (allFeatures.Count == 1)
            {
                originFeature = allFeatures[0];
            }
            else
            {
                originFeature = allFeatures[Random.Range(0, allFeatures.Count - 1)];
            }

            Wall wall = ChoseWall(originFeature);
            if (wall == null) continue;

            string type;

            if (originFeature.type == "Room")
            {
                type = "Corridor";
            }
            else
            {
                if (Random.Range(0, 100) < 90)
                {
                    type = "Room";
                }
                else
                {
                    type = "Corridor";
                }
            }

            GenerateFeature(type, wall);

            if (countFeatures >= maxFeatures) break;
        }

        SpawnPlayer();
        SpawnEnemies();
        MD.DrawMap();
    }

    void GenerateFeature(string type, Wall wall, bool isFirst = false)
    {
        Feature room = new Feature();
        room.positions = new List<Vector2Int>();

        int roomWidth = 0;
        int roomHeight = 0;

        if (type == "Room")
        {
            roomWidth = Random.Range(widthMinRoom, widthMaxRoom);
            roomHeight = Random.Range(heightMinRoom, heightMaxRoom);
        }
        else
        {
            switch (wall.direction)
            {
                case "South":
                    roomWidth = 3;
                    roomHeight = Random.Range(minCorridorLength, maxCorridorLength);
                    break;
                case "North":
                    roomWidth = 3;
                    roomHeight = Random.Range(minCorridorLength, maxCorridorLength);
                    break;
                case "West":
                    roomWidth = Random.Range(minCorridorLength, maxCorridorLength);
                    roomHeight = 3;
                    break;
                case "East":
                    roomWidth = Random.Range(minCorridorLength, maxCorridorLength);
                    roomHeight = 3;
                    break;

            }
        }

        int xStartingPoint;
        int yStartingPoint;

        if (isFirst)
        {
            xStartingPoint = mapWidth / 2;
            yStartingPoint = mapHeight / 2;
        }
        else
        {
            int id;
            if (wall.positions.Count == 3) id = 1;
            else id = Random.Range(1, wall.positions.Count - 2);

            xStartingPoint = wall.positions[id].x;
            yStartingPoint = wall.positions[id].y;
        }

        Vector2Int lastWallPosition = new Vector2Int(xStartingPoint, yStartingPoint);

        if (isFirst)
        {
            xStartingPoint -= Random.Range(1, roomWidth);
            yStartingPoint -= Random.Range(1, roomHeight);
        }
        else
        {
            switch (wall.direction)
            {
                case "South":
                    if (type == "Room") xStartingPoint -= Random.Range(1, roomWidth - 2);
                    else xStartingPoint--;
                    yStartingPoint -= Random.Range(1, roomHeight - 2);
                    break;
                case "North":
                    if (type == "Room") xStartingPoint -= Random.Range(1, roomWidth - 2);
                    else xStartingPoint--;
                    yStartingPoint++;
                    break;
                case "West":
                    xStartingPoint -= roomWidth;
                    if (type == "Room") yStartingPoint -= Random.Range(1, roomHeight - 2);
                    else yStartingPoint--;
                    break;
                case "East":
                    xStartingPoint++;
                    if (type == "Room") yStartingPoint -= Random.Range(1, roomHeight - 2);
                    else yStartingPoint--;
                    break;
            }
        }

        if (!CheckIfHasSpace(new Vector2Int(xStartingPoint, yStartingPoint), new Vector2Int(xStartingPoint + roomWidth - 1, yStartingPoint + roomHeight - 1)))
        {
            return;
        }

        room.walls = new Wall[4];

        for (int i = 0; i < room.walls.Length; i++)
        {
            room.walls[i] = new Wall();
            room.walls[i].positions = new List<Vector2Int>();
            room.walls[i].length = 0;

            switch (i)
            {
                case 0:
                    room.walls[i].direction = "South";
                    break;
                case 1:
                    room.walls[i].direction = "North";
                    break;
                case 2:
                    room.walls[i].direction = "West";
                    break;
                case 3:
                    room.walls[i].direction = "East";
                    break;
            }
        }

        for (int y = 0; y < roomHeight; y++)
        {
            for (int x = 0; x < roomWidth; x++)
            {
                Vector2Int position = new Vector2Int();
                position.x = xStartingPoint + x;
                position.y = yStartingPoint + y;

                room.positions.Add(position);

                MapManager.map[position.x, position.y] = new Tile();
                MapManager.map[position.x, position.y].xPosition = position.x;
                MapManager.map[position.x, position.y].yPosition = position.y;

                if (y == 0)
                {
                    room.walls[0].positions.Add(position);
                    room.walls[0].length++;
                    MapManager.map[position.x, position.y].type = "Wall";
                }
                if (y == (roomHeight - 1))
                {
                    room.walls[1].positions.Add(position);
                    room.walls[1].length++;
                    MapManager.map[position.x, position.y].type = "Wall";
                }
                if (x == 0)
                {
                    room.walls[2].positions.Add(position);
                    room.walls[2].length++;
                    MapManager.map[position.x, position.y].type = "Wall";
                }
                if (x == (roomWidth - 1))
                {
                    room.walls[3].positions.Add(position);
                    room.walls[3].length++;
                    MapManager.map[position.x, position.y].type = "Wall";
                }
                if (MapManager.map[position.x, position.y].type != "Wall")
                {
                    MapManager.map[position.x, position.y].type = "Floor";
                }
            }
        }

        if (!isFirst)
        {
            MapManager.map[lastWallPosition.x, lastWallPosition.y].type = "Floor";
            switch (wall.direction)
            {
                case "South":
                    MapManager.map[lastWallPosition.x, lastWallPosition.y - 1].type = "Floor";
                    break;
                case "North":
                    MapManager.map[lastWallPosition.x, lastWallPosition.y + 1].type = "Floor";
                    break;
                case "West":
                    MapManager.map[lastWallPosition.x - 1, lastWallPosition.y].type = "Floor";
                    break;
                case "East":
                    MapManager.map[lastWallPosition.x + 1, lastWallPosition.y].type = "Floor";
                    break;
            }
        }

        room.width = roomWidth;
        room.height = roomHeight;
        room.type = type;
        allFeatures.Add(room);
        countFeatures++;
    }

    bool CheckIfHasSpace(Vector2Int start, Vector2Int end)
    {
        for (int y = start.y; y <= end.y; y++)
        {
            for (int x = start.x; x <= end.x; x++)
            {
                if (x < 0 || y < 0 || x >= mapWidth || y >= mapHeight) return false;
                if (MapManager.map[x, y] != null) return false;
            }
        }

        return true;
    }

    Wall ChoseWall(Feature feature)
    {
        for (int i = 0; i < 10; i++)
        {
            int id = Random.Range(0, 100) / 25;
            if (!feature.walls[id].hasFeature)
            {
                return feature.walls[id];
            }
        }
        return null;
    }

    public void GetRooms()
    {
        foreach (Feature feature in allFeatures)
        {
            if (feature.type == "Room")
            {
                allRooms.Add(feature);
            }
        }
    }

    public void SpawnPlayer()
    {
        GetRooms();
        Feature room = allRooms[0];

        for (int i = 0; i < room.positions.Count; i++)
        {
            Vector2Int cordinates = room.positions[i];

            if (MapManager.map[cordinates.x, cordinates.y].type == "Floor")
            {
                playerSpawnCordinates = cordinates;
                MapManager.map[cordinates.x, cordinates.y].hasPlayer = true;
                room.hasPlayer = true;
                //camera.MoveCamera(new Vector2(cordinates.x, cordinates.y));
                break;

                /*playerSpawnCordinates = cordinates;
                GameObject player = GameObject.Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                player.GetComponent<PlayerMovement>().playerPosition = cordinates;
                MapManager.map[cordinates.x, cordinates.y].hasPlayer = true;
                room.hasPlayer = true;
                GetComponent<GameManager>().player = player.GetComponent<PlayerMovement>();
                */
            }
        }
    }

    public void SpawnEnemies()
    {   while(spawnedEnemiesCount < maxEnemies) {  
            //������� �������� �� ���������� ������ � �������.
            int roomInd = Random.Range(1, allRooms.Count);
            int enemiesSpawn = Random.Range(minEnemiesPerRoom, maxEnemiesPerRoom);
            Feature room = allRooms[roomInd];

            for (int j = 0; j < enemiesSpawn ; j++)
            {
                int tileInd = Random.Range(0, room.positions.Count - 1);
                int enemyInd = Random.Range(0, enemies.Count);

                Vector2Int spawnPoint = room.positions[tileInd];

                if (MapManager.map[spawnPoint.x, spawnPoint.y].type == "Wall")
                    continue;
                else
                {
                    MapManager.map[spawnPoint.x, spawnPoint.y].baseObject = enemies[enemyInd];
                    spawnedEnemies.Add(enemies[enemyInd]);
                    room.enemyInRoom++;
                    spawnedEnemiesCount++;
                }
            }
        }
    }

    /*public void DrawASCII()
    {
        Text screen = GameObject.Find("ASCII").GetComponent<Text>();

        string asciiMap = "";

        for (int y = (mapHeight - 1); y >= 0; y--)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                if (MapManager.map[x, y] != null)
                {
                    if (MapManager.map[x, y].hasPlayer)
                    {
                        asciiMap += "@";
                    }
                    else
                    {
                        switch (MapManager.map[x, y].type)
                        {
                            case "Wall":
                                asciiMap += "#";
                                break;
                            case "Floor":
                                asciiMap += ".";
                                break;
                        }
                    }
                }
                else
                {
                    asciiMap += " ";
                }

                if (x == (mapWidth - 1))
                {
                    asciiMap += "\n";
                }
            }
        }

        screen.text = asciiMap;
    }

    public void DrawSprites()
    {
        
        mapTransform = new GameObject("Map").transform;
        

        for (int y = 0; y < mapHeight - 1; y++)
        {
            for (int x = 0; x < mapWidth - 1; x++)
            {
                if (MapManager.map[x, y] == null)
                    continue;
                if (MapManager.map[x, y].type == "Wall")
                {
                    GameObject toInstantiate = wallPrefab;
                    GameObject instance = Instantiate(toInstantiate, new Vector3(x * mapk, y * mapk, 0), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(mapTransform);

                } else if(MapManager.map[x, y].type == "Floor")
                {
                    int randomNum = Random.Range(0, floorPrefab.Count - 1);
                    GameObject toInstantiate = floorPrefab[randomNum];
                    GameObject instance = Instantiate(toInstantiate, new Vector3(x * mapk, y * mapk, 0), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(mapTransform);
                }
            }
        }

                /*for (int x = -1; x <= columns; x++)
                {
                    for (int y = -1; y <= rows; y++)
                    {
                        GameObject toInstantiate = floorTiles;
                        if (x == -1 || x == columns || y == -1 || y == rows)
                        {
                            toInstantiate = wallTiles;
                        }

                        GameObject instance = Instantiate(toInstantiate, new Vector3Int(x, y, 0), Quaternion.identity) as GameObject;
                        instance.transform.SetParent(boardHolder);
                    }
                }*/
    //}

    /*public void DrawMap(bool isSprites)
    {
        if (isSprites)
            DrawSprites();
        else
            DrawASCII();
    }*/
}
