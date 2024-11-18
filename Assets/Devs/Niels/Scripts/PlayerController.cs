using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    private float moveSpeed = 0;

    [SerializeField]
    private float maxSpeed;

    [SerializeField]
    private float rotationSpeed = 5f;

    [SerializeField]
    private float jumpForce;

    private Vector2 moveDirection = Vector2.zero;
    private Vector2 lastMoveDirection = Vector2.zero;
    private bool isGrounded = true;

    private PlayerControlls playerControls;
    private InputAction move;
    private InputAction jump;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerControls = new PlayerControlls();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        jump = playerControls.Player.Jump;
        jump.Enable();
        jump.performed += OnJump;
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }

    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
        LookAt();

        if (moveSpeed < 0)
        {
            moveSpeed = 0;
        }
    }

    private void FixedUpdate()
    {
        // Only update lastMoveDirection when moveDirection changes.
        if (moveDirection != Vector2.zero)
        {
            // If only one axis has changed to zero, keep the last non-zero value.
            float x = moveDirection.x != 0 ? moveDirection.x : lastMoveDirection.x;
            float y = moveDirection.y != 0 ? moveDirection.y : lastMoveDirection.y;

            lastMoveDirection = new Vector2(x, y).normalized;
            moveSpeed = maxSpeed;
        }
        else if (moveSpeed > 0)
        {
            // Gradually decrease speed if no input is detected
            moveSpeed -= Time.deltaTime * 20;
        }

        // Maintain the vertical velocity to avoid interference with jumping
        float yVelocity = rb.velocity.y;

        // Apply the calculated movement direction and speed
        rb.velocity = new Vector3(
            lastMoveDirection.x * moveSpeed,
            yVelocity,
            lastMoveDirection.y * moveSpeed
        );
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }

    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            rb.rotation = Quaternion.Slerp(
                rb.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }
    }
}
