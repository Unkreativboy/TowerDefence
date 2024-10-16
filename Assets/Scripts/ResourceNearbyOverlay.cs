using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceNearbyOverlay : MonoBehaviour
{
    [SerializeField] private Transform prefabNearbyOverlay;
    [SerializeField] private Transform containerTransform;

    private List<ResourceGeneratorData> resourceGeneratorData;
    private Dictionary<ResourceGeneratorData, Transform> nearbyResourcePercentageUI;


    private void Awake()
    {
        nearbyResourcePercentageUI = new Dictionary<ResourceGeneratorData, Transform>();
        Hide();
    }

    private void Update()
    {
        SetNearbyResourcePercentageUI();
    }



    public void Show(List<ResourceGeneratorData> resourceGeneratorData)
    {
        DestroyAllResourcePercentageUI();
        this.resourceGeneratorData = resourceGeneratorData;
        gameObject.SetActive(true);
        CreateNearbyResourcePercentageUI();
    }

    private void DestroyAllResourcePercentageUI()
    {
        foreach(Transform transform in nearbyResourcePercentageUI.Values)
        {
            Destroy(transform.gameObject);
        }
        nearbyResourcePercentageUI.Clear();
    }

    private void CreateNearbyResourcePercentageUI()
    {
        foreach (ResourceGeneratorData resourceGeneratorData in resourceGeneratorData)
        {
            Transform transform = Instantiate(prefabNearbyOverlay, containerTransform.transform);
            nearbyResourcePercentageUI.Add(resourceGeneratorData,transform) ;
        }
    }
    private void SetNearbyResourcePercentageUI()
    {
        foreach(ResourceGeneratorData resourceGeneratorData in resourceGeneratorData)
        {
            //Set the Sprite to match the resource
            Transform resourceNearbyUI = nearbyResourcePercentageUI[resourceGeneratorData];
            resourceNearbyUI.GetComponentInChildren<Image>().sprite = resourceGeneratorData.resourceType.sprite;

            //Set the percentage number
            int nearbyResourceAmount = ResourceGenerator.GetNearbyResourceAmount(resourceGeneratorData, transform.position);
            float percent = Mathf.RoundToInt((float)nearbyResourceAmount / resourceGeneratorData.maxResourceAmount * 100f);
            resourceNearbyUI.GetComponentInChildren<TextMeshProUGUI>().SetText(percent.ToString()+"%");
        }
    }


    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
