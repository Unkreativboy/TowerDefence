using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingConstruction : MonoBehaviour
{
    private ActionOnTimer actionOnTimer;
    private float constructionTime;
    private BuildingTypeSO buildingType;

    public static BuildingConstruction Create(Vector3 position, BuildingTypeSO buildingType)
    {
        Transform pfBuildingConstruction = Resources.Load<Transform>("pfBuildingConstruction");
        Transform buildingConstructionTransform = Instantiate(pfBuildingConstruction, position, Quaternion.identity);

        BuildingConstruction buildingConstruction = buildingConstructionTransform.GetComponent<BuildingConstruction>();
        buildingConstruction.SetUp(2f, buildingType);

        return buildingConstruction;
    }



    private void SetUp(float constructionTime, BuildingTypeSO buildingType)
    {
        this.constructionTime = constructionTime;
        this.buildingType = buildingType;
        CreateActionOnTimer(constructionTime);

    }

    private void CreateActionOnTimer(float constructionTime)
    {
        actionOnTimer  = gameObject.AddComponent<ActionOnTimer>();
        actionOnTimer.SetTimer(constructionTime, () => 
        {
            Instantiate(buildingType.prefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        });
    }


}
