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
    public float CountDown = 30f;
    private int waveIndex = 0;
    private int partitions;


    // Update is called once per frame
    void Update()
    {
        if(EnemiesAlive > 0)
        {
            return;
        }else if (waveIndex >= waves.Length)
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

    private struct rarity
    {
        public int index;
        public int minPartitions;
        public int maxPartitions;
    }

    IEnumerator SpawnWave()
    {
        PlayerStats.Rounds++;

        Wave wave = waves[waveIndex];
        
        EnemiesAlive = wave.count;

        partitions = 0;

        List<rarity> rarityList = new List<rarity>();
        
        for(int cont = 0; cont < wave.enemies.Count; cont++)
        {
            rarity rarity = new rarity();
            rarity.index = cont;
            rarity.minPartitions = partitions;
            rarity.maxPartitions = partitions + wave.enemies[cont].priority;
            rarityList.Add(rarity);
            partitions += wave.enemies[cont].priority;
        }
        for (int i = 0; i < wave.count; i++)
        {
            int partition = Random.Range(0, partitions);
            foreach (rarity rarity in rarityList)
            {
                if (partition <= rarity.maxPartitions && partition >= rarity.minPartitions)
                {
                    SpawnEnemy(wave.enemies[rarity.index].enemy);
                    break;
                }
            }
            yield return new WaitForSeconds(wave.spawnRate);
        }
        waveIndex++;
    }

    void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, SpawnPoint.position, SpawnPoint.rotation);
    }


}
