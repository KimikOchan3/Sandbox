using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public GameObject Camera;
    public GameObject Legs;
    public UnityEngine.CharacterController Controller;
    public Vector3 move;
    public float velocity;
    private float Gravity = 9.81f;
    public float MoveSpeed = 1f;
    public float CameraSensitivity = 2f;
    public float cameraSmoothing = 5f;
    public float JumpPower = 1f;
    public float bunnyHop = 1f;
    public float SprintSpeed = 1.5f;
    public bool pushing = false;
    // private float StairsHeight = 0.5f; Пока что бесполезняк
    public float MouseX = 0f;
    public float MouseY = 0f;
    private float MinMouseY = -90f;
    private float MaxMouseY = 90f;
    public bool isGrounded;
    public float PlayerX;
    public float PlayerY;
    public float groundDistance = 1.1f;
    private float speed;
    private Quaternion cameraLogic;
    private Quaternion playerLogic;

    void Start()
    {
        Debug.Log("Player Controller started!");

        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MouseX += Input.GetAxis("Mouse X") * CameraSensitivity;
        MouseY += Input.GetAxis("Mouse Y") * CameraSensitivity;

        if(MouseX > 359.9f)
            MouseX = 0;
        if(MouseX < 0)
            MouseX = 359.9f;

        MouseY = Mathf.Clamp(MouseY, MinMouseY, MaxMouseY);

        Quaternion playerTarget = Quaternion.Euler(0, MouseX, 0);
        playerLogic = Quaternion.Slerp(transform.rotation, playerTarget, Time.deltaTime * cameraSmoothing);
        
        Quaternion cameraTarget = Quaternion.Euler(-MouseY, MouseX, 0);
        cameraLogic = Quaternion.Slerp(Camera.transform.rotation, cameraTarget, Time.deltaTime * cameraSmoothing);
        transform.rotation = playerLogic;
        Camera.transform.rotation = cameraLogic;

        PlayerX = Input.GetAxis("Horizontal") * bunnyHop * MoveSpeed * Time.deltaTime;
        PlayerY = Input.GetAxis("Vertical") * bunnyHop * MoveSpeed * Time.deltaTime;
        
        move.y -= Gravity * Time.deltaTime;
        if(isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
            if(bunnyHop<2)
                bunnyHop += 0.2f;
            PlayerJump();
            }
        
        PlayerMove();
        // GroundDetection();
        isGrounded = Controller.isGrounded;


        // if(!Controller.isGrounded)
        // {
        //     move.y += -Gravity * Time.deltaTime;       
        // }
    }

    void FixedUpdate()
    {
        if(isGrounded)
        {
            if(move.y<0)
                move.y = 0f;
            if(bunnyHop>1)
                bunnyHop -= 0.05f;            
        }

        bunnyHop = Mathf.Clamp(bunnyHop, 1, 2);
    }
    void PlayerJump()
    {
        // rb.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
        // velocity += Mathf.Sqrt(JumpPower * 3f * Gravity);
        move.y += Mathf.Sqrt(JumpPower * 3f * Gravity);
        
    }
    void PlayerMove()
    {
        // velocity += -Gravity * Time.deltaTime;
        move.y += -Gravity * Time.deltaTime;
        move = transform.up * move.y + transform.right * PlayerX + transform.forward * PlayerY;
        if(Input.GetKey(KeyCode.LeftShift))
        {
            // rb.velocity = transform.TransformDirection(new Vector3(PlayerX * SprintSpeed, rb.velocity.y, PlayerY * SprintSpeed));
            Controller.Move(move * SprintSpeed*  Time.deltaTime);
        }

        else
        {
            // rb.velocity = transform.TransformDirection(new Vector3(PlayerX, rb.velocity.y, PlayerY));
            Controller.Move(move * Time.deltaTime);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit obj)
    {
        //Не лезь, оно тебе не надо!
        if(obj.rigidbody && pushing)
        {
            Rigidbody hit = obj.collider.attachedRigidbody;
            // hit.AddForceAtPosition(transform.forward, transform.position * 0.2f, ForceMode.Impulse);
            Debug.Log("PUSH!");
            hit.AddForce(transform.forward * 0.5f, ForceMode.Impulse);
        }
        
    }
    // void GroundDetection()
    // {
    //     if(Physics.Raycast(transform.position, Vector3.down, groundDistance))
    //     {
    //         isGrounded = true;

    //     }
    //     else
    //     {
    //         isGrounded = false;

    //     }
    // }
}
