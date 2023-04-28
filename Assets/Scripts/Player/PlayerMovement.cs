using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{

    private Animator anim;
    private PhotonView myView;
    private Rigidbody rb;
    private Transform cameraTransform;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 2f, -5f);

    private float mouseX;
    private float mouseY;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;

        // Disable cursor and lock it to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        anim = GetComponent<Animator>();
        myView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (myView.IsMine)
        {
            // Handle jumping
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    private void FixedUpdate()
    {
        if (myView.IsMine)
        {
            Move();
        }
    }

    void Move()
    {
        // Handle movement
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(horizontalMovement, 0f, verticalMovement);

        // Calculate movement direction based on the camera's forward direction
        Vector3 cameraForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movementDirection = cameraForward * movement.z + cameraTransform.right * movement.x;

        rb.MovePosition(transform.position + movementDirection.normalized * moveSpeed * Time.fixedDeltaTime);

        // Handle rotation
        mouseX += Input.GetAxis("Mouse X") * 2;
        mouseY -= Input.GetAxis("Mouse Y") * 2;

        mouseY = Mathf.Clamp(mouseY, -45f, 45f);

        Quaternion cameraRotation = Quaternion.Euler(mouseY, mouseX, 0f);
        Vector3 cameraPosition = transform.position + cameraRotation * cameraOffset;

        cameraTransform.rotation = cameraRotation;
        cameraTransform.position = cameraPosition;

        if (horizontalMovement > 0 || verticalMovement > 0)
        {
            anim.SetBool("isRun", true);
        }
        else
        {
            anim.SetBool("isRun", false);
        }
    }

}
