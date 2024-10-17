using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingConstruction : MonoBehaviour
{

    [SerializeField] private SpriteRenderer spriteRenderer;
    


    private ActionOnTimer actionOnTimer;
    private BuildingTypeSO buildingType;
    private BoxCollider2D boxCollider2D;
    private BuildingTypeHolder buildingTypeHolder;
    private Material constructionMaterial;


    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        buildingTypeHolder = GetComponent<BuildingTypeHolder>();
        constructionMaterial = spriteRenderer.material;
    }


    public static BuildingConstruction Create(Vector3 position, BuildingTypeSO buildingType)
    {
        Transform pfBuildingConstruction = Resources.Load<Transform>("pfBuildingConstruction");
        Transform buildingConstructionTransform = Instantiate(pfBuildingConstruction, position, Quaternion.identity);

        BuildingConstruction buildingConstruction = buildingConstructionTransform.GetComponent<BuildingConstruction>();
        buildingConstruction.SetUpBuildingTypeAndStartTimer(buildingType);

        return buildingConstruction;
    }
    private void Update()
    {
        //Invert the GetCurrentTimeNormalized to start from 0 otherwise it would start at 1 and decline
        constructionMaterial.SetFloat("_Progress", (actionOnTimer.GetCurrentTimeNormalized() - 1) * (-1));
        
    }




    private void SetUpBuildingTypeAndStartTimer( BuildingTypeSO buildingType)
    {
        spriteRenderer.sprite = buildingType.sprite;

        this.buildingType = buildingType;
        CreateActionOnTimer(buildingType.constructionTime);

        boxCollider2D.offset = buildingType.prefab.GetComponent<BoxCollider2D>().offset;
        boxCollider2D.size = buildingType.prefab.GetComponent<BoxCollider2D>().size;
        buildingTypeHolder.buildingType = buildingType;
       
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

    public ActionOnTimer GetActionOnTimer()
    {
        return actionOnTimer;
    }


}
