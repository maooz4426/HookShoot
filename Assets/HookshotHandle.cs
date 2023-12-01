using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;


namespace StarterAssets
{
    public class HookshotHandle : MonoBehaviour
    {


        //private State state;
        private Vector3 hookshotPosition;
        //private float hookshotSize;
        private Vector3 characterVelocityMomentum;
        public LineRenderer lr;
        private CharacterController _controller;


        [Header("Hookshot")]
        [SerializeField] private Transform debugHitPosition;
        [SerializeField] private LayerMask Hookable;
        [SerializeField] private Transform hookshotTransform;




        public void Awake()
        {
            lr = GetComponent<LineRenderer>();
            lr.enabled = false;
            _controller = GetComponent<CharacterController>();
        }


        //Hookshot
        //public void HandleHookshotStart()
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 100f, Hookable))
        //        {


        //            //hookshotSize = 0f;
        //            //state = State.HookshotThrown;
        //            //Debug.Log(hit.point);
        //            //state = State.HookshotFlyingPlayer;
        //        }
        //    }
        //}

        //���[�v�`��
        private Vector3 currentHookshot;
        public void DrawRope()
        {
            currentHookshot = Vector3.Lerp(currentHookshot, hookshotTransform.position, Time.deltaTime * 8f);
            lr.SetPosition(0, hookshotTransform.position);
            lr.SetPosition(1, hookshotPosition);
        }

        ////private void HandleHookshotThrown()
        ////{
        ////    hookshotTransform.LookAt(hookshotPosition);

        ////    float hookshotThrownSpeed = 5f;
        ////    hookshotSize += hookshotThrownSpeed * Time.deltaTime;
        ////    hookshotTransform.localScale = new Vector3(1, 1, hookshotSize);
        ////}

        public bool isreached;
        public void HandleHookshotMovement(Vector3 hitPoint)
        {
            
            lr.enabled = true;

            // debugHitPosition �̈ʒu�� hit.point �ɐݒ�
            debugHitPosition.position = hitPoint;
            hookshotPosition = hitPoint;

            //normalized�̓x�N�g���̐��K��(�[���x�N�g���j
            Vector3 hookshotDir = (hookshotPosition - transform.position).normalized;

            float hookshotSpeedMin = 10f;
            float hookshotSpeedMax = 40f;
            float hookshotSpeed = Mathf.Clamp(Vector3.Distance(transform.position, hookshotPosition), hookshotSpeedMin, hookshotSpeedMax);
            float hookshotSpeedMultipulier = 2f;
            _controller.Move(hookshotDir * hookshotSpeed * hookshotSpeedMultipulier * Time.deltaTime);

            float reachedHookshotPositionDistance = 1f;
            isreached = Vector3.Distance(transform.position, hookshotPosition) < reachedHookshotPositionDistance;
            if (isreached)
            {
                //state = State.Normal;
                HookDelete();

                return;
            }

            //�L�����Z��
            if (Input.GetMouseButtonDown(0))
            {
                float momentumExtraSpeed = 7f;
                characterVelocityMomentum = hookshotDir * hookshotSpeed * momentumExtraSpeed;
                float jumpSpeed = 40f;
                characterVelocityMomentum += Vector3.up * jumpSpeed;
                //state = State.Normal;
                HookDelete();
                return;
            }
        }

        private void HookDelete()
        {
            if (lr != null)
            {
                lr.enabled = false;
            }
        }
    }
}
