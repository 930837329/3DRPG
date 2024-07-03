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


    [BoxGroup("����"), SerializeField]
    protected KeyCode forwordKey;
    [BoxGroup("����"), SerializeField]
    protected KeyCode backKey;
    [BoxGroup("����"), SerializeField]
    protected KeyCode leftKey;
    [BoxGroup("����"), SerializeField]
    protected KeyCode rightKey;
    [BoxGroup("����"), SerializeField]
    protected KeyCode jumpKey;



    [BoxGroup("����"), SerializeField]
    protected float moveSpeed = 5.0f; // �ƶ��ٶ�

    [BoxGroup("����"), SerializeField]
    protected float zxSpeed = 4.0f; // �ƶ��ٶ�
    [BoxGroup("����"), SerializeField]
    protected float jumpSpeed = 2.0f; // �ƶ��ٶ�
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
        Vector3 dir = new Vector3(hor, 0, ver).normalized; // ��һ����������

        

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


            Vector3 normalVector = new Vector3(0, 1, 0);  // �������ķ������������磬������ˮƽ�ģ�
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

