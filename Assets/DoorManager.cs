using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Door door = GetComponent<Door>();
        if (other.GetComponent<PlayerTarget>() != null)
        {
            Debug.Log("플레이어 발견 ");
            door.GetIsTrue = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        Door door = GetComponent<Door>();
        Debug.Log("문 닫힘 ");
         door.GetIsTrue = false;
    }

}
