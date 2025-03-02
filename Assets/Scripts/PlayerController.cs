using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    [Header("Camera Settings")]
    [SerializeField] private float sensitivityX = 2f;
    [SerializeField] private float sensitivityY = 2f;
    [SerializeField] private float defaultFOV = 60f;
    [SerializeField] private float sprintFOV = 75f;
    [SerializeField] private float fovSmooth = 8f;
    [SerializeField] private float cameraAngle = 80f;

    private float verticalVelocity;
    private float cameraRotation;
    private float speed;
    private bool isSprinting;
    private float targetFOV;

    private CharacterController controller;
    private AudioManager audioManager;
    private AudioClip lastClipPlayed; // Pour éviter de rejouer le même son

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {
        if (Time.timeScale == 0f) return; // Empêche le mouvement en pause

        HandleMovement();
        HandleCamera();
        HandleFOV();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveVector = transform.forward * vertical + transform.right * horizontal;
        isSprinting = Input.GetKey(sprintKey);
        speed = isSprinting ? sprintSpeed : walkSpeed;

        if (controller.isGrounded)
        {
            verticalVelocity = 0;
            if (Input.GetKeyDown(jumpKey))
            {
                verticalVelocity = jumpForce;
            }

            HandleFootsteps(moveVector.magnitude);
        }

        verticalVelocity -= gravity * Time.deltaTime;
        moveVector.y = verticalVelocity;
        controller.Move(moveVector * (speed * Time.deltaTime));
    }

    void HandleFootsteps(float movementMagnitude)
    {
        if (movementMagnitude > 0.1f)
        {
            AudioClip clipToPlay = isSprinting ? audioManager.runningOnMarble : audioManager.walkingOnMarble;
            
            if (clipToPlay != lastClipPlayed) // Évite de rejouer le même son inutilement
            {
                audioManager.PlayLoopingSFX(clipToPlay);
                lastClipPlayed = clipToPlay;
            }
        }
        else if (lastClipPlayed != null)
        {
            audioManager.StopSFX();
            lastClipPlayed = null;
        }
    }

    void HandleCamera()
    {
        float x = Input.GetAxisRaw("Mouse X") * sensitivityX;
        float y = Input.GetAxisRaw("Mouse Y") * sensitivityY;

        transform.rotation *= Quaternion.Euler(0, x, 0);
        cameraRotation = Mathf.Clamp(cameraRotation - y, -cameraAngle, cameraAngle);
        playerCamera.transform.localRotation = Quaternion.Euler(cameraRotation, 0, 0);
    }

    void HandleFOV()
    {
        targetFOV = isSprinting ? sprintFOV : defaultFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, fovSmooth * Time.deltaTime);
    }
}
