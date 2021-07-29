using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDatas : MonoBehaviour
{
    public int level;
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
