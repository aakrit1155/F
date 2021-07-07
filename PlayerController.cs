using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;          //can only have on value

    public float moveSpeed, gravityModifier, jumpPower, runSpeed = 12f;
    public CharacterController charCon;
    private Vector3 moveInput;

    public Transform camTrans;

    public float mouseSensitivity;
    public bool invertX;
    public bool invertY;

    private bool canJump, canDoubleJump;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;

    public Animator anim;

    //public GameObject bullet;
    public Transform firePoint;

    public Gun activeGun;
    public List<Gun> allGuns = new List<Gun>();
    public List<Gun> unlockableGuns = new List<Gun>();
    public int currentGun;

    private void Awake()              //happens right before start
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentGun--;
        SwitchGun();
    }

    // Update is called once per frame
    void Update()
    {
        if (!UIController.instance.pauseScreen.activeInHierarchy && !GameManager.instance.levelEnding)
        {
            //moveInput.x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;  //goes into unity input system gets axis by default a and d  - 1 and 1  Delta time is how long each frame takes  (1/FPS)
            //moveInput.z = Input.GetAxis("Vertical")   * moveSpeed * Time.deltaTime;                   // samne but w and s   

            //store y velocity 
            float yStore = moveInput.y;

            Vector3 vertMove = transform.forward * Input.GetAxis("Vertical");
            Vector3 horiMove = transform.right * Input.GetAxis("Horizontal");

            moveInput = horiMove + vertMove;
            moveInput.Normalize();            //controls the speed when two direction ket are pressed

            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveInput *= runSpeed;
            }
            else
            {
                moveInput *= moveSpeed;
            }

            moveInput.y = yStore;

            moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;  //applies graity at a cosntant rate instead of accelrating

            if (charCon.isGrounded)
            {
                moveInput.y = Physics.gravity.y * gravityModifier * Time.deltaTime;
            }


            canJump = Physics.OverlapSphere(groundCheckPoint.position, .25f, whatIsGround).Length > 0;            //creates a sphere and checks whether an object is there or not




            //Handle Jumping
            if (Input.GetKeyDown(KeyCode.Space) && canJump)                                    //down means the moment being pressed gte key is for the while it is being pressed and up is when the user lets go
            {
                moveInput.y = jumpPower;

                canDoubleJump = true;
                AudioManager.instance.PlaySFX(5);

            }
            else if (canDoubleJump && Input.GetKeyDown(KeyCode.Space))
            {
                moveInput.y = jumpPower;

                canDoubleJump = false;
                AudioManager.instance.PlaySFX(5);
            }

            charCon.Move(moveInput * Time.deltaTime);


            //camera rotation 
            Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;       //get axis has smoothing applued to ii and raw gives the exact value
                                                                                                                                 //mouse moves in 2 d x and y axis of unity. horizantal rotation is Y and vertical rotation is X
            if (invertX)
            {
                mouseInput.x = -mouseInput.x;
            }
            if (invertY)
            {
                mouseInput.y = -mouseInput.y;
            }

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);

            camTrans.rotation = Quaternion.Euler(camTrans.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f)); //rotation of body horizantally but only camera vertically

            //Handle Shooting
            //single shots
            if (Input.GetMouseButtonDown(0) && activeGun.fireCounter <= 0)
            {
                //create an imaginary lne to tell if an object is being hit
                RaycastHit hit;
                if (Physics.Raycast(camTrans.position, camTrans.forward, out hit, 50f))  //out hit says it an obbject is hit 50f denotes the line length
                {
                    if (Vector3.Distance(camTrans.position, hit.point) > 2f)
                    {
                        firePoint.LookAt(hit.point);

                    }
                }
                else
                {
                    firePoint.LookAt(camTrans.position + (camTrans.forward * 30f));      //for places with no objects 
                }
                //Instantiate(bullet, firePoint.position, firePoint.rotation);
                FireShot();
            }

            //repeat shots
            if (Input.GetMouseButton(0) && activeGun.canAutoFire)
            {
                if (activeGun.fireCounter <= 0)
                {
                    FireShot();
                }
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SwitchGun();
            }

            if (Input.GetMouseButtonDown(1))
            {
                CameraController.instance.ZoomIn(activeGun.zoomAmount);
            }
            if (Input.GetMouseButtonUp(1))
            {
                CameraController.instance.ZoomOut();
            }

            anim.SetFloat("moveSpeed", moveInput.magnitude);
            anim.SetBool("onGround", canJump);
        }
    }

    public void FireShot()
    {
        if (activeGun.currentAmmo > 0)
        {
            activeGun.currentAmmo--;
            Instantiate(activeGun.bullet, firePoint.position, firePoint.rotation);
            activeGun.fireCounter = activeGun.fireRate;
            UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;
        }
    }

    public void SwitchGun()
    {
        activeGun.gameObject.SetActive(false);
        currentGun++;
        if(currentGun >= allGuns.Count)
        {
            currentGun = 0;
        }
      

        activeGun = allGuns[currentGun];
        activeGun.gameObject.SetActive(true);

        UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;

        firePoint.position = activeGun.firepoint.position;
    }

    public void AddGun(string gunToAdd)
    {
        bool gunUnlcoked = false;
        
        if(unlockableGuns.Count > 0)
        {
            for(int i = 0; i < unlockableGuns.Count; i++)
            {
                if(unlockableGuns[i].gunName == gunToAdd)
                {
                    gunUnlcoked = true;
                    allGuns.Add(unlockableGuns[i]);
                    unlockableGuns.RemoveAt(i);
                    i = unlockableGuns.Count;
                }
            }
        }

        if(gunUnlcoked)
        {
            currentGun = allGuns.Count - 2;
            SwitchGun();
        }
    }

}
