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
            rb.MovePosition(transform.position + (direction * speed));
        }

        private void CheckState()
        {
            if (Vector3.Distance(centerStack, transform.localPosition) < thresholdPerfect)
            {
                StopBlock(true);
                transform.localPosition = centerStack;
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
                ComboParticleEffect();

            isMoving = false;
            OnStackBlockPlaced.RaisedEvent();
        }

        void ComboParticleEffect()
        {
            stackManager.IncreaseComboCount();

            // Normal combo
            ParticleSystem ps = Instantiate(pSPerfectCombo, pSPerfectCombo.transform.position, pSPerfectCombo.transform.rotation, transform);
            ps.Play();

            int comboStart = 4;
            // Combo x+
            if (stackManager.GetComboCount() >= comboStart)
            {
                StartCoroutine(SpawnComboParticles(stackManager.GetComboCount() - comboStart, 0.4f));
            }
        }

        IEnumerator SpawnComboParticles(int count, float delay)
        {
            float scale = 1f;

            if (count > 4)
                count = 4;
            while(count >= 0)
            {
                ParticleSystem ps = Instantiate(pSPerfect, pSPerfect.transform.position, pSPerfect.transform.rotation, transform);
                ps.Play();
                ps.transform.DOScale(scale, ps.startLifetime);

                scale += 0.2f;
                count--;
                yield return new WaitForSeconds(delay);   
            }
        }

        void SplitBlock()
        {
            // Break block by faking
            float distToCenter = Vector3.Distance(centerStack, transform.localPosition);
            Vector3 newPos = transform.position + direction * distToCenter * 0.5f;

            Vector3 newScale = Vector3.zero;
            float initialScale = 0f;

            // Create newBlock
            GameObject newBlock = Instantiate(gameObject);

            switch (directionAxis)
            {
                case StackManager.DirectionAxis.A_FORWARD:
                    {
                        newScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z - distToCenter);
                        newBlock.transform.localScale = new Vector3(newBlock.transform.localScale.x, newBlock.transform.localScale.y, distToCenter);
                        initialScale = transform.localScale.z;
                    }
                    break;
                case StackManager.DirectionAxis.A_RIGHT:
                    {
                        newScale = new Vector3(transform.localScale.x - distToCenter, transform.localScale.y, transform.localScale.z);
                        newBlock.transform.localScale = new Vector3(distToCenter, newBlock.transform.localScale.y, newBlock.transform.localScale.z);
                        initialScale = transform.localScale.x;
                    }
                    break;
            }

            transform.position = newPos;
            transform.localScale = newScale;

            newBlock.transform.position = centerStack - (initialScale - distToCenter) * direction;

            Rigidbody rb = newBlock.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;

            Destroy(newBlock.GetComponent<StackBlock>());
            Destroy(newBlock.gameObject, 4f);

            // Update stackPrefab
            stackManager.SetSpawnOffset(new Vector2(newPos.x, newPos.z));
            stackManager.SetStackBlockScale(newScale);

            stackManager.ResetComboCount();

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
