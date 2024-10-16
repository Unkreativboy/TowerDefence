using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{


    private List<ResourceGeneratorData> resourceGeneratorData;
    private List<ResourceTypeSO> usedResourceTypesInThisGenerator;
    private Dictionary<ResourceTypeSO, ActionOnTimer> actionOnTimerDictonary;
    private Dictionary<ResourceTypeSO, float> timerMaxResourceDictonary;


    private void Awake()
    {
        usedResourceTypesInThisGenerator = new List<ResourceTypeSO>();
        actionOnTimerDictonary = new Dictionary<ResourceTypeSO, ActionOnTimer>();
        timerMaxResourceDictonary = new Dictionary<ResourceTypeSO, float>();
        resourceGeneratorData = GetComponent<BuildingTypeHolder>().buildingType.resourceGeneratorData;
        SetUpUsedResourceTypesInThisGenerator();
        SearchForNodesNearby();
        SetupTimers();
    }

    #region Gizmos
    //Draw Gizmos for Debugging
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        //Draw Gizmos for resourceDetectionRadius 
        for (int i = 0; i < resourceGeneratorData.Count; i++)
        {
            Gizmos.DrawWireSphere(transform.position, resourceGeneratorData[i].resourceDetectionRadius);
        }
    }
    #endregion
    private void SetUpUsedResourceTypesInThisGenerator()
    {
        
        foreach(ResourceGeneratorData data in resourceGeneratorData)
        {
            usedResourceTypesInThisGenerator.Add(data.resourceType);
        }
    }
    public static int GetNearbyResourceAmount(ResourceGeneratorData resourceGeneratorData, Vector3 position)
    {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(position, resourceGeneratorData.resourceDetectionRadius);
        int nearbyResourceAmount = 0;
        foreach (Collider2D collider2D in collider2DArray)
        {
            ResourceNode resourceNode = collider2D.GetComponent<ResourceNode>();
            if (resourceNode != null && resourceNode.resourceType.Equals(resourceGeneratorData.resourceType))
            {
                //it is a resource node
                nearbyResourceAmount++;
            }
        }

        nearbyResourceAmount = Mathf.Clamp(nearbyResourceAmount, 0, resourceGeneratorData.maxResourceAmount);

        return nearbyResourceAmount;
    }




    //TODO in einer eigene Klasse auslagern ? 
    private void SearchForNodesNearby()
    {
        //For every resourceData 
        for (int i = 0; i < resourceGeneratorData.Count; i++)
        {
            int nearbyResourceAmount = GetNearbyResourceAmount(resourceGeneratorData[i], transform.position);

            if (nearbyResourceAmount == 0)
            {
                //No Resources nearby
                //Disable Resource Generation by disableing this Script
                enabled = false;

            }
            else
            {
                SetUpTimerMax(resourceGeneratorData[i].resourceType, nearbyResourceAmount, resourceGeneratorData[i].timerMax, resourceGeneratorData[i].maxResourceAmount);
            }

        }
    }

    #region Timer Handling
    private void SetUpTimerMax(ResourceTypeSO resourceType, int nearbyResourceAmount, float timerMax, int maxResourceAmount)
    {
        float timerMaxCalculated = (timerMax / 2f) + timerMax * (1 - (float)nearbyResourceAmount / maxResourceAmount);


        timerMaxResourceDictonary.Add(resourceType, timerMaxCalculated);
    }
    private void SetupTimers()
    {
        //Create new Timer foreach Resource that is being created
        for (int i = 0; i < resourceGeneratorData.Count; i++)
        {

            if (timerMaxResourceDictonary.TryGetValue(resourceGeneratorData[i].resourceType, out var timerMax))
            {
                //There are nodes in that are nearby
                CreateNewTimer(timerMax, resourceGeneratorData[i].resourceType);

            }
            else
            {
                //there are no nodes nearby
            }
        }
    }
    private void CreateNewTimer(float timerMax, ResourceTypeSO resourceType)
    {
        ActionOnTimer timer = gameObject.AddComponent<ActionOnTimer>();
        timer.SetTimer(timerMax, () => { AddResource(resourceType); }, true);
        actionOnTimerDictonary.Add(resourceType, timer);

    }
    private void AddResource(ResourceTypeSO resourceType)
    {
        ResourceManager.Instance.AddResource(resourceType, 1);
    }
    #endregion

    #region Getter Methods
    //AsReadOnly verwenden oder lieber eine Kopie Erstellen, da so die Klassen die Getter Methode verwenden können um die Listen zu verändern
    public List<ResourceTypeSO> GetResourceTypesInThisGenerator() 
    {
        return usedResourceTypesInThisGenerator;
    }
    public List<ResourceGeneratorData> GetResourceGeneratorData() { return resourceGeneratorData; }
    public Dictionary<ResourceTypeSO, ActionOnTimer> GetTimerDictionary() { return actionOnTimerDictonary; }
    #endregion




}