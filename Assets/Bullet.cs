using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{


    public void CreateBullet()
    {
        //transform.Translate(Vector3.forward * 0.3f, Space.Self);
        Rigidbody rb = GetComponent<Rigidbody>();
        //rb.AddForce(Vector3.forward * 10f, ForceMode.Impulse);    
        rb.velocity = transform.forward * 20f;
    }


    private void OnTriggerEnter(Collider other)
    {
        other = (Collider)GetComponent<FoundTarget>();

        BulletObjectPool.instance.ReturnPoolBullet(gameObject);

        if(other != null)
            BulletObjectPool.instance.ReturnPoolBullet(gameObject);


    }

    // Update is called once per frame
    void Update()
    {
        CreateBullet();  
    }
}
