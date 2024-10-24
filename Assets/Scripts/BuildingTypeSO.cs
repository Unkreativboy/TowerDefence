using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]
public class BuildingTypeSO : ScriptableObject
{
    public string nameString;
    public Transform prefab;
    public List<ResourceGeneratorData> resourceGeneratorData;
    public Sprite sprite;
    public float minConstructionRadius;
    public ResourceAmount[] constructionResourceCostArray;
    public int healthAmountMax;
    public float constructionTime;

    public string GetConstructionResourceCostString()
    {
        string str = "";
        foreach(ResourceAmount resourceAmount in constructionResourceCostArray)
        {
            str += resourceAmount.resourceType.nameString + ":" + resourceAmount.amount;
        }
        return str;
    }
}
