
// using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float cameramoveSpeed = 250f;
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float gravity = -9.81f;
    private Vector2 moveInput;
    private CharacterController controller;
    private Vector3 velocity;
    private Vector2 vectorinput;
    private Camera playerCamera;
    private bool isGrounded;
    private float lookx;
    private float looky;

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
        vectorinput = input.Get<Vector2>();
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Ground check
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Movement
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        controller.Move(move * moveSpeed * Time.deltaTime);
        this.transform.localRotation = new Quaternion(0f, playerCamera.transform.rotation.x, 0f, 0f);
        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Camera movement
        Vector3 cameraMove = new Vector3(vectorinput.x, 0, vectorinput.y);
        playerCamera.transform.RotateAround(this.transform.position, Vector3.up, -cameraMove.x * cameramoveSpeed * Time.deltaTime);
        Vector3 cameraright = playerCamera.transform.right;
        playerCamera.transform.RotateAround(this.transform.position, cameraright, cameraMove.y * cameramoveSpeed * Time.deltaTime);

    }
}