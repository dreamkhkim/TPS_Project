using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{


    public void CreateBullet()
    {
        

      
        //Destroy(gameObject, f);

    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * 20f;
        CreateBullet();
        
    }
}
