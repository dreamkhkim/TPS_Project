using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem.XInput;





public class Player : Character, PlayerTarget
{
    [SerializeField]
    private CinemachineVirtualCamera aimVirtualCamera;


    //Hp_Bar 
    [SerializeField]
    private TMP_Text current;
    public Image hpImage;


    public GameObject enemyInfo;

    [SerializeField]
    private float healthTimer = 0f;
    private float hpRangeRate = 2;

    [SerializeField]
    private CapsuleCollider coverHeight;


    private CharacterController controller;
    private PlayerInput playerInput;

    public static Player instance = null;
    

   
    public int initSize = 10;

    [SerializeField]
    private int currentBullet; // 총에 들어갈 탄약 수
    public  int carryBullet = 120; // 가지고 있는 총알 수

    public int bulletCount = 0;
    public int maxBullet = 30;
    public bool isReload = false;
    public bool coverisReload = false;




    private float fireRate = 4f;
    private float fireNextTime = 0f;


    //총기 사운드 
    public AudioClip fireSFX;
    public AudioSource source = null;

    public override int Hp { get =>
            base.Hp;

        set
        {
            base.Hp = value;
        }

    }



    [SerializeField]
    private Vector3 playerVelocity; 
    [SerializeField]
    private bool groundPlayer; // 플레이어가 땅에 닿았는지

    [SerializeField]
    private float playerSpeed = 1.0f;
    [SerializeField]
    private float lowPlayerSpeed = 1.0f;

    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f; //표준중력가속도 (9.80665 m/s^2)
    [SerializeField]
    private float rotationSpeed = 0.8f;

    [SerializeField]
    private Transform cameraTransform;
    
    
    
    private InputAction moveAction;
    private InputAction jumpAction;

    private Animator anim;

    public GameObject crossHair;


    public GameObject firePosition; // 총알 위치 정보
    public GameObject bulletPrefab;


    public GameObject reloadObject;

    public GameObject reloadObject2;
    public float bulletRange = 1000f;

    private Gamepad gamepad;


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
        
        
        controller  = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        coverHeight = GetComponent<CapsuleCollider>();

        source = GetComponent<AudioSource>();
       
        cameraTransform = Camera.main.transform;

        anim = GetComponent<Animator>();


        
        

        reloadObject.SetActive(false);
        //reloadObject2.SetActive(true);

        //moveAction = playerInput.actions["Move"];

        moveAction = playerInput.actions["MoveMent"];

