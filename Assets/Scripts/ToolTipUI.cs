using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToolTipUI : MonoBehaviour
{
    public static ToolTipUI Instance {  get; private set; }



    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private RectTransform backgroundRectTransform;
    [SerializeField] private RectTransform canvasRectTransform;


    private RectTransform rectTransform;
    private ActionOnTimer timer;


    private void Awake()
    {
        Instance = this;
        rectTransform = GetComponent<RectTransform>();
        Hide();
        timer = gameObject.AddComponent<ActionOnTimer>();
    }
    private void Update()
    {
       HandleFollowMouse();

    }
    private void HandleFollowMouse()
    {
        Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;

        if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width)
        {
            anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
        }
        if (anchoredPosition.y + backgroundRectTransform.rect.height > canvasRectTransform.rect.height)
        {
            anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;
        }


        rectTransform.anchoredPosition = anchoredPosition;
    }


    private void SetText(string tooltipText)
    {
        textMeshPro.SetText(tooltipText);
        textMeshPro.ForceMeshUpdate();

        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 padding = new Vector2(8, 8);

        backgroundRectTransform.sizeDelta = textSize+padding;
    }

    public void Show(string tooltipTex, float showTime = -1)
    {
        gameObject.SetActive(true);
        SetText(tooltipTex);
        if(showTime > 0)
        {
            timer.SetTimer(showTime, () => { Hide(); });
        }
        HandleFollowMouse();

    }
    

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    

}
