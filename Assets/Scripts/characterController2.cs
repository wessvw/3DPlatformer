using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float cameramoveSpeed = 150f;
    [SerializeField] float cameraDistance = 5f;
    private float yaw = 0f;
    private float pitch = 20f; // slight downward look by default
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float gravity = -9.81f;
    private Vector2 moveInput;
    private CharacterController controller;
    private Vector3 velocity;
    private Vector2 vectorinput;
    private Camera playerCamera;
    private bool isGrounded;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        // Give each player a random color for clarity
        GetComponent<Renderer>().material.color = new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f)
        );
        Transform firstChild = this.transform.GetChild(0);
        playerCamera = firstChild.GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Input System calls this automatically per player
    public void OnMove(InputValue input)
    {
        moveInput = input.Get<Vector2>();
    }

    public void OnJump(InputValue input)
    {
        if (input.isPressed && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public void OnLook(InputValue input)
    {
        Debug.Log("i am looking");
        vectorinput = input.Get<Vector2>();
    }

    void Update()
    {
        // Ground check
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // --- Movement relative to camera ---
        Vector3 camForward = playerCamera.transform.forward;
        Vector3 camRight = playerCamera.transform.right;

        // Ignore vertical tilt of the camera
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        // Build movement vector
        Vector3 move = (camForward * moveInput.y + camRight * moveInput.x).normalized;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // --- Camera orbit movement ---
        yaw += vectorinput.x * cameramoveSpeed * Time.deltaTime;
        pitch -= vectorinput.y * cameramoveSpeed * Time.deltaTime;

        // Clamp vertical pitch so camera doesn’t flip
        pitch = Mathf.Clamp(pitch, 0f, 70f);

        // Calculate camera rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Set camera position behind player
        Vector3 offset = rotation * new Vector3(0f, 0f, -cameraDistance);
        playerCamera.transform.position = transform.position + offset;

        // Make camera look at player
        Vector3 pointToLookAt = new Vector3(0, 0, 0);
        playerCamera.transform.LookAt(transform.position + pointToLookAt * 1.5f); // aim at chest/head height
    }
}