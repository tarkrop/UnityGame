using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 inputVec;
    Vector2 prevVec;
    Vector3 rotateVec;
    public float rotationSpeed;
    public float moveSpeed;
    float angle;

    Rigidbody2D rigid;
    [SerializeField]
    public BoxCollider2D playerArea;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        GamePadSetting();

    }

    void Update()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerMove();
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
        if (inputVec == null) inputVec = new Vector2(0f, 1f);

        if (inputVec.x == 0f && inputVec.y == 0f)
        {
            inputVec = prevVec;
        }
    }
    private void PlayerMove()
    {
        prevVec = inputVec;

        float hAxis = inputVec.x;
        float vAxis = inputVec.y;
        if (hAxis != 0 || vAxis != 0)
        {
            float zAxis = Mathf.Atan2(hAxis, vAxis) * Mathf.Rad2Deg;
            angle = Mathf.LerpAngle(angle, -zAxis, Time.fixedDeltaTime * rotationSpeed);
            rigid.MoveRotation(Quaternion.Euler(Vector3.forward * angle));
        }
        rigid.velocity = transform.up * moveSpeed * Time.fixedDeltaTime;
    }

    private void GamePadSetting()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            // Gamepad Device를 사용하도록 설정합니다.
            InputDevice gamepadDevice = Gamepad.current;

            if (gamepadDevice != null)
            {
                playerInput.SwitchCurrentControlScheme("Gamepad", gamepadDevice);
            }
            else
            {
                Debug.Log("Gamepad device not found.");
            }
        }
        else
        {
            Debug.Log("PlayerInput component not found.");
        }

    }

}
