using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class EnemyWaveUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyWaveSpawner waveSpawner;
    [SerializeField] private TextMeshProUGUI waveNumberText;
    [SerializeField] private TextMeshProUGUI waveMessageText;
    [SerializeField] private RectTransform enemyWaveSpawnPositionIndicator;
    [SerializeField] private RectTransform closesEnemyPositionIndicator;
    [Header("Settings")]
    [SerializeField] private float waveSpawnIndicatorOffsetToCenter;
    [SerializeField] private float nearestEnemyIndicatorOffsetToCenter;

    private Camera mainCamera;
    private ActionOnTimer waveTimer;

    private Vector3 nextWaveSpawnPosition;

    private void Start()
    {
        mainCamera = Camera.main;
        waveTimer = waveSpawner.GetWaveTimer();
        waveSpawner.OnWaveNumberUpdate += WaveSpawner_OnWaveNumberUpdate;
        waveSpawner.OnNextSpawnPositionSet += WaveSpawner_OnNextSpawnPositionSet;
    }

    private void WaveSpawner_OnNextSpawnPositionSet(object sender, EnemyWaveSpawner.OnNextSpawnPositionSetEventArgs e)
    {
        nextWaveSpawnPosition = e.nextSpawnPosition;
        
    }

    private void OnDestroy()
    {
        waveSpawner.OnWaveNumberUpdate -= WaveSpawner_OnWaveNumberUpdate;
        waveSpawner.OnNextSpawnPositionSet -= WaveSpawner_OnNextSpawnPositionSet;

    }

    private void Update()
    {
        HandleWaveTimerText();

        //SpawnPositonIndicator
        PointTowardsPositionWithIndicator(nextWaveSpawnPosition, enemyWaveSpawnPositionIndicator, waveSpawnIndicatorOffsetToCenter);
        //NearestEnemyIndicator


        Enemy closesEnemy = EnemyManager.Instance.GetClosestEnemy(mainCamera.transform.position);
        if (closesEnemy == null) return;
        Vector3 nearestEnemyPosition = EnemyManager.Instance.GetClosestEnemy(mainCamera.transform.position).transform.position;
        PointTowardsPositionWithIndicator(nearestEnemyPosition, closesEnemyPositionIndicator, nearestEnemyIndicatorOffsetToCenter);
    }

    private void HandleWaveTimerText()
    {
        if (waveTimer.GetCurrentTime() > 0)
        {
            waveMessageText.SetText("Next Wave in " + waveTimer.GetCurrentTime().ToString("F1") + "s");
        }
        else
        {
            waveMessageText.SetText("");
        }
    }

    private void PointTowardsPositionWithIndicator(Vector3 targetPosition, RectTransform indicator, float offSetToCenter)
    {
        Vector3 dirToNextSpawnPosition = (targetPosition - mainCamera.transform.position).normalized;
        indicator.anchoredPosition = dirToNextSpawnPosition * offSetToCenter;
        indicator.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(dirToNextSpawnPosition));


        float distanceToNextSpawnPosition = Vector3.Distance(targetPosition, mainCamera.transform.position);
        indicator.gameObject.SetActive(distanceToNextSpawnPosition > mainCamera.orthographicSize * 1.5f);
    }



    private void HandleWaveSpawnPositionIndicator()
    {
        Vector3 dirToNextSpawnPosition = (nextWaveSpawnPosition - mainCamera.transform.position).normalized;
        enemyWaveSpawnPositionIndicator.anchoredPosition = dirToNextSpawnPosition * waveSpawnIndicatorOffsetToCenter;
        enemyWaveSpawnPositionIndicator.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(dirToNextSpawnPosition));


        float distanceToNextSpawnPosition = Vector3.Distance(nextWaveSpawnPosition, mainCamera.transform.position);
        enemyWaveSpawnPositionIndicator.gameObject.SetActive(distanceToNextSpawnPosition > mainCamera.orthographicSize * 1.5f);
    }


    private void WaveSpawner_OnWaveNumberUpdate(object sender, EnemyWaveSpawner.OnWaveNumberUpdateEventArgs e)
    {
        waveNumberText.SetText("Wave: "+e.waveNumber.ToString());
    }


}
