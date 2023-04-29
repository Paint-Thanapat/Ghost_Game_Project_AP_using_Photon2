using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject cameraHolder;
    [SerializeField] GameObject modelHolder;
    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
    [SerializeField] Item[] items;

    int itemIndex;
    int previousItemIndex = -1;

    float verticalLookRotation;
    public bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    Animator anim;
    Rigidbody rb;

    PhotonView PV;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (PV.IsMine)
        {
            EquipItem(0);
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
        }
    }

    private void Update()
    {
        if (!PV.IsMine)
            return;

        Look();
        Move();
        Jump();


        // checking player press 1 or 2 on keybroad to switch the tool
        for (int i = 0; i < items.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItem(i);
                break;
            }
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            EquipItem(itemIndex + 1 >= items.Length ? 0 : itemIndex + 1);
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            EquipItem(itemIndex - 1 < 0 ? items.Length - 1 : itemIndex - 1);
        }
    }

    private void FixedUpdate()
    {
        if (!PV.IsMine)
            return;

        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.eulerAngles = new Vector3(-verticalLookRotation, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        bool isSprint = Input.GetKey(KeyCode.LeftShift);

        Vector3 moveDir = new Vector3(horizontal, 0, vertical).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (isSprint ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);

        // Animation
        if (horizontal != 0 || vertical != 0)
        {
            if (isSprint)
            {
                anim.SetBool("isRun", true);
                anim.SetBool("isWalk", true);
            }
            else
            {
                anim.SetBool("isWalk", true);
                anim.SetBool("isRun", false);
            }
        }
        else
        {
            anim.SetBool("isWalk", false);
            anim.SetBool("isRun", false);
        }

        // Rotate the Model
        Vector3 lookPoint = Vector3.Slerp(modelHolder.transform.forward.normalized, transform.TransformDirection(moveAmount).normalized, smoothTime);
        modelHolder.transform.LookAt(transform.position + lookPoint);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);

            anim.SetTrigger("isJump");
        }
    }

    void EquipItem(int _index)
    {
        if (_index == previousItemIndex)
            return;

        itemIndex = _index;

        items[itemIndex].itemObject.SetActive(true);

        if (previousItemIndex != -1)
        {
            items[previousItemIndex].itemObject.SetActive(false);
        }

        previousItemIndex = itemIndex;

        if (PV.IsMine)
        {
            ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
            hash.Add("ItemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, HashTable changedProps)
    {
        if (!PV.IsMine && targetPlayer == PV.Owner)
        {
            EquipItem((int)changedProps["ItemIndex"]);
        }
    }

    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }

}
