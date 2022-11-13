using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Game.Stack.Core
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float jumpForce = 10f;

        [Header("Stamina Float")]
        [SerializeField]
        private Image staminaSliderBGImage;
        [SerializeField]
        private Image staminaSliderFillImage;

        [SerializeField]
        private float maxTimeFly = 1f;

        [Header("Events")]
        [SerializeField] private VoidEventSO OnStackBlockPlaced;
        [SerializeField] private VoidEventSO OnGameEnd;

        public Rigidbody Rb => rb;
        private Rigidbody rb;

        private Animator anim;

        private float currentTimeFly = 0f;
        private float percentageStamina = 1f;

        private PlayerState playerState = PlayerState.PS_IDLE;

        public enum PlayerState
        {
            PS_IDLE,
            PS_JUMP,
            PS_FLOAT,
            PS_FALL,
            PS_DEAD
        }

        private void OnEnable()
        {
            OnStackBlockPlaced.OnEventRaised += MovePlayerToCenter;
        }

        private void OnDisable()
        {
            OnStackBlockPlaced.OnEventRaised -= MovePlayerToCenter;
        }

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.Instance.CheckGameState(GameManager.GameState.GS_PLAY))
            {
#if UNITY_EDITOR
                if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && playerState == PlayerState.PS_IDLE)
                    Jump();
                else if ((Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space)) && (playerState == PlayerState.PS_JUMP || playerState == PlayerState.PS_FLOAT) && percentageStamina > 0f)
                    FloatBall();
                else if ((Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space) || percentageStamina <= 0f) && playerState == PlayerState.PS_FLOAT)
                    FallBall();
#endif

#if UNITY_IOS || UNITY_ANDROID
                // Handle screen touches.
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began && playerState == PlayerState.PS_IDLE)
                    {
                        Jump();
                    }
                    else if ((touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) && (playerState == PlayerState.PS_JUMP || playerState == PlayerState.PS_FLOAT) && percentageStamina > 0f)
                    {
                        FloatBall();
                    }
                    else if ((touch.phase == TouchPhase.Ended || percentageStamina <= 0f) && playerState == PlayerState.PS_FLOAT)
                    {
                        FallBall();
                    }
                }
#endif
            }
        }

        private void Jump()
        {
            rb.AddForce(transform.up * jumpForce);
            playerState = PlayerState.PS_JUMP;

            transform.DOPunchScale(Vector3.one * 0.05f, 0.25f, 2);
        }

        private void FloatBall()
        {
            if (rb.velocity.y < 0)
            {
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
                playerState = PlayerState.PS_FLOAT;

                float fadeTime = 0.1f;
                staminaSliderBGImage.DOFade(1f, fadeTime);
                staminaSliderFillImage.DOFade(1f, fadeTime);

                anim.SetBool("Float", true);
            }

            currentTimeFly += Time.deltaTime;
            percentageStamina = Mathf.Clamp01(1 - (currentTimeFly / maxTimeFly));
            staminaSliderFillImage.fillAmount = percentageStamina;
        }

        private void FallBall()
        {
            rb.useGravity = true;
            playerState = PlayerState.PS_FALL;
            currentTimeFly = 0f;
            percentageStamina = 1f;

            float fadeTime = 0.1f;
            staminaSliderBGImage.DOFade(0f, fadeTime);
            staminaSliderFillImage.DOFade(0f, fadeTime);

            anim.SetBool("Float", false);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag("StackBlock") && GetPlayerBottomPositionY() > collision.transform.position.y && !CheckPlayerState(PlayerState.PS_FLOAT))
                playerState = PlayerState.PS_IDLE;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Death") && GameManager.Instance.CheckGameState(GameManager.GameState.GS_PLAY) && !CheckPlayerState(PlayerState.PS_DEAD))
            {
                OnGameEnd.RaisedEvent();
                FallBall();
                playerState = PlayerState.PS_DEAD;
            }
        }

        void MovePlayerToCenter()
        {
            Vector3 centerPos = GameManager.Instance.StackManagerInstace.GetCenterPosition();
            centerPos.y = transform.position.y;
            transform.DOMove(centerPos, 0.1f);
        }

        public float GetPlayerBottomPositionY()
        {
            return transform.position.y - transform.localScale.y * 0.5f;
        }

        public bool CheckPlayerState(PlayerState state)
        {
            return (playerState == state);
        }
    }
}
