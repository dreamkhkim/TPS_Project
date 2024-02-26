using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObjectPool : MonoBehaviour
{
    public static BulletObjectPool instance = null;


    public GameObject bulletPrefab; // 총알 프리팹
    public static int currentBullet;
    [SerializeField]
    public Queue<GameObject> poolObj;

    public int initSize = 30;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }



    // Start is called before the first frame update
    void Start()
    {
        
        poolObj = new Queue<GameObject>();
        AddPoolBullet(initSize);

    }

    public void AddPoolBullet(int size)
    {
        for(int i = 0; i < size; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            poolObj.Enqueue(bullet);
        }

    }

    public void PopPoolBullet(Vector3 pos, Quaternion rot)
    {
        GameObject deQueuebullet = poolObj.Dequeue();

        deQueuebullet.transform.position = pos;
        deQueuebullet.transform.rotation = rot;

        deQueuebullet.SetActive(true);

    }

    public void ReturnPoolBullet(GameObject returnObj)
    {
        returnObj.SetActive(false);
        poolObj.Enqueue(returnObj);
    }

    
}
