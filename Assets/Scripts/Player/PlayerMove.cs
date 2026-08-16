using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private Player player;

    [SerializeField] private Joystick joystick;
    [SerializeField] float moveSpeed = 5f;

    private CharacterController controller;

    private float Gravity = -9.81f;
    [SerializeField] private float groundDistance = .3f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private Transform ground;
    [SerializeField] private LayerMask groundMask;

    [Header("Movement Boundaries")]
    [SerializeField] private float minX = -20f;
    [SerializeField] private float maxX = 20f;
    [SerializeField] private float minZ = -20f;
    [SerializeField] private float maxZ = 20f;

    [Header("Climb Settings")]
    [SerializeField] private float climbSpeed = 3f;
    [SerializeField] private float wallCheckDistance = 0.6f;
    [SerializeField] private LayerMask wallMask;

    Vector3 velocity;

    public bool isGround;
    public bool isPressed;
    public bool isClimbing;

    void Start()
    {
        player = GetComponent<Player>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if(CameraTour.isTouring) return;

        isGround = Physics.CheckSphere(ground.position, groundDistance, groundMask);

        MoveAndClimb();
        ClampPosition();

        if (isGround && !isClimbing) player.animController.RunAnim();
    }

    private void MoveAndClimb()
    {
        float horizontalInput = joystick.Horizontal;
        float verticalInput = joystick.Vertical;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) verticalInput += 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) verticalInput -= 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) horizontalInput -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) horizontalInput += 1f;
        }

        horizontalInput = Mathf.Clamp(horizontalInput, -1f, 1f);
        verticalInput = Mathf.Clamp(verticalInput, -1f, 1f);

        bool isTouchingWall = Physics.Raycast(transform.position + Vector3.up * 1f, transform.forward, wallCheckDistance, wallMask);

        if (isTouchingWall && (verticalInput > 0 || isPressed))
        {
            isClimbing = true;
        }
        else if (!isTouchingWall)
        {
            isClimbing = false;
        }

        Vector3 moveDirection = Vector3.zero;

        if (isClimbing)
        {
            float climbInput = verticalInput;
            if (isPressed) climbInput = 1f;

            velocity.y = climbInput * climbSpeed;

            moveDirection = Vector3.zero;

            player.animController.SetClimb(true, climbInput);
        }
        else
        {
            if (isGround && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            if (isPressed && isGround)
            {
                player.animController.Jump();
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * Gravity);
                isGround = false;
            }

            velocity.y += Gravity * Time.deltaTime;
            moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;

            player.animController.SetClimb(false);
        }

        Vector3 finalVelocity = moveDirection * moveSpeed + Vector3.up * velocity.y;
        controller.Move(finalVelocity * Time.deltaTime);

        float currentSpeed = (horizontalInput != 0f || verticalInput != 0f) ? 1f : 0f;
        player.animController.Speed(currentSpeed);
    }

    private void ClampPosition()
    {
        Vector3 pos = transform.position;
        float clampedX = Mathf.Clamp(pos.x, minX, maxX);
        float clampedZ = Mathf.Clamp(pos.z, minZ, maxZ);

        if (pos.x != clampedX || pos.z != clampedZ)
        {
            controller.enabled = false;
            transform.position = new Vector3(clampedX, pos.y, clampedZ);
            controller.enabled = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        float yPos = transform.position.y;
        Vector3 topLeft = new Vector3(minX, yPos, maxZ);
        Vector3 topRight = new Vector3(maxX, yPos, maxZ);
        Vector3 bottomLeft = new Vector3(minX, yPos, minZ);
        Vector3 bottomRight = new Vector3(maxX, yPos, minZ);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position + Vector3.up * 1f, transform.forward * wallCheckDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(ground.position, groundDistance);
    }
}