using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] 
    public GameObject floorPrefab;

    [SerializeField]
    GameObject playerPrefab;

    [SerializeField]
    GameObject wallPrefab;

    [SerializeField]
    GameObject pitPrefab;

    [SerializeField]
    GameObject redCubePrefab;

    [SerializeField]
    GameObject greenCubePrefab;

    public int width,height;
    public int playerPositionX, playerPositionY;
    int level;

    public GameObject[,] gridTiles;
    List<GameObject> mapObjects;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateMap()
    {
        level = GameObject.FindGameObjectWithTag("GameDatas").GetComponent<GameDatas>().level;

        GenerateTilesAndWalls();
        SetLevelObjects();

        GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>().mapObjects = this.mapObjects;
    }

    void GenerateTilesAndWalls()
    {
        gridTiles = new GameObject[width, height];
        mapObjects = new List<GameObject>();

        for (int x = 0; x < width; x++)
        {
            GameObject mapObject;
            for (int y = 0; y < height; y++)
            {
                mapObject = Instantiate(floorPrefab, new Vector3(x, 0, y), Quaternion.identity);
                mapObject.transform.SetParent(gameObject.transform);
                mapObject.name = "Tile " + x + "," + y;
                mapObjects.Add(mapObject);
                if (x == 0 || x == (width - 1) || y == 0 || y == (height - 1))
                {
                    mapObject = Instantiate(wallPrefab, new Vector3(x, 1.5f, y), Quaternion.identity);
                    mapObject.transform.SetParent(gameObject.transform);
                    mapObject.name = "Wall " + x + "," + y;
                    mapObjects.Add(mapObject);
                }
                gridTiles[x, y] = mapObject;
            }
        }
    }

    void SetLevelObjects()
    {   
        if (level == 1)
        {
            playerPositionX = 2;
            playerPositionY = 11;

            SpawnPlayer(new Vector3(2f,1.5f,11f));
            SpawnWalls(4,4,8,1);
            SpawnWalls(4,4,8,10);
            
            SpawnRedCube(new Vector3(3f, 1f, 7f),3,7,false);
            SpawnVictoryConditionPoint(16, 10, SpawnRedCubeFinishTile(16, 10));
        }
        else if (level == 2)
        {
            playerPositionX = 15;
            playerPositionY = 11;

            SpawnPlayer(new Vector3(15f, 1.5f, 11f));
            SpawnPits(1,12,10,1);
            
            SpawnGreenCube(new Vector3(14f, 1f, 7f), 14, 7,false);
            SpawnGreenCube(new Vector3(17f, 1f, 9f), 17, 9,false);
            SpawnVictoryConditionPoint(4, 10, SpawnGreenCubeFinishTile(4, 10));

            SpawnRedCube(new Vector3(2f, 1f, 6f), 2, 6,false);
            SpawnVictoryConditionPoint(17, 5, SpawnRedCubeFinishTile(17, 5));
        }
        else if(level == 3)
        {
            playerPositionX = 4;
            playerPositionY = 10;

            SpawnPlayer(new Vector3(4f, 1.5f, 10f));
            SpawnWalls(1, 3, 7, 8);
            SpawnWalls(3, 1, 14, 5);

            SpawnPits(1, 12, 10, 1);

            SpawnGreenCube(new Vector3(5f, 1f, 5f), 5, 5, false);

            SpawnRedCube(new Vector3(6f, 1f, 5f), 6, 5, false);
            SpawnRedCube(new Vector3(13f, 1f, 7f), 13, 7, true);

            SpawnVictoryConditionPoint(13, 10, SpawnRedCubeFinishTile(13, 10));

            SpawnVictoryConditionPoint(17, 7, SpawnRedCubeFinishTile(17, 7));
        }
    }

    void SpawnPlayer(Vector3 playerStartPosition)
    {
        GameObject player = Instantiate(playerPrefab, playerStartPosition, Quaternion.identity);
        player.name = "Player";
        player.transform.SetParent(GameObject.FindGameObjectWithTag("LevelController").transform);
        mapObjects.Add(player);
        gridTiles[playerPositionX, playerPositionY] = playerPrefab;
    }

    void SpawnWalls(int width,int height,int startPositionX,int startPositionY)
    {
        int finishPositionX = startPositionX + width;
        int finishPositionY = startPositionY + height;

        for (int x = startPositionX; x < finishPositionX; x++)
        {
            for (int y = startPositionY; y < finishPositionY; y++)
            {
                GameObject wallObject = Instantiate(wallPrefab, new Vector3(x, 1.5f, y), Quaternion.identity);
                wallObject.name = "Wall " + x + "," + y;
                wallObject.transform.SetParent(gameObject.transform);
                mapObjects.Add(wallObject);
                gridTiles[x, y] = wallObject;
            }
        }
    }

    void SpawnRedCube(Vector3 spawnPosition,int x,int y,bool movementRestricted)
    {
        GameObject redCube = Instantiate(redCubePrefab, spawnPosition, Quaternion.identity);
        redCube.GetComponent<Rigidbody>().useGravity = true;
        redCube.name = "Red Cube";
        redCube.transform.SetParent(GameObject.FindGameObjectWithTag("LevelController").transform);
        redCube.GetComponent<CubeControl>().CubePositionX = x;
        redCube.GetComponent<CubeControl>().CubePositionY = y;
        redCube.GetComponent<CubeControl>().movementRestricted = movementRestricted;
        mapObjects.Add(redCube);
        gridTiles[x, y] = redCube;
    }

    void SpawnGreenCube(Vector3 spawnPosition, int x, int y, bool movementRestricted)
    {
        GameObject greenCube = Instantiate(greenCubePrefab, spawnPosition, Quaternion.identity);
        greenCube.GetComponent<Rigidbody>().useGravity = true;
        greenCube.name = "Green Cube";
        greenCube.transform.SetParent(GameObject.FindGameObjectWithTag("LevelController").transform);
        greenCube.GetComponent<CubeControl>().CubePositionX = x;
        greenCube.GetComponent<CubeControl>().CubePositionY = y;
        greenCube.GetComponent<CubeControl>().movementRestricted = movementRestricted;
        mapObjects.Add(greenCube);

        gridTiles[x, y] = greenCube;
    }

    GameObject SpawnRedCubeFinishTile(int spawnPositionX,int spawnPositionY)
    {
        float x = Mathf.FloorToInt(spawnPositionX);
        float y = Mathf.FloorToInt(spawnPositionY);

        Destroy(gridTiles[spawnPositionX, spawnPositionY]);
        GameObject redCubeTile = redCubePrefab;
        redCubeTile.GetComponent<Rigidbody>().useGravity = false;

        redCubeTile = Instantiate(redCubeTile, new Vector3(x, 0f, y), Quaternion.identity);
        redCubeTile.name = "Red Cube Tile " + spawnPositionX + "," + spawnPositionY;
        redCubeTile.GetComponent<CubeControl>().CubePositionX = spawnPositionX;
        redCubeTile.GetComponent<CubeControl>().CubePositionY = spawnPositionY;
        redCubeTile.transform.SetParent(gameObject.transform);

        mapObjects.Add(redCubeTile);

        gridTiles[spawnPositionX, spawnPositionY] = redCubeTile;

        return redCubeTile;
    }

    GameObject SpawnGreenCubeFinishTile(int spawnPositionX, int spawnPositionY)
    {
        float x = Mathf.FloorToInt(spawnPositionX);
        float y = Mathf.FloorToInt(spawnPositionY);

        Destroy(gridTiles[spawnPositionX, spawnPositionY]);
        GameObject greenCubeTile = greenCubePrefab;
        greenCubeTile.GetComponent<Rigidbody>().useGravity = false;
        greenCubeTile = Instantiate(greenCubeTile, new Vector3(x, 0f, y), Quaternion.identity);
        greenCubeTile.transform.SetParent(gameObject.transform);
        greenCubeTile.name = "Green Cube Tile " + spawnPositionX + "," + spawnPositionY;

        mapObjects.Add(greenCubeTile);
        gridTiles[spawnPositionX, spawnPositionY] = greenCubeTile;

        return greenCubeTile;
    }

    void SpawnVictoryConditionPoint(int x,int y, GameObject cube)
    {
        VictoryPoint victoryPoint = new VictoryPoint();
        victoryPoint.x = x;
        victoryPoint.y = y;
        victoryPoint.cubeObject = cube;
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>().victoryPoints.Add(victoryPoint);
    }

    void SpawnPits(int width, int height, int startPositionX, int startPositionY)
    {
        int finishPositionX = startPositionX + width;
        int finishPositionY = startPositionY + height;

        for (int x = startPositionX; x < finishPositionX; x++)
        {
            for (int y = startPositionY; y < finishPositionY; y++)
            {
                Destroy(gridTiles[x, y]);
                GameObject pitObject = Instantiate(pitPrefab, new Vector3(x, -1f, y), Quaternion.identity);
                pitObject.name = "Pit " + x + "," + y;
                pitObject.transform.SetParent(gameObject.transform);
                mapObjects.Add(pitObject);
                gridTiles[x, y] = pitObject;
            }
        }
    }
}
