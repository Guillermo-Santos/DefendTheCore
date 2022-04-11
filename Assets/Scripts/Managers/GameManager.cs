using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject gameOverUI;
    public GameObject completeLevelUI;
    public SceneFader sceneFader;
    public bool Editor = false;
    [HideInInspector]
    public static bool isGameOver;
    public static bool isEditor;
    void Start()
    {
        isGameOver = false;    
        isEditor = Editor;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
            return;
        if(PlayerStats.Lives <= 0)
        {
            EndGame();
            return;
        }
    }

    void EndGame()
    {
        isGameOver = true;
        gameOverUI.SetActive(true);
    }

    public void WinLevel()
    {

        isGameOver = true;
        completeLevelUI.SetActive(true);
    }
}
