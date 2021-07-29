using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    [SerializeField]
    Canvas gameMenuCanvas;

    [SerializeField]
    Canvas FinishLevelCanvas;

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().allowMove = false;
            gameMenuCanvas.enabled = true;
        }
    }
    // Start is called before the first frame update
    public void ContinueGame()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().allowMove = true;
        gameMenuCanvas.enabled = false;
    }

    public void TryCurrentLevelAgain()
    {
        FinishLevelCanvas.enabled = false;
        gameMenuCanvas.enabled = false;
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>().DestroyOldMap();
        LoadLevel();
    }

    public void NextLevel()
    {
        FinishLevelCanvas.enabled = false;
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>().DestroyOldMap();
        GameObject.FindGameObjectWithTag("GameDatas").GetComponent<GameDatas>().level += 1;
        LoadLevel();
    } 

    void LoadLevel()
    {
        GameObject.FindGameObjectWithTag("MapGenerator").GetComponent<MapGenerator>().GenerateMap();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
