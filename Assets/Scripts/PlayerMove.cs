using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    private PlayerControls playerControls;

    [Header("Movement")]
    private Vector2 movement;
    private Rigidbody rb;
    private Vector3 direction;
    public float speed = 3f;
    public float sprintSpeed = 5f;
    private bool sprinting;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask ground;
    private bool grounded;
    public float groundDrag;

    [Header("Camera Settings")]
    public float sensitivityX;
    public float sensitivityY;

    private float xRotation;
    private float yRotation;

    private Vector2 mouseRotation;

    public GameObject camera;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public bool canJump = true;

    [Header("Flashlight")]
    public Flashlight flashlight;

    // Start is called before the first frame update
    void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerControls.Movement.Walk.performed += context => movement = context.ReadValue<Vector2>();
        playerControls.Movement.Walk.canceled += context => movement = Vector2.zero;

        playerControls.Movement.Look.performed += context => mouseRotation = context.ReadValue<Vector2>();
        playerControls.Movement.Look.canceled += context => mouseRotation = Vector2.zero;

        playerControls.Movement.Sprint.performed += context => sprinting = true;
        playerControls.Movement.Sprint.canceled += context => sprinting = false;

        playerControls.Movement.Flash.performed += context => flashlight.flashOn += 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!sprinting)
        {

            Move(speed);
        }
        else
        {
            Move(sprintSpeed);
        }

        if (grounded)
        {
            rb.AddForce(direction.normalized * speed * 10f, ForceMode.Force);
        }
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ground);

        yRotation += mouseRotation.x;
        xRotation -= mouseRotation.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        camera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);

        if (playerControls.Movement.Jump.triggered && canJump && grounded)
        {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        UseFlashlight();
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void Move(float type)
    {
        Vector3 playerDirection = (movement.y * transform.forward) + (movement.x * transform.right);
        transform.position += playerDirection * type * Time.deltaTime;
    }

    private void UseFlashlight()
    {
        if (flashlight.flashOn == 0)
        {
            flashlight.flashlight.SetActive(false);
            flashlight.flashing = false;
        }
        else if (flashlight.flashOn == 1)
        {
            flashlight.flashlight.SetActive(true);
            flashlight.flashing = true;
        }

        if (flashlight.flashOn >= 2)
        {
            flashlight.flashOn = 0;
        }
    }

    private void ResetJump()
    {
        canJump = true;
    }
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
