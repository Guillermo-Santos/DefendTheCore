using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{

    public static int EnemiesAlive = 0;

    public Wave[] waves;

    public Transform SpawnPoint;
    public TextMeshProUGUI WaveCountDownText;
    public GameManager gameManager;

    public float WaveCountDown = 10f;
    private int waveIndex = 0;
    private float CountDown = 5f;

    // Update is called once per frame
    void Update()
    {
        if(EnemiesAlive > 0)
        {
            return;
        }
        if (waveIndex >= waves.Length)
        {
            gameManager.WinLevel();
            this.enabled = false;
            return;
        }
        if (CountDown <= 0f)
        {
            StartCoroutine(SpawnWave());
            CountDown = WaveCountDown;
            return;
        }
        CountDown -= Time.deltaTime;
        CountDown = Mathf.Clamp(CountDown,0f,Mathf.Infinity);
        WaveCountDownText.text = string.Format("{0:00.00}",CountDown);
    }

    IEnumerator SpawnWave()
    {
        PlayerStats.Rounds++;

        Wave wave = waves[waveIndex];
        
        EnemiesAlive = wave.count;

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(0.5f);
        }
        waveIndex++;
    }

    void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, SpawnPoint.position, SpawnPoint.rotation);
    }


}
