using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePositionSortingOrder : MonoBehaviour
{

    [SerializeField] private bool runOnce;
    [SerializeField] private float positionOffsetY;

    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {

        float percisionMultiplier = 5f;

        spriteRenderer.sortingOrder = (int)(-(transform.position.y + positionOffsetY)*percisionMultiplier);
        if (runOnce)
        {
            Destroy(this);
        }

    }


}
