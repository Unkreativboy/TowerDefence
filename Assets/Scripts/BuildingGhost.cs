using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{

    [SerializeField] private GameObject spriteGameObject;
    [SerializeField] private ResourceNearbyOverlay resourceNearbyOverlay;


    private BuildingTypeSO currentBuildingTypeSO;
    

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
       if(currentBuildingTypeSO == null) return;
        //Draw Gizmos for resourceDetectionRadius 
        for (int i = 0; i < currentBuildingTypeSO.resourceGeneratorData.Count; i++)
        {
            Gizmos.DrawWireSphere(transform.position, currentBuildingTypeSO.resourceGeneratorData[i].resourceDetectionRadius);
            
        }
    }



    private void Awake()
    {
        Hide();
    }

    private void Start()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;
    }

    private void OnDestroy()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChanged -= BuildingManager_OnActiveBuildingTypeChanged;

    }
    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, BuildingManager.OnActiveBuildingTypeChangedEventArgs e)
    {
        if(e.activeBuildingType == null)
        {
            Hide();
            currentBuildingTypeSO = null;
            resourceNearbyOverlay.Hide();
        }
        else
        {
            Show(e.activeBuildingType.sprite);
            currentBuildingTypeSO = e.activeBuildingType;
            resourceNearbyOverlay.Show(e.activeBuildingType.resourceGeneratorData);
        }

        
    }




    private void Update()
    {
        transform.position = UtilsClass.GetMouseWorldPosition();
    }

    private void Show(Sprite ghostSprite)
    {
        spriteGameObject.SetActive(true);
        spriteGameObject.GetComponent<SpriteRenderer>().sprite = ghostSprite;
    }
    private void Hide()
    {
        spriteGameObject.SetActive(false);
    }

}
