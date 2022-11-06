using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Stack.Core
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float jumpForce = 10f;

        private Rigidbody rb;
        private bool isJumping = false;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0) && !isJumping)
                Jump();
#endif

#if UNITY_IPHONE || UNITY_ANDROID
        
#endif
        }

        private void Jump()
        {
            rb.AddForce(transform.up * jumpForce);
            isJumping = true;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag("StackBlock"))
                isJumping = false;
        }
    }
}