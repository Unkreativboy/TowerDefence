using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveSpawner : MonoBehaviour
{
    public event EventHandler<OnNextSpawnPositionSetEventArgs> OnNextSpawnPositionSet;
    public event EventHandler<OnWaveNumberUpdateEventArgs> OnWaveNumberUpdate;
    public class OnNextSpawnPositionSetEventArgs : EventArgs
    {
        public Vector3 nextSpawnPosition;
    }
   public class OnWaveNumberUpdateEventArgs : EventArgs
    {
        public int waveNumber;
    }


    [SerializeField] private List<Transform> spawnPositionsList;
    [SerializeField] private float spawnTime;
    [SerializeField] private float timeBetweenEnemySpawn;
    private ActionOnTimer waveTimer;
    private ActionOnTimer nextEnemySpawnTimer;

    private int waveNumber = 0;
    private int enemySpawnAmountWave;
    private Vector3 currentSpawnPosition;



    private void Awake()
    {
        waveTimer = gameObject.AddComponent<ActionOnTimer>();
        nextEnemySpawnTimer = gameObject.AddComponent<ActionOnTimer>();
    }


    private void Start()
    {
        SetRandomSpawnPosition();
        SpawnWave();
    }


    private void SpawnEnemy(Vector3 spawnPosition)
    {
        
        Enemy.Create(spawnPosition + UtilsClass.GetRandomDir() * UnityEngine.Random.Range(0, 10));
    }


    private void WaveSpawEnded()
    {
        waveTimer.SetTimer(spawnTime, () => { SpawnWave(); }, false);
        SetRandomSpawnPosition();
    }

    private void SetRandomSpawnPosition()
    {
        int randomIndexNumber = UnityEngine.Random.Range(0, spawnPositionsList.Count);
        currentSpawnPosition = spawnPositionsList[randomIndexNumber].position;
        OnNextSpawnPositionSet?.Invoke(this, new OnNextSpawnPositionSetEventArgs { nextSpawnPosition = currentSpawnPosition });

    }
    private void IncreaseWaveNumber()
    {
        waveNumber++;
        OnWaveNumberUpdate?.Invoke(this, new OnWaveNumberUpdateEventArgs { waveNumber = waveNumber  });
    }

    private void SpawnWave()
    {
        IncreaseWaveNumber();
        int enemyAmount = 5 + 3 * waveNumber;
        nextEnemySpawnTimer.SetTimer(timeBetweenEnemySpawn, enemyAmount, () => { SpawnEnemy(currentSpawnPosition); }, () => { WaveSpawEnded(); });

    }

    public ActionOnTimer GetWaveTimer() {  return waveTimer; }

    

}
