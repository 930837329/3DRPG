using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

namespace SG
{
    public class CameraHandler : MonoBehaviour
    {
        private InputSystem inputSystem;
        public Transform targetTransform;
        public Transform cameraTransform;
        public Transform cameraPivotTransform;
        private Transform myTransform;
        private Vector3 cameraTransformPosition;
        private LayerMask ignoreLayers;

        private Vector3 cameraFollowVelocity = Vector3.zero;

        public static CameraHandler singleton;

        public float lookSpeed = 0.1f;
        public float followSpeed = 0.1f;
        public float pivotSpeed = 0.03f;


        private float defaultPosition;
        private float lookAngle;
        private float pivotAngle;
        public float minimumPivot = -35;
        public float maximumPivot = 35;

        private void Awake()
        {
            inputSystem = new InputSystem();
            
            singleton = this;
            myTransform = transform;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
        }




        private void OnEnable()
        {
            inputSystem.Enable();
        }
        private void OnDisable()
        {
            inputSystem.Disable();
        }

        private void FixedUpdate()
        {
            float mm2 = Input.GetAxis("Mouse ScrollWheel");
            float mouseX = Input.GetAxis("Mouse X");//获取鼠标X轴移动
            float mouseY = Input.GetAxis("Mouse Y");//获取鼠标Y轴移动

            float delta = Time.fixedDeltaTime;
            FollowTarget(delta);
            HandlecameraRotation(delta, mouseX, mouseY);
            Vector2 mm = inputSystem.Mouse.Scroll.ReadValue<Vector2>().normalized;
            
            //Debug.Log(mm);

            //cameraTransform.Translate(Vector3.forward * mm.y);//速度可调  自行调整
            if (mm2 > 0.0f)
            {
                cameraTransform.Translate(Vector3.forward * 1f);//速度可调  自行调整
            }
            //Input.GetAxis("Mouse ScrollWheel")
            if (mm2 < 0.0f)
            {
                cameraTransform.Translate(Vector3.forward * -1f);//速度可调  自行调整
            }


        }
        public void FollowTarget(float delta)
        {

            //Vector3 targetPosition = Vector3.Lerp(myTransform.position, targetTransform.position, delta / followSpeed);
            Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
            myTransform.position = targetPosition;
        }

        public void HandlecameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            lookAngle += (mouseXInput * lookSpeed) / delta;
            pivotAngle -= (mouseYInput * pivotSpeed) / delta;
            pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

            Vector3 rotation = Vector3.zero;
            rotation.y = lookAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            myTransform.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = pivotAngle;

            targetRotation = Quaternion.Euler(rotation);
            cameraPivotTransform.localRotation = targetRotation;

            //cameraTransform.RotateAround(targetTransform.position, Vector3.up, mouseXInput * 5);
            //cameraTransform.RotateAround(targetTransform.position, cameraTransform.right, mouseYInput * 5);
        }

    }



}

