using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class ThirdPersonController : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField]
    private LayerMask aimLayerMask;

    private ThirdPersonController thi;

   


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Player player = GetComponent<Player>();
        if (Input.GetAxis("Aim") == 1)
        {

            Debug.Log("가상 카메라 환경 테스트  ");

            aimVirtualCamera.gameObject.SetActive(true);

            //Gamepad.current.SetMotorSpeeds(0.124f, 0.245f);
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
        }


        if (player.isReload == true)
            aimVirtualCamera.gameObject.SetActive(false);




    }
}
