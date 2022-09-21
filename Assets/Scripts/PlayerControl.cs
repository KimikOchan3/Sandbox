using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public GameObject Camera;
    public CharacterController Controller;
    private Vector3 movement;

    public float moveSpeed = 2f;
    public float SprintSpeed = 1.5f;
    public float jumpPower = 1f;
    public float bunnyHop = 1f;
    public float bunnyHopLimit = 5f;
    [Tooltip("Сколько добавляется при каждом прыжке")]
    public float bunnyHopJump = 0.5f;
    [Tooltip("Сколько отнимается при остановке распрыжки")]
    public float bunnyHopStop = 0.05f;
    public float cameraSensitivity = 2f;
    public float cameraSmoothing = 15f;
    public float minMouseX = -80f;
    public float maxMouseX = 80f;
    public bool pushing = false;    //Неготовая система толкания
    private float playerX;
    private float playerZ;
    private float mouseX = 0f;
    private float mouseY = 0f;
    private bool isGrounded;
    private float Gravity = 9.81f;

    void Start()
    {
        Debug.Log("Player Controller started!");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //Кординаты камеры
        mouseX += Input.GetAxis("Mouse Y") * cameraSensitivity;
        mouseY += Input.GetAxis("Mouse X") * cameraSensitivity;

        //Смещение камеры
        // Camera.transform.TransformDirection(Vector3.up * Mathf.Clamp(movement.y, -0.2f, 0.2f));

        //Ограничения поворота камеры
        if(mouseY > 359.9f)
            mouseY = 0;
        if(mouseY < 0)
            mouseY = 359.9f;

        mouseX = Mathf.Clamp(mouseX, minMouseX, maxMouseX);

        //Плавный поворот камеры
        Quaternion playerTarget = Quaternion.Euler(0, mouseY, 0);
        Quaternion playerLogic = Quaternion.Slerp(transform.rotation, playerTarget, Time.deltaTime * cameraSmoothing);
        
        Quaternion cameraTarget = Quaternion.Euler(-mouseX, mouseY, 0);
        Quaternion cameraLogic = Quaternion.Slerp(Camera.transform.rotation, cameraTarget, Time.deltaTime * cameraSmoothing);
        transform.rotation = playerLogic;
        Camera.transform.rotation = cameraLogic;

        //Вектор ходьбы
        playerX = Input.GetAxis("Horizontal") * bunnyHop * moveSpeed;
        playerZ = Input.GetAxis("Vertical") * bunnyHop * moveSpeed;
        movement = transform.up * movement.y + transform.right * playerX  + transform.forward * playerZ;
        Debug.Log(movement);

        //Гравитация
        movement.y -= Gravity * Time.deltaTime;
        
        //Проверка нахождения на земле
        if(isGrounded && movement.y < 0)
        {
            movement.y = -0.1f;
        }

        isGrounded = Controller.isGrounded;

        //Системы
        PlayerMove();

        if(isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            if(bunnyHop < bunnyHopLimit)
                bunnyHop += bunnyHopJump;
            
            PlayerJump();
        }
    }

    void FixedUpdate()
    {
        BunnyHop();
    }

//Система прыжка
    void PlayerJump()
    {
        movement.y += Mathf.Sqrt(jumpPower * Gravity);
    }

//Система перемещения
    void PlayerMove()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            movement.x *= SprintSpeed;
            movement.z *= SprintSpeed;
            Controller.Move(movement * Time.deltaTime);
        }
        else
        {
            Controller.Move(movement * Time.deltaTime);
        }
    }

//Система распрыжки
    void BunnyHop()
    {
        if(isGrounded && bunnyHop > 1)
            bunnyHop -= bunnyHopStop;

        bunnyHop = Mathf.Clamp(bunnyHop, 1, 5);
    }

//Альфа версия системы толкания
    void OnControllerColliderHit(ControllerColliderHit obj)
    {
        if(obj.rigidbody && pushing)
        {
            Rigidbody hit = obj.collider.attachedRigidbody;
            Debug.Log("PUSH!");
            hit.AddForce(transform.forward * 0.5f, ForceMode.Impulse);
        }
        
    }
}