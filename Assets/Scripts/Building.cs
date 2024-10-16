using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public static event EventHandler OnBuildingDestroyed;
    public static event EventHandler OnBuildingBuild;

    private BuildingTypeSO buildingType;
    private HealthSystem healthSystem;

    private void Start()
    {
        OnBuildingBuild?.Invoke(this, EventArgs.Empty);
        buildingType = GetComponent<BuildingTypeHolder>().buildingType;
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.SetHealthAmountMax(buildingType.healthAmountMax, true);

        healthSystem.OnDied += HealthSystem_OnDied;
    }
    private void OnDestroy()
    {
        healthSystem.OnDied -= HealthSystem_OnDied;
    }
   



    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        OnBuildingDestroyed?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }



}