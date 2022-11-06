using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Stack.Core
{
    public class StackBlock : MonoBehaviour
    {
        private StackManager.DirectionAxis directionAxis;
        private float speed;
        private Rigidbody rb;

        bool isMoving = true;
        int stackNum = 0;
        float offsetY = 0;

        [SerializeField] private VoidEventSO OnStackBlockPlaced;

        public StackBlock Init(StackManager.DirectionAxis dA, float _speed, float _offsetY, int _stackNum)
        {
            rb = GetComponent<Rigidbody>();

            directionAxis = dA;
            offsetY = _offsetY;
            speed = _speed;
            stackNum = _stackNum;
            return this;
        }

        // Update is called once per frame
        void Update()
        {
            if (isMoving)
            {
                Move();
                CheckState();
            }
        }

        private void Move()
        {
            switch (directionAxis)
            {
                case StackManager.DirectionAxis.A_FORWARD:
                    {
                        rb.MovePosition(transform.position - (transform.forward * speed * Time.deltaTime));
                    }
                    break;
                case StackManager.DirectionAxis.A_RIGHT:
                    {
                        rb.MovePosition(transform.position + (transform.right * speed * Time.deltaTime));
                    }
                    break;
            }
        }

        private void CheckState()
        {
            if (Vector3.Distance(Vector3.zero + transform.up * offsetY * stackNum, transform.localPosition) < 0.05f)
            {
                StopBlock();
                transform.localPosition = Vector3.zero + transform.up * offsetY * stackNum;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.transform.CompareTag("Player") && isMoving)
            {
                StopBlock();
            }
        }

        void StopBlock()
        {
            isMoving = false;
            //rb.isKinematic = false;
            //rb.useGravity = true;
            OnStackBlockPlaced.RaisedEvent();
        }
    }
}