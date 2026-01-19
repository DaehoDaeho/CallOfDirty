using UnityEngine;

public class FPSCameraController : MonoBehaviour
{
    [SerializeField]
    private float mouseSensitivity = 200.0f;

    [SerializeField]
    private Transform playerBody;

    private float xRotation = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 상하 회전 계산.
        // 마우스를 위로 올리면 시선이 위를 보도록.

        //if(OptionData.invertMouseY == true)
        //{
        //    xRotation += mouseY;
        //}
        //else
        //{
        //    xRotation -= mouseY;
        //}

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);
        //xRotation = Mathf.Clamp(xRotation, -OptionData.mouseAngleLimit, OptionData.mouseAngleLimit);

        transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);

        if(playerBody != null)
        {
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
