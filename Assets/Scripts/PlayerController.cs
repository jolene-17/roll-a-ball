using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public Transform orientation;
    float horizontalInput;
    float verticalInput;
    public float groundDrag;
    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump = true;
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;


    [Header("Rest of the stuff")]

    private Rigidbody rb; 

    [Header("Physics Materials")]
    [SerializeField] private PhysicMaterial bouncyMaterial;

    [SerializeField]
    private PhysicMaterial slipperyMaterial;
    [SerializeField]
    private PhysicMaterial bumpyMaterial;

    [Header("TextGUIs")]
    public TextMeshProUGUI countText;
    public TextMeshProUGUI warningText;
    public GameObject winTextObject;
    public static int count;

    private float jumpdebugtimer = 0f;
    private float jumpdebugtimertime = 2f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        SetWarningText();
        winTextObject.SetActive(false);

        rb.freezeRotation = true; //cam
        readyToJump = true; //jumping
    }

    private void Update()
    {
        if(jumpdebugtimer <= jumpdebugtimertime)
        {
            jumpdebugtimer += Time.deltaTime;
            readyToJump = true; 
        }
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Debug.Log("Should be jumping");
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    Vector3 moveDirection;
    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }

    void SetWarningText()
    {
        warningText.text = "Beware! You could get sucked into a singularity if you venture too far!";
    }
    void SetCountText()
    {
        countText.text = "Score: " + count.ToString();
        if (count >= 3)
        {
            winTextObject.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }  

        if (other.gameObject.CompareTag("BouncyPower"))
        {
            GetComponent<Collider>().material = bouncyMaterial;
            Debug.Log("Bouncy-ed");
        }

        if (other.gameObject.CompareTag("SlipperyPower"))
        {
            GetComponent<Collider>().material = slipperyMaterial;
            Debug.Log("Slippery!");
        }
            
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            winTextObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
        }
    }

}
