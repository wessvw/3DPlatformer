using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;

[RequireComponent(typeof(CharacterController))]
public class FatManController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float cameramoveSpeed = 150f;
    [SerializeField] float cameraDistance = 5f;
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] Animator animationController;
    [SerializeField] Collider layingHitBox;
    private Transform fatManModel;
    private activateCanvasses canvasses;
    private float yaw = 0f;
    private float pitch = 20f;
    private float coyoteTimeCounter;
    private Vector2 moveInput;
    private Vector2 vectorinput;
    private Vector3 velocity;
    private CharacterController controller;
    private Camera playerCamera;
    public SkeletonController skeleton;
    public LayerMask groundMask;
    public int collectAbleCount;
    private bool isGrounded;
    public bool isLaying = false;
    Quaternion targetRotation = new Quaternion();
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        Transform secondChild = this.transform.GetChild(1);
        fatManModel = secondChild;
    }

    private void Start()
    {
        // Give each player a random color for clarity
        Transform firstChild = this.transform.GetChild(0);
        playerCamera = firstChild.GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameObject canvassesObject = GameObject.Find("canvasses");
        canvasses = canvassesObject.GetComponent<activateCanvasses>();
    }


    void FixedUpdate()
    {
        // bool isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, controller.height / 2, 0), 0.4f, groundMask);
        Debug.Log(isGrounded);
        // Ground check
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        if (isGrounded)
        {
            coyoteTimeCounter = 0.1f;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        animationController.SetBool("isGrounded", isGrounded);
        animationController.SetBool("isLaying", isLaying);
        animationController.SetFloat("speed", Mathf.Abs(moveInput.y + moveInput.x));


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

        // rotate the player based on camera position
        if (!isLaying)
        {
            targetRotation = Quaternion.LookRotation(camForward);
        }

        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime * 10f);
        // controller.


        // Make camera look at player
        Vector3 pointToLookAt = new Vector3(0, 0, 0);
        playerCamera.transform.LookAt(this.transform.position + pointToLookAt); // aim at chest/head height

        Vector3 raycastDirection = new Vector3(0, -1, 0);
        if (Physics.Raycast(transform.position, raycastDirection, out RaycastHit hit, controller.height / 2 + 0.2f, groundMask))
        {
            Vector3 platformMove = (camForward * moveInput.y + camRight * moveInput.x).normalized;
            if (hit.transform.GetComponent<VelocityCalculator>())
            {
                Debug.Log(hit.transform.name);
                platformMove += hit.transform.GetComponent<VelocityCalculator>().GetVelocity();
                controller.Move(platformMove * Time.deltaTime);
            }
        }

    }

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
        else if (coyoteTimeCounter > 0f)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            coyoteTimeCounter = 0f;
        }
    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.name == "fatMan(Clone)" && isLaying)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 50 * -2f * gravity);
        }

        if (hit.gameObject.tag == "movingObject")
        {
            // Debug.Log("movingObject is hit");
        }
        // Debug.Log(hit.gameObject.name);
    }

    public void OnLook(InputValue input)
    {
        vectorinput = input.Get<Vector2>();
    }

    public void OnAbility1(InputValue input)
    {

        if (isLaying == false)
        {
            isLaying = true;
            moveSpeed = 0f;
            jumpHeight = 0f;
            controller.height = 0;
            fatManModel.Rotate(new Vector3(90f, 0f, 0f));
            // fatManModel.transform.localPosition = (new Vector3(0f, -0.2f, 0f));
            controller.center = new Vector3(0, 0.6f, 0.4f);
            layingHitBox.enabled = true;
            targetRotation = Quaternion.Euler(-90f, yaw, 0f);
        }
        else
        {
            isLaying = false;
            velocity.y += 10;
            moveSpeed = 5f;
            jumpHeight = 2f;
            layingHitBox.enabled = false;
            controller.center = new Vector3(0, 0.6f, 0);
            controller.height = 3;
            // fatManModel.transform.localPosition = (new Vector3(0f, 0, 0f));
            fatManModel.Rotate(new Vector3(-90f, 0f, 0f));
        }
    }

    public void OnAbility2(InputValue input)
    {
        Debug.Log(collectAbleCount);
    }

    public void OnAbility3(InputValue input)
    {
        Debug.Log("ab3");
    }

    public void OnAbility4(InputValue input)
    {
        Debug.Log("ab4");
    }

    public void updateCountInCanvas()
    {

        canvasses.updateText(collectAbleCount, this.name);
    }


}