using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;


    [Header("Settings")]
    [SerializeField] private float cameraMovementSpeed = 5f;
    [SerializeField] private float zoomAmount = 2f;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float minOrthographicSize = 10;
    [SerializeField] private float maxOrthographicSize = 30;

    private float orthographicSize;
    private float targetOrthograficSize;


    private void Start()
    {
        orthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        targetOrthograficSize = orthographicSize;
    }


    private void Update()
    {
        HandleMovement();
        HandleZoom();

        
    }

    private void HandleMovement()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        Vector2 moveDir = new Vector2(inputX, inputY).normalized;

        transform.position += (Vector3)moveDir * cameraMovementSpeed * Time.deltaTime;
    }

    private void HandleZoom()
    {
        targetOrthograficSize += -Input.mouseScrollDelta.y * zoomAmount;
        targetOrthograficSize = Mathf.Clamp(targetOrthograficSize, minOrthographicSize, maxOrthographicSize);
        orthographicSize = Mathf.Lerp(orthographicSize, targetOrthograficSize, Time.deltaTime * zoomSpeed);
        cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographicSize;
    }

}
