using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mock_CameraController : MonoBehaviour
{
#if UNITY_EDITOR
    public float moveSpeed = 5.0f;
    public float sensitivity = 2.0f;
    public Transform playerBody; // The transform to move and rotate

    private float rotationX = 0.0f;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Rotation
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);

        // Movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = (playerBody.forward * verticalInput + playerBody.right * horizontalInput).normalized;
        moveDirection.y = 0; // Prevent vertical movement

        playerBody.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
#endif
}
