using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTypeSelectUI : MonoBehaviour
{
    [SerializeField] private Transform btnTemplate;
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private List<BuildingTypeSO> ignoreBuildingTypeList;
    private Dictionary<BuildingTypeSO, Transform> btnTransformDictonary;


    private Transform arrowBtn;

    private void Awake()
    {
        btnTemplate.gameObject.SetActive(false);
        BuildingTypeListSO buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);

        btnTransformDictonary = new Dictionary<BuildingTypeSO, Transform>();
        int index = 0;


        CreateArrowButton();
        index++; // Has to be increased because of the created ArrowButton

        
        foreach(BuildingTypeSO buildingType in buildingTypeList.list)
        {
            if(ignoreBuildingTypeList.Contains(buildingType)) continue;

            Transform btnTransform = Instantiate(btnTemplate, transform);
            btnTransform.gameObject.SetActive(true);

            float offsetAmount = +130f;
            btnTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);


            btnTransform.Find("image").GetComponent<Image>().sprite = buildingType.sprite;

            btnTransform.GetComponent<Button>().onClick.AddListener(() => 
            { 
                BuildingManager.Instance.SetActiveBuildingType(buildingType); 
            });

            MouseEnterExitEvents mouseEnterExitEvents = btnTransform.GetComponent<MouseEnterExitEvents>();
            mouseEnterExitEvents.OnMouseEnter += (object sender, EventArgs e) => 
            {
                ToolTipUI.Instance.Show(buildingType.nameString + "\n"+ buildingType.GetConstructionResourceCostString());
            };
            mouseEnterExitEvents.OnMouseExit += (object sender, EventArgs e) =>
            {
                ToolTipUI.Instance.Hide();
            };


            btnTransformDictonary[buildingType] = btnTransform;
            index++;
        }


    }
    private void CreateArrowButton()
    {
        arrowBtn = Instantiate(btnTemplate, transform);
        arrowBtn.gameObject.SetActive(true);

        float offsetAmount = 0;
        arrowBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount, 0);

        arrowBtn.Find("image").GetComponent<Image>().sprite = arrowSprite;
        arrowBtn.Find("image").GetComponent<RectTransform>().sizeDelta = new Vector2(0,-30);

        MouseEnterExitEvents mouseEnterExitEvents = arrowBtn.GetComponent<MouseEnterExitEvents>();
        mouseEnterExitEvents.OnMouseEnter += (object sender, EventArgs e) =>
        {
            ToolTipUI.Instance.Show("Arrrow");
        };
        mouseEnterExitEvents.OnMouseExit += (object sender, EventArgs e) =>
        {
            ToolTipUI.Instance.Hide();
        };

        arrowBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            BuildingManager.Instance.SetActiveBuildingType(null);
        });
    }
    


    private void Start()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;
        UpdateActiveBuildingTypeButton();
    }

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, BuildingManager.OnActiveBuildingTypeChangedEventArgs e)
    {
        UpdateActiveBuildingTypeButton();
    }

    private void OnDestroy()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChanged -= BuildingManager_OnActiveBuildingTypeChanged;
    }
    private void UpdateActiveBuildingTypeButton()
    {

        arrowBtn.Find("selected").gameObject.SetActive(false);
        foreach(BuildingTypeSO buildingType in btnTransformDictonary.Keys)
        {
            Transform btnTransform = btnTransformDictonary[buildingType];
            btnTransform.Find("selected").gameObject.SetActive(false);

        }

        BuildingTypeSO activeBuildingType =  BuildingManager.Instance.GetActiveBuildingType();  
        if(activeBuildingType == null)
        {
            arrowBtn.Find("selected").gameObject.SetActive(true);
        }
        else
        {
            btnTransformDictonary[activeBuildingType].Find("selected").gameObject.SetActive(true);
        }


    }



}
