using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public float speed = 6.0f;
    public float sprintMultiplier = 1.5f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    public float sensitivity = 100f;

    private float xRotation = 0f;
    private Vector3 velocity;
    private bool isGrounded;

    private CharacterController charController;
    private Transform cam;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        charController = GetComponent<CharacterController>();
        cam = transform.Find("Camera");
    }

    void Update()
    {
        Move();
        CameraMovement();
    }

    void Move()
    {
        isGrounded = charController.isGrounded;

        float moveSpeed = Input.GetKey(KeyCode.LeftShift) ? speed * sprintMultiplier : speed;
        Vector3 move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");

        charController.Move(Vector3.ClampMagnitude(move, 1.0f) * moveSpeed * Time.deltaTime);

        // Прыжок
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Гравитация
        velocity.y += gravity * Time.deltaTime;
        charController.Move(velocity * Time.deltaTime);
    }

    void CameraMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -70f, 70f);

        cam.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }
}
