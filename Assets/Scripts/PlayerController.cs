using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb; 
    private float movementX;
    private float movementY;
    [SerializeField]
    private PhysicMaterial bouncyMaterial;

    [SerializeField]
    private PhysicMaterial slipperyMaterial;
    [SerializeField]
    private PhysicMaterial bumpyMaterial;

    public float speed = 0;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI warningText;
    public GameObject winTextObject;
    public static int count;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        SetWarningText();
        winTextObject.SetActive(false);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetWarningText()
    {
        warningText.text = "Beware! You could get sucked into a singularity if you venture too far!";
    }
    void SetCountText()
    {
        countText.text = "Score: " + count.ToString();
        if (count >= 12)
        {
            winTextObject.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement*speed);
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
