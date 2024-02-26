using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.JoystickButton0))
        {
            
            Debug.Log("ㅋㅣ 누");
            Gamepad.current.SetMotorSpeeds(0.1f, 0.5f);
        }
        
    }
}
