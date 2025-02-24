using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Camera playerCamera;

    [SerializeField] float walkSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float gravity;

    [Space]
    [SerializeField] float cameraSensitivity;
    [SerializeField] float defaultFOV;
    [SerializeField] float sprintFOV;
    [SerializeField] float fovSmooth;
    [SerializeField] float cameraAngle = 80f;

    float speed;
    float verticalVelocity;
    float initFOV;
    float cameraRotation;

    bool isSprinting;

    CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        playerCamera.fieldOfView = defaultFOV;
        initFOV = playerCamera.fieldOfView;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Time.timeScale == 0f) return; // Empêche toute mise à jour si le jeu est en pause

        HandleMovement();
    }

    void HandleMovement()
    {
        // Movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveVector = transform.forward * vertical + transform.right * horizontal;

        isSprinting = Input.GetKey(KeyCode.LeftShift);

        if (isSprinting)
        {
            speed = sprintSpeed;

            if (playerCamera.fieldOfView != sprintFOV)
            {
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, sprintFOV, fovSmooth * Time.deltaTime);
            }
        }
        else
        {
            speed = walkSpeed;

            if (playerCamera.fieldOfView != initFOV)
            {
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, initFOV, fovSmooth * Time.deltaTime);
            }
        }

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

        controller.Move(moveVector * (speed / 100));

        // Camera Movement
        float x = Input.GetAxisRaw("Mouse X") * cameraSensitivity;
        float y = Input.GetAxisRaw("Mouse Y") * cameraSensitivity;

        transform.rotation *= Quaternion.Euler(0, x, 0);

        cameraRotation -= y;
        cameraRotation = Mathf.Clamp(cameraRotation, -cameraAngle, cameraAngle);

        playerCamera.transform.localRotation = Quaternion.Euler(cameraRotation, 0, 0);
    }
}
