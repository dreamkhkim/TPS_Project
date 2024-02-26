using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SwitchVCam : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerinput;
    [SerializeField]
    private int priorityBoostAmount = 10;

    private CinemachineVirtualCamera virtualCamera;
    private InputAction aimAction;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        aimAction = playerinput.actions["Aim"];
    }

    private void OnEnable()
    {
        aimAction.performed += (obj) => StartAim();
        aimAction.canceled += (obj) => CancelAim();
    }

    private void OnDisable()
    {
        aimAction.performed -= (obj) => StartAim();
        aimAction.canceled -= (obj) => CancelAim();
        
    }

    private void StartAim()
    {
        virtualCamera.Priority += priorityBoostAmount;
    }

    private void CancelAim()
    {
        virtualCamera.Priority -= priorityBoostAmount;

    }
}
