using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDemolishButton : MonoBehaviour
{
    [SerializeField] private Building building;
    private void Awake()
    {
        BuildingTypeSO buildingType = building.GetComponent<BuildingTypeHolder>().buildingType;

        GetComponent<Button>().onClick.AddListener(() => 
        {
            
            foreach(ResourceAmount resourceAmount in buildingType.constructionResourceCostArray)
            {
                ResourceManager.Instance.AddResource(resourceAmount.resourceType, Mathf.FloorToInt(resourceAmount.amount * 0.6f) );
            } 


            Destroy(building.gameObject);
        });
    }

    private void Start()
    {
        building.OnMouseOverBuilding += Building_OnMouseOverBuilding;
        building.OnMouseExitOverBuilding += Building_OnMouseExitOverBuilding;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        building.OnMouseOverBuilding -= Building_OnMouseOverBuilding;
        building.OnMouseExitOverBuilding -= Building_OnMouseExitOverBuilding;
    }

    private void Building_OnMouseExitOverBuilding(object sender, System.EventArgs e)
    {
        gameObject.SetActive(false);
    }

    private void Building_OnMouseOverBuilding(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
    }
}
