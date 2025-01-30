using System.Collections;
using UnityEngine;

public class EvilGuySpawning : MonoBehaviour
{
    public GameObject EvilGuyPrefab;
    public float SecondsBetweenWaves = 4;
    public float SpawnDelayTimer = 1;
    public int EnemiesPerWaveMin = 1;
    public int EnemiesPerWaveMax = 5;

    public float SpeedFactorMin = 1;
    public float SpeedFactorMax = 2;

    public Transform RightSpawn;
    public Transform LeftSpawn;

    private bool canSpawn = false;
    private float spawnTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnTimer = SecondsBetweenWaves;    
        //canSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canSpawn)
        {
            return;
        }

        spawnTimer -= Time.deltaTime;

        if(spawnTimer < 0 )
        {
            StartCoroutine(SpawnDudes());
        }
    }

    private IEnumerator SpawnDudes()
    {
        canSpawn = false;

        int enemyAmount = Random.Range(EnemiesPerWaveMin, EnemiesPerWaveMax+1);
        bool rightSpawn = true;

        for(int i = 0; i < enemyAmount; i++)
        {
            if(rightSpawn)
            {
                rightSpawn = false;
                GameObject evil = Instantiate(EvilGuyPrefab, RightSpawn.position, Quaternion.identity);
                evil.GetComponent<EvilGuy>().MoveSpeed = 
                    evil.GetComponent<EvilGuy>().MoveSpeed * Random.Range(SpeedFactorMin,SpeedFactorMax);
            }
            else
            {
                rightSpawn = true;

                GameObject evil = Instantiate(EvilGuyPrefab, LeftSpawn.position, Quaternion.identity);
                evil.GetComponent<EvilGuy>().MoveSpeed =
                    evil.GetComponent<EvilGuy>().MoveSpeed * Random.Range(SpeedFactorMin + 0.1f, SpeedFactorMax);
            }

            yield return new WaitForSeconds(SpawnDelayTimer);
        }

        spawnTimer = SecondsBetweenWaves;
        canSpawn = true;
    }

    public void StopSpawning()
    {
        canSpawn = false;

        StopAllCoroutines();
    }

    public void EnableSpawning()
    {
        spawnTimer = SecondsBetweenWaves;
        canSpawn=true;
    }
}
