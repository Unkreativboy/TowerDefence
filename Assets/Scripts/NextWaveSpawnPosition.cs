using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NextWaveSpawnPosition : MonoBehaviour
{
    [SerializeField] private EnemyWaveSpawner waveSpawner;

    private void Start()
    {

        waveSpawner.OnNextSpawnPositionSet += WaveSpawner_OnNextSpawnPositionSet;
    }
    private void OnDestroy()
    {
        waveSpawner.OnNextSpawnPositionSet -= WaveSpawner_OnNextSpawnPositionSet;

    }

    


    private void WaveSpawner_OnNextSpawnPositionSet(object sender, EnemyWaveSpawner.OnNextSpawnPositionSetEventArgs e)
    {
        transform.position = e.nextSpawnPosition;
    }
}
