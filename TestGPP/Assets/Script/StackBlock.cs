using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace Game.Stack.Core
{
    public class StackBlock : MonoBehaviour
    {
        private StackManager.DirectionAxis directionAxis;
        private float speed;
        public Rigidbody Rb => rb;
        private Rigidbody rb;
        private MeshRenderer meshRenderer;

        bool isMoving = true;

        float thresholdPerfect = 0.05f;
        Vector3 centerStack;
        Vector3 direction;

        [SerializeField]
        private ParticleSystem pSPerfect;
        [SerializeField]
        private ParticleSystem pSPerfectCombo;

        [Header("Events")]
        [SerializeField] private VoidEventSO OnStackBlockPlaced;

        StackManager stackManager;

        bool moveToLimit = true;

        public StackBlock Init(StackManager.DirectionAxis dA, float _speed, Vector3 _centerStack, Color col, StackManager _stackManager)
        {
            rb = GetComponent<Rigidbody>();
            meshRenderer = GetComponent<MeshRenderer>();

            stackManager = _stackManager;

            ChangeColor(col);

            centerStack = _centerStack;
            directionAxis = dA;
            speed = _speed;

            switch (directionAxis)
            {
                case StackManager.DirectionAxis.A_FORWARD:
                    {
                        direction = -transform.forward;
                    }
                    break;
                case StackManager.DirectionAxis.A_RIGHT:
                    {
                        direction = transform.right;
                    }
                    break;
            }

            return this;
        }

        // Update is called once per frame
        void Update()
        {
            if (isMoving && GameManager.Instance.CheckGameState(GameManager.GameState.GS_PLAY))
            {
                Move();
                CheckState();
            }
        }

        private void Move()
        {
            if(moveToLimit)
                rb.MovePosition(transform.position + (direction * speed));
            else
                rb.MovePosition(transform.position - (direction * speed));
        }

        private void CheckState()
        {
            switch (directionAxis)
            {
                case StackManager.DirectionAxis.A_FORWARD:
                    {
                        Transform moveToPoint;

                        if (moveToLimit)
                            moveToPoint = stackManager.LimitPointZ;
                        else
                            moveToPoint = stackManager.SpawnPointZ;

                        if (Mathf.Abs(moveToPoint.position.z - transform.position.z) < 0.25F)
                        {
                            moveToLimit = !moveToLimit;
                        }
                    }
                    break;
                case StackManager.DirectionAxis.A_RIGHT:
                    {
                        Transform moveToPoint;

                        if (moveToLimit)
                            moveToPoint = stackManager.LimitPointX;
                        else
                            moveToPoint = stackManager.SpawnPointX;

                        if (Mathf.Abs(moveToPoint.position.x - transform.position.x) < 0.25F)
                        {
                            moveToLimit = !moveToLimit;
                        }
                    }
                    break;
            }

           
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.transform.CompareTag("Player") && isMoving)
            {
                if(collision.transform.position.y > transform.position.y)
                    StopBlock(Vector3.Distance(centerStack, transform.localPosition) < thresholdPerfect);
            }
        }

        void StopBlock(bool isPerfect)
        {
            // Check if is perfect placed
            if (!isPerfect)
                SplitBlock();
            else
            {
                transform.localPosition = centerStack;
                ComboParticleEffect();
            }

            isMoving = false;
            OnStackBlockPlaced.RaisedEvent();
        }

        void ComboParticleEffect()
        {
            Audio.AudioManager.Instance.PlaySFX("SFX_Perfect");

            stackManager.IncreaseComboCount();

            // Normal combo
            ParticleSystem ps = Instantiate(pSPerfect, pSPerfect.transform.position, pSPerfect.transform.rotation, transform);
            ps.Play();

            int comboStart = 4;
            // Combo x+
            if (stackManager.GetComboCount() >= comboStart)
            {
                StartCoroutine(SpawnComboParticles(stackManager.GetComboCount() - comboStart, pSPerfectCombo.startLifetime * 0.7f));
            }
        }

        IEnumerator SpawnComboParticles(int count, float delay)
        {
            float scale = 1f;

            if (count > 4)
                count = 4;
            while(count >= 0)
            {
                ParticleSystem ps = Instantiate(pSPerfectCombo, pSPerfectCombo.transform.position, pSPerfectCombo.transform.rotation, transform);
                ps.Play();
                ps.transform.DOScale(scale, ps.startLifetime * 0.5f);

                scale += 0.1f;
                count--;
                yield return new WaitForSeconds(delay);   
            }
        }

        void SplitBlock()
        {
            // Break block by faking
            float distToCenter = 0f;
            Vector3 newScale = Vector3.zero;
            float initialScale = 0f;

            // Create newBlock
            GameObject newBlock = Instantiate(gameObject);

            switch (directionAxis)
            {
                case StackManager.DirectionAxis.A_FORWARD:
                    {
                        distToCenter = transform.localPosition.z - centerStack.z;
                        newScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z - Mathf.Abs(distToCenter));
                        newBlock.transform.localScale = new Vector3(newBlock.transform.localScale.x, newBlock.transform.localScale.y, Mathf.Abs(distToCenter));
                        initialScale = transform.localScale.z;
                    }
                    break;
                case StackManager.DirectionAxis.A_RIGHT:
                    {
                        distToCenter = centerStack.x - transform.localPosition.x;
                        newScale = new Vector3(transform.localScale.x - Mathf.Abs(distToCenter), transform.localScale.y, transform.localScale.z);
                        newBlock.transform.localScale = new Vector3(Mathf.Abs(distToCenter), newBlock.transform.localScale.y, newBlock.transform.localScale.z);
                        initialScale = transform.localScale.x;
                    }
                    break;
            }

            Vector3 newPos = transform.position + direction * distToCenter * 0.5f;
            transform.position = newPos;
            transform.localScale = newScale;

            if(Mathf.Sign(distToCenter) > 0)
                newBlock.transform.position = centerStack - (initialScale + distToCenter) * direction * 0.5f;
            else
                newBlock.transform.position = centerStack + (initialScale - distToCenter) * direction * 0.5f;

            Rigidbody rb = newBlock.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;

            Destroy(newBlock.GetComponent<StackBlock>());
            Destroy(newBlock.gameObject, 4f);

            // Update stackPrefab
            stackManager.SetSpawnOffset(new Vector2(newPos.x, newPos.z));
            stackManager.SetStackBlockScale(newScale);

            stackManager.ResetComboCount();

            Audio.AudioManager.Instance.PlaySFX("SFX_Chop");
            Audio.AudioManager.Instance.BreakIncrementPitch("SFX_Perfect");

        }

        public void Explode()
        {
            rb.isKinematic = false;
            rb.AddForce(500f * ((directionAxis == StackManager.DirectionAxis.A_FORWARD) ? transform.forward : transform.right));
            Destroy(gameObject, 1f);
        }

        public void ChangeColor(Color col)
        {
            meshRenderer.material.color = col;
        }
    }
}