        jumpAction = playerInput.actions["Jump"];

    }

  
    

    // Update is called once per frame
    void Update()
    {

        hpImage.fillAmount = this.Hp * 0.01f;


        if(this.Hp < 100)
        {
            healthTimer += Time.deltaTime;

            if(healthTimer >= 5)
            {
                healthTimer = 0f;

                this.Hp += 1;

                Hp = Mathf.Clamp(this.Hp, 0, 101);
            }

        }


        current.text = currentBullet.ToString() + " / " + carryBullet.ToString();
        groundPlayer = controller.isGrounded;

        if (groundPlayer && playerVelocity.y < 0)
            playerVelocity.y = 0;

        Vector2 input = moveAction.ReadValue<Vector2>();
        
        Vector3 move = new Vector3(input.x, 0, input.y);

        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        

        if (input == Vector2.zero)
        {
            //Gamepad.current.SetMotorSpeeds(0.24f, 0.75f);
            //gamepad.SetMotorSpeeds(0.1f, 0.2f);
            anim.SetFloat("YInput", 0);
            anim.SetFloat("XInput", 0);
        }

        



        //XBOX 패드

        //엄폐
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            Debug.Log("a버튼 ");
            anim.SetBool("Cover", true);

            controller.height = 1.2f;
            //yield return new WaitForSeconds(3f);


            coverisReload = true;

        }

        
  
        if (Input.GetAxis("Vertical") > 0)
        {
            anim.SetFloat("YInput", 1);
            controller.height = 2f;
            if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") > 0)
            {
                anim.SetFloat("YInput", 1);
                anim.SetFloat("XInput", 1);
            }

            else if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") < 0)
            {
                anim.SetFloat("YInput", 1);
                anim.SetFloat("XInput", -1);
            }

        }

        else if (Input.GetAxis("Vertical") < 0)
        {
            anim.SetFloat("YInput", -1);
            
            coverisReload = false;

            if (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") > 0)
            {
                anim.SetFloat("YInput", -1);
                anim.SetFloat("XInput", 1);
            }

            else if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") < 0)
            {
                anim.SetFloat("YInput", -1);
                anim.SetFloat("XInput", -1);
            }

        }



        else if(Input.GetAxis("Horizontal") < 0)
            anim.SetFloat("XInput", -1);

        else if (Input.GetAxis("Horizontal") > 0)
            anim.SetFloat("XInput", 1);


        
        //Debug.Log(Input.GetAxis("Aim"));
        if (isReload == false && Input.GetAxis("Aim") == 1)
        {
            Debug.Log("눌림 ");

            controller.height = 2f;
            //gamepad.SetMotorSpeeds(0.25f, 0.75f); ;
            crossHair.gameObject.SetActive(true);
            //enemyInfo.SetActive(true);
            if(isReload == true)
                crossHair.SetActive(false);
            anim.SetBool("aim", true);


            anim.SetBool("Fire", false);

            bool isFireable = Input.GetAxisRaw("AimFire") == 1;
            isFireable &= Time.time >= fireNextTime;
            isFireable &= currentBullet > 0;
            isFireable &= isReload == false;

            if (isFireable)
            {
                //Gamepad gamepad = GetComponent<XboxGamepadMacOS>();
                //XInputController.current.SetMotorSpeeds(0.4f, 0.9f);
                //gamepad.SetMotorSpeeds(0.04f, 0.09f);


                //Gamepad.current.SetMotorSpeeds(0.5f, 0.5f);
                //gamepad.SetMotorSpeeds(0.5f, 0.9f);
                anim.SetBool("Fire", true);
                source.PlayOneShot(fireSFX, 0.9f);

                currentBullet--;

                bulletCount++;


                if (currentBullet <= 0 && carryBullet > 0)
                {
                    currentBullet = 0;
                    //Invoke("Shoot", 5f);
                    reloadObject.SetActive(true);
                    isReload = true;
                    StartCoroutine(ReloadBullet());
                }
                //if (GetCurretBullet <= 29)
                //{
                //    anim.SetTrigger("Reload");
                //    //Invoke("Shoot", 5f);
                //    reloadObject.SetActive(true);
                //    isReload = true;
                //    StartCoroutine(ReloadBullet());
                //}
                else if (carryBullet <= 0)
                {
                    carryBullet = 0;
                    isReload = false;
                }
                else
                {
                    reloadObject.SetActive(false);
                    isReload = false;
                }

                fireNextTime = Time.time + 1f / fireRate;
                Shoot();

            }
            else
            {
                
                anim.SetBool("Fire", false);

            }
            
                
            
        }
        else
        {
            controller.height = 1.74f;
            anim.SetBool("aim", false);
            crossHair.SetActive(false);
            //enemyInfo.SetActive(false);
        }

        if (carryBullet > 0 && currentBullet != 30 &&  Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            //Invoke("Shoot", 2f);
            reloadObject.SetActive(true);
            isReload = true;
            StartCoroutine(ReloadBullet());
            //reloadObject2.SetActive(true);
        }
        else
        {
            reloadObject.SetActive(false);
            //isReload = false;
        }
        float speed = 1;
        if (Input.GetAxis("Vertical") > 0 && Input.GetKey(KeyCode.JoystickButton0))
        {

            anim.SetFloat("YInput", 2);
            anim.SetBool("Cover", false);
            controller.height = 2f;
            coverisReload = false;

            if(Input.GetKey(KeyCode.Joystick1Button16))
            {
                speed = 2;
            }
            if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") > 0 && Input.GetKey(KeyCode.Joystick1Button16))
            {
                speed = 2;
                anim.SetFloat("YInput", 2);
                anim.SetFloat("XInput", 2);
            }
            else if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") < 0 && Input.GetKey(KeyCode.Joystick1Button16))
            {
                anim.SetFloat("YInput", 2);
                anim.SetFloat("XInput", -2);
            }
            else
            {
                anim.SetFloat("YInput", 2);
                anim.SetFloat("XInput", 0);
            }
            playerSpeed *= 4;

            if (playerSpeed > 4)
                playerSpeed = 4;


        }
        else
        {
            playerSpeed = lowPlayerSpeed;
        }



        
        anim.SetFloat("Yinput", input.y * speed);
        anim.SetFloat("Xinput", input.x * speed);





        if (Input.GetKey(KeyCode.W))
        {
            anim.SetFloat("YInput", 1);

            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
            {
                anim.SetFloat("YInput", 1);
                anim.SetFloat("XInput", 1);
            }

            else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            {
                anim.SetFloat("YInput", 1);
                anim.SetFloat("XInput", -1);
            }
        }

        else if (Input.GetKey(KeyCode.S))
        { 
            anim.SetFloat("YInput", -1);

            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
            {
                anim.SetFloat("YInput", -1);
                anim.SetFloat("XInput", 1);
            }

            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
            {
                anim.SetFloat("YInput", -1);
                anim.SetFloat("XInput", -1);
            }
        }

        else if (Input.GetKey(KeyCode.A))
            anim.SetFloat("XInput", -1);
        else if (Input.GetKey(KeyCode.D))
            anim.SetFloat("XInput", 1);

       

        if (Input.GetKeyDown(KeyCode.R))
        {
            anim.SetTrigger("Reload");
            Invoke("Shoot", 2f);
            reloadObject.SetActive(true);
            //reloadObject2.SetActive(true);
        }
        else
        {
            reloadObject.SetActive(false);
        }




        //적 UI정p
        RaycastHit ray;

        if (Physics.Raycast(firePosition.transform.position, firePosition.transform.forward, out ray, 1000, 1 << LayerMask.NameToLayer("Enemy")))
        {
            ray.collider.GetComponent<MidEnemy>().EnemyFound(0);
            this.enemyInfo.SetActive(true);
        }    
        else
            this.enemyInfo.SetActive(false);



        controller.Move(move * Time.deltaTime * playerSpeed);


        if (jumpAction.triggered && groundPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -5.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);



        LookAround();

        //Cover();
       
        

    }

    void LookAround()
    {
        //회전할 카메라 방향
        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);

        Vector3 gunDirection = cameraTransform.forward;

        transform.rotation = targetRotation;
    }

    void KeyBoard()
    {
        //if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        //{

        //    anim.SetFloat("YInput", 2);

        //    if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift))
        //    {
        //        anim.SetFloat("YInput", 2);
        //        anim.SetFloat("XInput", 2);
        //    }
        //    else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.LeftShift))
        //    {
        //        anim.SetFloat("YInput", 2);
        //        anim.SetFloat("XInput", -2);
        //    }
        //    else
        //    {
        //        anim.SetFloat("YInput", 2);
        //        anim.SetFloat("XInput", 0);
        //    }
        //    playerSpeed *= 2;

        //    if (playerSpeed > 2)
        //        playerSpeed = 2;


        //}

        //else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.LeftShift))
        //{

        //    anim.SetFloat("YInput", -2);

        //    if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift))
        //    {
        //        anim.SetFloat("YInput", -2);
        //        anim.SetFloat("XInput", 2);
        //    }
        //    else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.LeftShift))
        //    {
        //        anim.SetFloat("YInput", -2);
        //        anim.SetFloat("XInput", -2);
        //    }
        //    playerSpeed *= 2;

        //    if (playerSpeed > 10)
        //        playerSpeed = 10;


        //}
        //else
        //{
        //    playerSpeed = lowPlayerSpeed;
        //}

    }


    IEnumerator ReloadBullet()
    {

        if (coverisReload == true)
            anim.SetTrigger("CoverReload");
        else
            anim.SetTrigger("Reload");

        while (isReload)
        {
            crossHair.SetActive(false);
            yield return new WaitForSeconds(2f);

            if (currentBullet <= 0)
            {
                carryBullet -= maxBullet;
                currentBullet = maxBullet;

                bulletCount = 0;

                yield return null;

            }
            else if(currentBullet > 0)
            {
                currentBullet = bulletCount;
                carryBullet -= currentBullet;
                currentBullet = maxBullet;
                bulletCount = 0;
                yield return null;
            }
            yield return new WaitForSeconds(2f);
            isReload = false;
        }
    }



    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(firePosition.transform.position, Camera.main.transform.forward, out hit, bulletRange, 1 << LayerMask.NameToLayer("Enemy")))
        {
            
            BulletObjectPool.instance.PopPoolBullet(firePosition.transform.position, transform.rotation);
            if (hit.transform.GetComponent<FoundTarget>() != null)
            {
                hit.transform.GetComponent<FoundTarget>().EnemyFound(this.GetDamage);
                Debug.Log("적 체력 깎임 ");
            }
            else
                reloadObject2.SetActive(true);
        }
    }


    public void PlayerFound(int value)
    {
        Hp -= value;
    }


}
