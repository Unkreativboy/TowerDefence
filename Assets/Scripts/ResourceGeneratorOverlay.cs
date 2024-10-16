using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceGeneratorOverlay : MonoBehaviour
{
    [SerializeField] private ResourceGenerator resourceGenerator;
    [SerializeField] private Transform prefabResourceUI;
    [SerializeField] private GameObject containerTransform;
    private List<ResourceGeneratorData> resourceGeneratorDataList;
    private List<ResourceTypeSO> usedResourceTypesInThisGenerator;

    private Dictionary<ResourceTypeSO, ActionOnTimer> actionOnTimerDictionary;
    private Dictionary<ResourceTypeSO, Transform> resourceUIDictonary;
    

    private void Awake()
    {
        resourceUIDictonary = new Dictionary<ResourceTypeSO, Transform>();
    }


    private void Start()
    {
        usedResourceTypesInThisGenerator = resourceGenerator.GetResourceTypesInThisGenerator();
        resourceGeneratorDataList = resourceGenerator.GetResourceGeneratorData();

        actionOnTimerDictionary = resourceGenerator.GetTimerDictionary();

        CreateResourceUI();
        SetUpResourcePerSecondText();
    }
    private void CreateResourceUI()
    {
        foreach (ResourceGeneratorData resourceGeneratorData in resourceGeneratorDataList)
        {
            ResourceTypeSO resourceType = resourceGeneratorData.resourceType;
            Transform transform = Instantiate(prefabResourceUI, containerTransform.transform);
            transform.Find("image").GetComponent<Image>().sprite = resourceType.sprite;
            resourceUIDictonary.Add(resourceType, transform);
        }
    }
    private void SetUpResourcePerSecondText()
    {

        foreach (ResourceTypeSO resourceTypeSO in usedResourceTypesInThisGenerator)
        {
            TextMeshProUGUI textMeshPro = resourceUIDictonary[resourceTypeSO].GetComponentInChildren<TextMeshProUGUI>();
            float resourcePerSecond;
            if (actionOnTimerDictionary.TryGetValue(resourceTypeSO, out ActionOnTimer actionOnTimer))
            {
                resourcePerSecond = actionOnTimer.GetCalledActionsPerSeconds();
            }
            else
            {
                resourcePerSecond = 0f;
            }

            
            textMeshPro.SetText(resourcePerSecond.ToString("F1"));
        }
    }

    private void UpdateResourceUI()
    {
        foreach (ResourceTypeSO resourceTypeSO in usedResourceTypesInThisGenerator)
        {
            Slider slider = resourceUIDictonary[resourceTypeSO].GetComponentInChildren<Slider>();
            if(slider != null)
            {
                if (actionOnTimerDictionary.TryGetValue(resourceTypeSO, out ActionOnTimer timer))
                {

                    slider.value = timer.GetCurrentTimeNormalized();
                    


                }
                else
                {
                    slider.value = 0f;
                }
            }
            else
            {
                Debug.LogError("There is not Slider Object on the resource UI");
            } 
        }
    }

    private void Update()
    {
       UpdateResourceUI();



       
        
        
        
    }





}
