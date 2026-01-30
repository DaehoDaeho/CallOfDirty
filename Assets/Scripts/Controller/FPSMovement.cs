using Unity.VisualScripting;
using UnityEngine;

public class FPSMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 6.0f;

    [SerializeField]
    private float jumpHeight = 2.0f;

    [SerializeField]
    private float gravity = -9.8f;

    [SerializeField]
    private float groundCheckDistance = 0.4f;   // 지면에 닿아있는지 체크하기 위한 여유 거리.

    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private CharacterController controller;

    [SerializeField]
    private int maxJumpCount = 1;

    private Vector3 velocity;   // 낙하속도.
    private bool isGrounded;
    private float addictiveSpeed = 0.0f;

    private int jumpCount = 0;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);

        if (isGrounded == true)
        {
            jumpCount = 0;
        }

        // 아래로 떨어지려고 할 때.
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2.0f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        addictiveSpeed = 0.0f;
        if(Input.GetKey(KeyCode.LeftControl) == true)
        {
            addictiveSpeed = 5.0f;
        }

        Vector3 move = transform.right * x + transform.forward * z;

        //controller.Move(move * moveSpeed * Time.deltaTime);
        controller.Move(move * (moveSpeed + addictiveSpeed) * Time.deltaTime);

        //if (Input.GetKeyDown(KeyCode.Space) == true && isGrounded == true)
        if (Input.GetKeyDown(KeyCode.Space) == true && jumpCount < maxJumpCount)
        {
            ++jumpCount;

            // 점프 공식 : sqrt(h * -2 * g)
            // 높이만큼 뛰기 위해 필요한 순간 속력을 구하는 물리 공식.
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        // 낙하 이동.
        controller.Move(velocity * Time.deltaTime);
    }

    /// <summary>
    /// 이동 중인지 여부를 반환.
    /// </summary>
    /// <returns></returns>
    public bool IsMoving()
    {
        return controller.velocity.sqrMagnitude > 0.1f;
    }
}
