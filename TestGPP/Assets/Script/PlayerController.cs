using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Stack.Core
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float jumpForce = 10f;

        [Header("Events")]
        [SerializeField] private VoidEventSO OnStackBlockPlaced;
        [SerializeField] private VoidEventSO OnGameEnd;

        public Rigidbody Rb => rb;
        private Rigidbody rb;
        private bool isJumping = false;

        private void OnEnable()
        {
            OnStackBlockPlaced.OnEventRaised += CheckPlayerPos;
        }

        private void OnDisable()
        {
            OnStackBlockPlaced.OnEventRaised -= CheckPlayerPos;
        }

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && !isJumping && GameManager.Instance.CheckGameState(GameManager.GameState.GS_PLAY))
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Death"))
            {
                OnGameEnd.RaisedEvent();
            }
        }

        void CheckPlayerPos()
        {
            if(transform.position.y < (GameManager.Instance.StackCount - 1) * GameManager.Instance.StackBlockSO.OffsetY)
                OnGameEnd.RaisedEvent();
        }
    }
}
