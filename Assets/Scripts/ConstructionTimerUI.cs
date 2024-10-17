using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionTimerUI : MonoBehaviour
{
    [SerializeField] private BuildingConstruction buildingConstruction;
    [SerializeField] private Image image;

    private ActionOnTimer actionOnTimer;
    private void Start()
    {
        actionOnTimer = buildingConstruction.GetActionOnTimer();
    }

    private void Update()
    {
        image.fillAmount = actionOnTimer.GetCurrentTimeNormalized();
    }


}
