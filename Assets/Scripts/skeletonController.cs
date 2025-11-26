using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class SkeletonController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float cameramoveSpeed = 150f;
    [SerializeField] float cameraDistance = 5f;
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] Animator animationController;
    [SerializeField] public GameObject ballGameObject;
    [SerializeField] GameObject TposeSkeleton;
    private activateCanvasses canvasses;
    private float yaw = 0f;
    private float pitch = 20f;
    private float coyoteTimeCounter;
    private Vector2 moveInput;
    private Vector2 vectorinput;
    private Vector3 velocity;
    private CharacterController controller;
    private Camera playerCamera;
    public FatManController Fatman;
    public LayerMask groundMask;
    public int collectAbleCount;
    public bool isBall = false;
    public bool ballIsThrown = false;
    private bool ballInAir;
    private bool isGrounded;
    public List<string> inventory;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        ballGameObject.SetActive(false);
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
        GameObject canvassesObject = GameObject.Find("canvasses");
        canvasses = canvassesObject.GetComponent<activateCanvasses>();
        Fatman = GameObject.Find("fatMan(Clone)").GetComponent<FatManController>();
        Fatman.skeleton = this;
    }


    void FixedUpdate()
    {
        // Debug.Log(isGrounded);
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
        if (!isBall)
        {
            controller.Move(move * moveSpeed * Time.deltaTime);
        }

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

        // Set camera position behind player and Make camera look at player
        Vector3 pointToLookAt = new Vector3(0, 2, 0);
        Vector3 offset = rotation * new Vector3(0f, 0f, -cameraDistance);
        if (isBall)
        {
            playerCamera.transform.position = ballGameObject.transform.position + offset;
            playerCamera.transform.LookAt(ballGameObject.transform.position + pointToLookAt);
        }
        else
        {
            playerCamera.transform.position = this.transform.position + offset;
            playerCamera.transform.LookAt(this.transform.position + pointToLookAt);
        }

        if (ballIsThrown)
        {
            Rigidbody ballRigidBody = ballGameObject.GetComponent<Rigidbody>();
            Vector3 direction = new Vector3(0, 0.4f, 0);
            ballRigidBody.useGravity = true;
            // Vector3 oppositeOffset = -Fatman.offset;
            Vector3 fatmanCamforward = new Vector3(Fatman.camForward.x, Fatman.camForward.y + direction.y, Fatman.camForward.z);
            ballRigidBody.AddForce(-Fatman.offset + fatmanCamforward * 10, ForceMode.Impulse);
            ballInAir = true;
            ballIsThrown = false;
            Fatman.isHoldingSkeleton = false;
        }
        // this.transform.position = ballGameObject.transform.position;
        if (ballInAir)
        {
            bool ballIsGrounded = Physics.CheckSphere(ballGameObject.transform.position - new Vector3(1f, 1f, 1f), 0.1f, groundMask);
            if (ballIsGrounded)
            {
                this.transform.position = ballGameObject.transform.position;
                isBall = false;
                transformIntoBall(isBall);
                ballInAir = false;
            }
        }

        // if (isBall && isGrounded)
        // {
        //     ballGameObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        // }

        // rotate the player based on camera position
        Quaternion targetRotation = Quaternion.LookRotation(camForward);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime * 10f);


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
        if (hit.gameObject.name == "fatMan(Clone)" && Fatman.isLaying)
        {
            velocity.y = -velocity.y;
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
        // if (collectAbleCount >= 2)
        // {
        if (isBall)
        {
            isBall = false;
            // jumpHeight = 2.1f;
            transformIntoBall(isBall);
        }
        else
        {
            isBall = true;
            // jumpHeight = 0f;
            transformIntoBall(isBall);
        }
        // }
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

    public void OnRestart(InputValue input)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void transformIntoBall(bool istheBall)
    {
        TposeSkeleton.SetActive(!istheBall);
        ballGameObject.SetActive(istheBall);
        ballGameObject.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1f, this.transform.position.z);
        ballGameObject.GetComponent<Rigidbody>().useGravity = true;
        ballGameObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        if (isBall)
        {
            controller.height = 0;
        }
        else
        {
            controller.height = 2f;
        }
    }


}