using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { UP, DOWN, LEFT, RIGHT }

public class PlayerMovement : MonoBehaviour
{
    GameObject[,] tileMap;
    int positionX, positionY;

    float time = 0.0f;
    public float movementPerSecond = 0.4f;

    MapGenerator mapGenerator;
    Direction direction;

    public bool allowMove = true;
    // Start is called before the first frame update
    void Start()
    {
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
        float movement = 1.0f;
        float inputDirectionHorizontal = Input.GetAxis("Horizontal");
        float inputDirectionVertical = Input.GetAxis("Vertical");
        bool doubleMovement = false;
        bool gettingInput = false;

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

            if(!CheckforWalls(positionX, positionY))
            {
                bool allowCharacterMovement = CheckCubesOnDirection(positionX, positionY,direction);
                if (allowCharacterMovement)
                {
                    Vector3 position = new Vector3(x, y, z);
                    transform.position = position;
                    tileMap[positionX, positionY] = gameObject;
                    tileMap[beforeX, beforeY] = mapGenerator.floorPrefab;
                    setTileMap();
                }
            }
            else
            {
                positionX = beforeX;
                positionY = beforeY;
            }
        }
    }

    bool CheckCubesOnDirection(int x, int y, Direction direction)
    {
        bool allowCharacterMovement = false;
        GameObject tileObject = tileMap[x, y];
        int newX = x, newY = y;
        if (tileObject.tag == "RedCube" || tileObject.tag == "GreenCube")
        {
            bool wallOnDirection = false;
            bool cubeOndirection = false;
            switch (direction)
            {
                case Direction.UP:
                    wallOnDirection = CheckforWalls(x, y + 1);
                    cubeOndirection = CheckForCubes(x, y + 1);
                    newY = y + 1;
                    break;
                case Direction.DOWN:
                    wallOnDirection = CheckforWalls(x, y - 1);
                    cubeOndirection = CheckForCubes(x, y - 1);
                    newY = y - 1;
                    break;
                case Direction.LEFT:
                    wallOnDirection = CheckforWalls(x - 1, y);
                    cubeOndirection = CheckForCubes(x - 1, y);
                    newX = x - 1;
                    break;
                case Direction.RIGHT:
                    wallOnDirection = CheckforWalls(x + 1, y);
                    cubeOndirection = CheckForCubes(x + 1, y);
                    newX = x + 1;
                    break;
                default:
                    break;
            }

            if (!wallOnDirection && !cubeOndirection)
            {
                if(tileObject.GetComponent<CubeControl>().Moved() == true)
                {
                    Vector3 position = new Vector3(newX, tileObject.transform.position.y, newY);
                    tileObject.transform.position = position;

                    tileObject.GetComponent<CubeControl>().CubePositionX = newX;
                    tileObject.GetComponent<CubeControl>().CubePositionY = newY;

                    if(tileMap[newX, newY].tag == "Pit")
                    {
                        tileMap[newX, newY] = GameObject.FindGameObjectWithTag("MapGenerator").GetComponent<MapGenerator>().floorPrefab;
                    }
                    else
                    {
                        tileMap[newX, newY] = tileObject;
                    }

                    setTileMap();
                    GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>().CheckVictoryCondition();

                    allowCharacterMovement = true;
                }
                else
                {
                    allowCharacterMovement = false;
                }
            }
            else
            {
                allowCharacterMovement = false;
            }
        }
        else
        {
            allowCharacterMovement = true;
        }
        return allowCharacterMovement;
    }

    bool CheckforWalls(int x,int y)
    {
        GameObject tileObject = tileMap[x, y];
        if (tileObject.tag == "Wall") return true; else return false;
    }

    bool CheckForCubes(int x,int y)
    {
        GameObject tileObject = tileMap[x, y];
        if (tileObject.name == "Red Cube" || tileObject.name == "Green Cube") return true; else return false;
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
