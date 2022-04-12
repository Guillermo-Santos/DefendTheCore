using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject gameOverUI;
    public AudioClip gameOverClip;
    public GameObject completeLevelUI;
    public AudioClip completeLeveClip;
    public GameObject CORE;
    public SceneFader sceneFader;
    public bool Editor = false;
    [HideInInspector]
    public static bool isGameOver;
    public static bool isEditor;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
        audioSource.loop = false;
        audioSource.clip = gameOverClip;
        audioSource.Play();
        isGameOver = true;
        gameOverUI.SetActive(true);
    }

    public void WinLevel()
    {
        if(isGameOver)
            return;
        audioSource.loop = false;
        audioSource.clip = completeLeveClip;
        audioSource.Play();
        isGameOver = true;
        completeLevelUI.SetActive(true);
    }
}
