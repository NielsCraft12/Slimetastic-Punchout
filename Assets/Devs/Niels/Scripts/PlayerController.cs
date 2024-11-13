using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float jumpForce;

    private Vector2 moveDirection = Vector2.zero;
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
    }

    private void FixedUpdate()
    {
        float yVelocity = rb.velocity.y;
        rb.velocity = new Vector3(
            moveDirection.x * moveSpeed,
            yVelocity,
            moveDirection.y * moveSpeed
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
}
