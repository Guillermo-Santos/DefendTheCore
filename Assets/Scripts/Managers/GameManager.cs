using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject gameOverUI;
    public GameObject completeLevelUI;
    public GameObject CORE;
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
        if(CORE == null)
        {
            EndGame();
            return;
        }
    }

    void EndGame()
    {
        if (isGameOver)
            return;
        isGameOver = true;
        gameOverUI.SetActive(true);
    }

    public void WinLevel()
    {
        if(isGameOver)
            return;
        isGameOver = true;
        completeLevelUI.SetActive(true);
    }
}
