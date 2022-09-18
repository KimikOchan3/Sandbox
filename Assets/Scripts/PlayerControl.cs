using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public GameObject Camera;
    public Rigidbody rb;
    // public Text sprint;
    // public Text ground;
    public float MoveSpeed = 100f;
    public float CameraSensivity = 2f;
    public float JumpPower = 150f;
    public float SprintSpeed = 1.5f;
    // private float StairsHeight = 0.5f; Пока что бесполезняк
    private float MouseX = 0f;
    private float MouseY = 0f;
    private float MinMouseY = -90f;
    private float MaxMouseY = 90f;
    private bool isGrounded;
    private float PlayerX;
    private float PlayerY;
    public float groundDistance = 1.1f;

    void Start()
    {
        Debug.Log("Player Controller started!");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MouseX += Input.GetAxis("Mouse X") * CameraSensivity;
        MouseY += Input.GetAxis("Mouse Y") * CameraSensivity;

        if(MouseX > 180 || MouseX < -180)
            MouseX = -MouseX;
        MouseX = Mathf.Clamp(MouseX, -180, 180);
        MouseY = Mathf.Clamp(MouseY, MinMouseY, MaxMouseY);

        transform.localEulerAngles = new Vector3(0, MouseX, 0);
        Camera.transform.localEulerAngles = new Vector3(-MouseY, 0, 0);

        PlayerX = Input.GetAxis("Horizontal") * MoveSpeed * Time.fixedDeltaTime;
        PlayerY = Input.GetAxis("Vertical") * MoveSpeed * Time.fixedDeltaTime;

        if(isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            // Debug.Log("Jump!");
            PlayerJump();
        }
    }

    void FixedUpdate()
    {
        PlayerMove();
        GroundDetection();
    }
    void PlayerJump()
    {
        rb.AddForce(new Vector3 (0, JumpPower, 0));
    }
    void PlayerMove()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            // sprint.enabled = true;
            rb.velocity = transform.TransformDirection(new Vector3(PlayerX * SprintSpeed, rb.velocity.y, PlayerY * SprintSpeed));
        }

        else
        {
            // sprint.enabled = false;
            rb.velocity = transform.TransformDirection(new Vector3(PlayerX, rb.velocity.y, PlayerY));
        }
    }

    void GroundDetection()
    {
        if(Physics.Raycast(transform.position, Vector3.down, groundDistance))
        {
            // ground.enabled = true;
            isGrounded = true;
        }
        else
        {
            // ground.enabled = false;
            isGrounded = false;
        }
    }
}
