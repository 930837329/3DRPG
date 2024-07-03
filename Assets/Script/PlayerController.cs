using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using DG.Tweening;
using static UnityEngine.Rendering.DebugUI;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;
    private Rigidbody rb;

    public Transform cameraTransform;


    [BoxGroup("按键"), SerializeField]
    protected KeyCode forwordKey;
    [BoxGroup("按键"), SerializeField]
    protected KeyCode backKey;
    [BoxGroup("按键"), SerializeField]
    protected KeyCode leftKey;
    [BoxGroup("按键"), SerializeField]
    protected KeyCode rightKey;
    [BoxGroup("按键"), SerializeField]
    protected KeyCode jumpKey;



    [BoxGroup("参数"), SerializeField]
    protected float moveSpeed = 5.0f; // 移动速度

    [BoxGroup("参数"), SerializeField]
    protected float zxSpeed = 4.0f; // 移动速度
    [BoxGroup("参数"), SerializeField]
    protected float jumpSpeed = 2.0f; // 移动速度
    // Start is called before the first frame update

    private float dua = 1.0f;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }




    void Update()
    {

        float delta = Time.deltaTime;
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(hor, 0, ver).normalized; // 归一化方向向量

        

        if (dir != Vector3.zero)
        {


            Vector3 targetDir = Vector3.zero;
            //float moveOverride = inputHandler.moveAmount;
            targetDir = cameraTransform.forward * ver;
            targetDir += cameraTransform.right * hor;
            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
                targetDir = transform.forward;

            Quaternion tr = Quaternion.LookRotation(targetDir);

            
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, zxSpeed * delta);
            transform.rotation = targetRotation;


            Vector3 moveDirection = cameraTransform.forward * ver;
            moveDirection += cameraTransform.right * hor;
            moveDirection.Normalize();
            moveDirection.y = 0;

            moveDirection *= moveSpeed;


            Vector3 normalVector = new Vector3(0, 1, 0);  // 假设地面的法线向量（例如，地面是水平的）
            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rb.velocity = projectedVelocity;


            animator.SetInteger("walkState", 2);
        }
        else
        {
            animator.SetInteger("walkState", 0);
        }




    }

}

