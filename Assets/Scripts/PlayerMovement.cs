using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { UP, DOWN, LEFT, RIGHT }

public class PlayerMovement : MonoBehaviour
{
    GameObject[,] tileMap;
    public int positionX, positionY;

    float time = 0.0f;
    float movement = 1.0f;

    public float movementPerSecond = 0.4f;

    MapGenerator mapGenerator;
    Direction direction;

    public bool allowMove = true;

    GameObject floorTile;
    // Start is called before the first frame update
    void Start()
    {
        floorTile = GameObject.FindGameObjectWithTag("MapGenerator").GetComponent<MapGenerator>().floorPrefab;
        mapGenerator = GameObject.FindGameObjectWithTag("MapGenerator").GetComponent<MapGenerator>();
        tileMap = mapGenerator.gridTiles;
        positionX = mapGenerator.playerPositionX;
        positionY = mapGenerator.playerPositionY;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time >= movementPerSecond)
        {
            time = 0.0f;
            if (allowMove) InputControls();
        }
    }

    void InputControls()
    {
        float inputDirectionHorizontal = Input.GetAxis("Horizontal");
        float inputDirectionVertical = Input.GetAxis("Vertical");
        bool doubleMovement = false, gettingInput = false;

        if (inputDirectionHorizontal != 0 && inputDirectionVertical != 0) doubleMovement = true; else doubleMovement = false;
        if (inputDirectionHorizontal != 0 || inputDirectionVertical != 0) gettingInput = true; else gettingInput = false;

        if (doubleMovement == false && gettingInput == true)
        {
            float x = 0f, y = 1f, z=0f;
            int beforeX = positionX, beforeY = positionY;
            if (inputDirectionHorizontal > 0)
            {
                x = transform.position.x + movement;
                y = transform.position.y;
                z = transform.position.z;
                positionX += 1;
                direction = Direction.RIGHT;
            }
            else if (inputDirectionHorizontal < 0)
            {
                x = transform.position.x - movement;
                y = transform.position.y;
                z = transform.position.z;
                positionX -= 1;
                direction = Direction.LEFT;
            }

            if(inputDirectionVertical > 0)
            {
                x = transform.position.x;
                y = transform.position.y;
                z = transform.position.z + movement;
                positionY += 1;
                direction = Direction.UP;
            }
            else if(inputDirectionVertical < 0)
            {
                x = transform.position.x;
                y = transform.position.y;
                z = transform.position.z - movement;
                positionY -= 1;
                direction = Direction.DOWN;
            }

            if(!CheckObstackles(positionX, positionY))
            {
                MovePlayer(x, y, z);
            }
            else if(CheckForCubes(positionX, positionY))
            {
                bool cubeMoved = CubeMoved(positionX, positionY, direction);
                if (cubeMoved == true) 
                    MovePlayer(x, y, z);
                else
                {
                    positionX = beforeX;
                    positionY = beforeY;
                }
            }
            else
            {
                positionX = beforeX;
                positionY = beforeY;
            }
        }
    }

    void MovePlayer(float x,float y,float z)
    {
        Vector3 position = new Vector3(x, y, z);
        transform.position = position;
    }

    bool CubeMoved(int x, int y, Direction direction)
    {
        int cubeX = x, cubeY = y;
        GameObject cubeObject = tileMap[x, y];
        if (cubeObject.GetComponent<CubeControl>().victoryTile == true) return true;

        switch (direction)
        {
            case Direction.UP:
                cubeY = y + 1;
                break;
            case Direction.DOWN:
                cubeY = y - 1;
                break;
            case Direction.LEFT:
                cubeX = x - 1;
                break;
            case Direction.RIGHT:
                cubeX = x + 1;
                break;
            default:
                break;
        }

        if (CheckObstackles(cubeX, cubeY) == false)
        {
            if (cubeObject.GetComponent<CubeControl>().Moved() == true)
            {
                SetTile(x, y, floorTile);
                MoveCube(cubeObject, cubeX, cubeY);
                return true;
            }
            else return false;
        }
        else if(CheckForCubes(cubeX, cubeY) == true)
        {
            GameObject nextTileObject = tileMap[cubeX, cubeY];
            if (nextTileObject.GetComponent<CubeControl>().victoryTile == true) 
            {
                if (cubeObject.GetComponent<CubeControl>().Moved() == true)
                {
                    SetTile(x, y, floorTile);
                    MoveCube(cubeObject, cubeX, cubeY);

                    cubeObject.GetComponent<CubeControl>().movementRestricted = true;
                    cubeObject.GetComponent<CubeControl>().movementCount = 0;
                    return true;
                }
                else return false;
            }
            else return false;
        }
        else return false;
    }

    void MoveCube(GameObject Cube,int x,int y)
    {
        Vector3 position = new Vector3(x, Cube.transform.position.y, y);
        Cube.transform.position = position;

        Cube.GetComponent<CubeControl>().CubePositionX = x;
        Cube.GetComponent<CubeControl>().CubePositionY = y;

        if (tileMap[x, y].tag == "Pit") SetTile(x, y, floorTile);
        else SetTile(x, y, Cube);

        setTileMap();
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>().CheckVictoryCondition();
    }

    void SetTile(int x,int y,GameObject tile)
    {
        tileMap[x, y] = tile;
    }

    bool CheckObstackles(int x, int y)
    {
        GameObject tileObject = tileMap[x, y];
        if (tileObject.tag == "Wall" || tileObject.tag == "RedCube" || tileObject.tag == "GreenCube") return true; else return false;
    }

    bool CheckforWalls(int x,int y)
    {
        GameObject tileObject = tileMap[x, y];
        if (tileObject.tag == "Wall") return true; else return false;
    }

    bool CheckForCubes(int x,int y)
    {
        GameObject tileObject = tileMap[x, y];
        if (tileObject.tag == "RedCube" || tileObject.tag == "GreenCube") return true; else return false;
    }

    void setTileMap()
    {
        GameObject.FindGameObjectWithTag("MapGenerator").GetComponent<MapGenerator>().gridTiles = tileMap;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Pit")
        {
            Debug.Log("I fall into the pit");
        }
    }
}
