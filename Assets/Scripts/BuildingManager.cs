using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }
    public event EventHandler<OnActiveBuildingTypeChangedEventArgs> OnActiveBuildingTypeChanged;

    public class OnActiveBuildingTypeChangedEventArgs : EventArgs
    {
        public BuildingTypeSO activeBuildingType;
    }


    [SerializeField] private Building hqBuilding;

    private BuildingTypeSO activeBuildingType;
    private BuildingTypeListSO buildingTypeList;

    private void Awake()
    {
        Instance = this;

        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
        
    }


   


    private void Update()
    {
        

        if (!Input.GetMouseButtonDown(0))return; //return when the button is not pressed #NoNesting XD
        if (EventSystem.current.IsPointerOverGameObject()) return;//return when you are over a UI GameObject
        if (activeBuildingType == null) return;//When the activeBuildingType is null you dont want to place one
        
        if (!CanSpawnBuilding(activeBuildingType, UtilsClass.GetMouseWorldPosition(), out string errorMessage)) //when you cant spawnBuilding return;
        {
            ToolTipUI.Instance.Show(errorMessage,2f);
            return;
        }
        if (!ResourceManager.Instance.CanAfford(activeBuildingType.constructionResourceCostArray)) //return when you cant afford
        {
            ToolTipUI.Instance.Show("Cannot afford " + activeBuildingType.GetConstructionResourceCostString(),2f);
            return;
        }

        ResourceManager.Instance.SpendResources(activeBuildingType.constructionResourceCostArray);
        Instantiate(activeBuildingType.prefab, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
            
    }

    public void SetActiveBuildingType(BuildingTypeSO buildingType)
    {
        activeBuildingType = buildingType;
        OnActiveBuildingTypeChanged?.Invoke(this, new OnActiveBuildingTypeChangedEventArgs { activeBuildingType = activeBuildingType});
        
    }

    public BuildingTypeSO GetActiveBuildingType()
    {
        return activeBuildingType;
    }

    private bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 position, out string errorMessage)
    {
        BoxCollider2D boxCollider2d =  buildingType.prefab.GetComponent<BoxCollider2D>();
        Collider2D[] collider2DArray =  Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2d.offset, boxCollider2d.size, 0);


        bool isAreaClear = collider2DArray.Length == 0;
        if (!isAreaClear)
        {
            errorMessage = "Area is not clear";
            return false;
        }



        collider2DArray =  Physics2D.OverlapCircleAll(position, buildingType.minConstructionRadius);
        foreach(Collider2D collider2D in collider2DArray)
        {
            //Colliders inside the construction radius
            BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();
            if(buildingTypeHolder != null)
            {
                //Has Building Type holder
                if(buildingTypeHolder.buildingType == buildingType)
                {
                    //There is already a building within the buildingConstructionRadius
                    errorMessage = "Too close to another building of the same type";
                    return false;
                }
            }
            else
            {
                //Has no Building Type holder
            }

        }

        //Check that it is not too far away
        float maxConstructionRadius = 25f;
        collider2DArray = Physics2D.OverlapCircleAll(position, maxConstructionRadius);
        foreach (Collider2D collider2D in collider2DArray)
        {
            //Colliders inside the construction radius
            BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder != null)
            {
                //It is a building
                errorMessage = "";
                return true;
            }
           

        }

        errorMessage = "Too far from any other building";
        return false;

    }


    public Building GetHQBuilding()
    {
        return hqBuilding;
    }


}
