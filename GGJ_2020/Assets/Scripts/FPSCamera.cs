using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FPSCamera : MonoBehaviour
{
    [SerializeField]
    private bool invertY;
    public Camera cam;

    public float mainSpeed = 5.0f; //regular speed\
    [SerializeField, Range(1f, 3f)]
    private float runSpeedMultiplier = 1.75f;
    
    public float jumpForce = 2.0f; // How Powerful is our Jump
    public LayerMask mask;

    bool isGrounded;
    bool hideMouse = true;
    Vector3 newPosition, newRotation;

    private Vector3
        lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)

    private float totalRun = 1.0f;


    private Rigidbody rb;
    private float mouseX;
    private float mouseY;

    private int x, y;
    private Vector3 rotateValue;

    public float rotationSpeed = 20f;

    [FormerlySerializedAs("testHeight")] public float groundCheckDistance = 1.05f;

    private new Transform transform;

    // Start is called before the first frame update
    void Start()
    {
        //jump = new Vector3(0.0f, 2.0f, 0.0f);
        rb = gameObject.GetComponent<Rigidbody>();

        transform = gameObject.transform;
        
        //set the characters position
        newPosition = transform.position;
        //set the rotation
        newRotation = transform.eulerAngles;
    }

    private void Update()
    {

        //function to move based on which WASD key is pressed
        GetBaseInput();
        mouseY = Input.GetAxis("Mouse Y");
        mouseX = Input.GetAxis("Mouse X");

        //   Debug.Log($"({mouseX}, {mouseY})");

        //set the initial rotation
        rotateValue = new Vector3(mouseX, mouseY * -1, 0);



        //set the characters position
        newPosition = transform.position;
        //set the rotation
        newRotation = transform.eulerAngles;

        //If left shift held, use run instead of walk speed
        if (Input.GetKey(KeyCode.LeftShift))
        {
            newPosition += transform.forward * (y * mainSpeed * runSpeedMultiplier * Time.deltaTime);
            newPosition += transform.right * (x * mainSpeed* runSpeedMultiplier * Time.deltaTime);
        }
        else
        {
            newPosition += transform.forward * (y * mainSpeed * Time.deltaTime);
            newPosition += transform.right * (x * mainSpeed * Time.deltaTime);
        }
        //apply changes in position and rotation
        

        newRotation.y += rotateValue.x * rotationSpeed * Time.deltaTime;


        //jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("jump)");
            Jump();

            // rb.AddForce(jump * jumpForce, ForceMode.Impulse);

        }

        //hide cursor

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            hideMouse = !hideMouse;
            Cursor.lockState = hideMouse ? CursorLockMode.Locked : CursorLockMode.None;

        }
    }

    private void LateUpdate()
    {
        var camRotation = cam.transform.rotation.eulerAngles;

        camRotation.x += invertY? -1f : 1f * rotateValue.y * rotationSpeed * 0.5f * Time.deltaTime;

        camRotation.x = Utilities.ClampAngle(camRotation.x, -50, 50);

        cam.transform.rotation = Quaternion.Euler(camRotation);
    }


    // Update is called once per frame
    private void FixedUpdate()
    {
        //rotate the chacter
        rb.rotation = Quaternion.Euler(newRotation);
        //move the character
        rb.position = newPosition;

    }

    private void GetBaseInput()
    {
        //set input vectors
        if (Input.GetKeyDown(KeyCode.W))
        {
            y = 1;
        }

        else if (Input.GetKeyUp(KeyCode.W))
        {
            y = 0;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            y = -1;
        }

        else if (Input.GetKeyUp(KeyCode.S))
        {
            y = 0;
        }


        if (Input.GetKeyDown(KeyCode.A))
        {
            x = -1;
        }

        else if (Input.GetKeyUp(KeyCode.A))

        {
            x = 0;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            x = 1;
        }

        else if (Input.GetKeyUp(KeyCode.D))
        {
            x = 0;
        }

    }

    private void Jump()
    {

        if (Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, mask.value))
        {
            Debug.Log("Did Hit");
            isGrounded = true;
            Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, Color.green, 2f);
        }
        else
        {

            isGrounded = false;
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
            Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, Color.red, 2f);
        }

        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

    }
}