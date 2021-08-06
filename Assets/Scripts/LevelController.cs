using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public List<VictoryPoint> victoryPoints;
    public List<GameObject> mapObjects;

    [SerializeField]
    Canvas FinishLevelCanvas;

    [SerializeField]
    Text finishGameText;

    // Start is called before the first frame update
    void Start()
    {
        victoryPoints = new List<VictoryPoint>();
        mapObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CheckVictoryCondition()
    {
        bool allVictoryConditionsTrue = true;
        GameObject[,] grids = GameObject.FindGameObjectWithTag("MapGenerator").GetComponent<MapGenerator>().gridTiles;

        for (int i = 0; i < victoryPoints.Count; i++)
        {
            VictoryPoint victoryPoint = victoryPoints[i];
            GameObject cube = victoryPoint.cubeObject;
            CubeControl cubeControl = cube.GetComponent<CubeControl>();

            GameObject gridObject = grids[victoryPoint.x, victoryPoint.y];
            if (cubeControl.Collided == true && gridObject.tag == cube.tag && gridObject.GetComponent<CubeControl>().victoryTile == false)
            {
                cubeControl.playParticles();
                allVictoryConditionsTrue = true;
            }
            else
            {
                allVictoryConditionsTrue = false;
                break;
            }
        }

        if (allVictoryConditionsTrue)
        {
            ShowLevelFinishUI();
        }
    }

    public void DestroyOldMap()
    {
        for (int i = 0; i < mapObjects.Count; i++)
        {
            Destroy(mapObjects[i]);
        }
        mapObjects.Clear();

        Camera.main.GetComponent<TopDownCamera>().playerFound = false;
        victoryPoints.Clear();
    }

    void ShowLevelFinishUI()
    {
        int level = GameObject.FindGameObjectWithTag("GameDatas").GetComponent<GameDatas>().level;
        SaveSystem.SaveLevel(GameObject.FindGameObjectWithTag("GameDatas").GetComponent<GameDatas>());
        finishGameText.text = "Bölüm " + level.ToString() + " Tamamlandý";
        FinishLevelCanvas.enabled = true;
    }
}
