using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public void NewGame()
    {
        GameObject.FindGameObjectWithTag("GameDatas").GetComponent<GameDatas>().level = 1;
        SceneManager.LoadScene("GameScene");
    }

    public void LoadGame()
    {
        int level = SaveSystem.LoadGameDatas();

        if(level != 0)
        {
            GameObject.FindGameObjectWithTag("GameDatas").GetComponent<GameDatas>().level = level+1;
        }
        else
        {
            GameObject.FindGameObjectWithTag("GameDatas").GetComponent<GameDatas>().level = 1;
        }

        SceneManager.LoadScene("GameScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
