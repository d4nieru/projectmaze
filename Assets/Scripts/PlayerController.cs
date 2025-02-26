using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;

    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = 9.81f;

    [Space]
    [SerializeField] private float sensitivityX = 2f;
    [SerializeField] private float sensitivityY = 2f;
    [SerializeField] private float defaultFOV = 60f;
    [SerializeField] private float sprintFOV = 75f;
    [SerializeField] private float fovSmooth = 8f;
    [SerializeField] private float cameraAngle = 80f;

    [Header("Head Bobbing")]
    [SerializeField] private float bobFrequency = 6f;
    [SerializeField] private float bobAmplitude = 0.05f;

    private float verticalVelocity;
    private float cameraRotation;
    private float speed;
    private float defaultYPos;
    private float headBobTimer = 0f;
    private bool isSprinting;
    private float targetFOV;

    private CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        defaultYPos = playerCamera.transform.localPosition.y;
        targetFOV = defaultFOV;
    }

    void Update()
    {
        if (Time.timeScale == 0f) return; // Empêche le mouvement en pause

        HandleMovement();
        HandleCamera();
        ApplyHeadBobbing();
        HandleFOV();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveVector = transform.forward * vertical + transform.right * horizontal;

        isSprinting = Input.GetKey(KeyCode.LeftShift);
        speed = isSprinting ? sprintSpeed : walkSpeed;

        if (controller.isGrounded)
        {
            verticalVelocity = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
            }
        }

        verticalVelocity -= gravity * Time.deltaTime;
        moveVector.y = verticalVelocity;
        controller.Move(moveVector * (speed * Time.deltaTime));
    }

    void HandleCamera()
    {
        float x = Input.GetAxisRaw("Mouse X") * sensitivityX;
        float y = Input.GetAxisRaw("Mouse Y") * sensitivityY;

        transform.rotation *= Quaternion.Euler(0, x, 0);
        cameraRotation -= y;
        cameraRotation = Mathf.Clamp(cameraRotation, -cameraAngle, cameraAngle);

        playerCamera.transform.localRotation = Quaternion.Euler(cameraRotation, 0, 0);
    }

    void ApplyHeadBobbing()
    {
        if (controller.velocity.magnitude > 0.1f && controller.isGrounded)
        {
            headBobTimer += Time.deltaTime * bobFrequency;
            float bobOffset = Mathf.Sin(headBobTimer) * bobAmplitude;

            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                defaultYPos + bobOffset,
                playerCamera.transform.localPosition.z
            );
        }
    }

    void HandleFOV()
    {
        targetFOV = isSprinting ? sprintFOV : defaultFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, fovSmooth * Time.deltaTime);
    }
}
