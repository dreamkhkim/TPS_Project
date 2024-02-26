using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MidEnemy : Character, FoundTarget
{

    public GameObject player;
    public Animator anim;
    public Transform bulletPos;
    public GameObject enemyBullet;

    public GameObject info;


    public Image enemyhp;
    public TMP_Text health;

    public Vector3 dir;

    public CapsuleCollider capsule;
    public int size;

    public float timeCheck;
    public float enemyTimeCheck;
    public float intervalCheck;
    public float bulletIntervalCheck;

    private void Start()
    {
        player = FindObjectOfType<Player>().gameObject;
        capsule = GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();
        
        
    }

    // 적 1 ~ 2레벨 
    public void EnemyFound(int value)
    {
        
        Debug.Log(gameObject.name + "해당 클릭은 적입니다. ");
        //info.SetActive(true);
        enemyhp.fillAmount = this.Hp * 0.1f;
        this.Hp -= value;

        if (this.Hp <= 0)
        {
            //info.SetActive(false);
            this.capsule.height = 1;
            //this.player = null;
            anim.SetBool("EnemyDie", true);
        }
    }


    void CreateBullet()
    {
        //this.capsule.height = 3;
    }
   

    private void Update()
    {   
        enemyTimeCheck += Time.deltaTime;
        
        RaycastHit ray;
        Debug.DrawRay(bulletPos.transform.position, bulletPos.transform.forward * 10f, Color.red);

        if (enemyTimeCheck >= bulletIntervalCheck && Physics.Raycast(bulletPos.transform.position, bulletPos.transform.forward * 100f, out ray, 1000f, 1 << LayerMask.NameToLayer("Player")))
        {
            
            GameObject bullet = Instantiate(enemyBullet, bulletPos.transform.position, transform.rotation);
            this.capsule.height = 3f;
            CreateBullet();
            
            bulletIntervalCheck = Random.Range(1, 6);

            if (ray.transform.GetComponent<PlayerTarget>() != null)
            {
                
                ray.transform.GetComponent<PlayerTarget>().PlayerFound(this.GetDamage);
                Debug.Log(ray.transform.name);
                Debug.Log("플레이어 발견 발사 ");

            }
           
            enemyTimeCheck = 0;
        }

        if (enemyTimeCheck <= 0)
        {
            this.capsule.height = 1;
            anim.SetTrigger("Cover");
        }

        if (enemyTimeCheck >= 1)
            this.capsule.height = 3;
       
        if (Hp <= 0)
        {
            this.capsule.height = 1;
            anim.SetBool("EnemyDie", true);
        }


        if (Player.instance != null)
        {
            dir = (player.transform.position - this.transform.position).normalized;
            dir.y = 0f;   
            transform.rotation = Quaternion.LookRotation(dir);
        }

    }

}
