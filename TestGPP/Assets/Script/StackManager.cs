using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Stack.Core
{
    public class StackManager : MonoBehaviour
    {
        [Header("Stack Configuration")]
        [SerializeField] private StackBlockSO stackBlockSO;

        public Transform SpawnPointX => spawnPointX;
        [SerializeField] private Transform spawnPointX;
        public Transform LimitPointX => limitPointX;
        [SerializeField] private Transform limitPointX;
        public Transform SpawnPointZ => spawnPointZ;
        [SerializeField] private Transform spawnPointZ;
        public Transform LimitPointZ => limitPointZ;
        [SerializeField] private Transform limitPointZ;

        [SerializeField] private MeshRenderer startPlatform;

        private DirectionAxis directionAxis = DirectionAxis.A_FORWARD;
        private float stackBlockSpeed;
        private Vector3 stackBlockScale;
        private Vector2 spawnOffset;
        private List<StackBlock> stackBlocks = new List<StackBlock>();

        [Header("Events")]
        [SerializeField] private VoidEventSO OnStackBlockPlaced;
        [SerializeField] private VoidEventSO OnGameRestart;

        int comboCount = 0;

        public enum DirectionAxis
        {
            A_NONE,
            A_FORWARD,
            A_RIGHT
        }

        private void Start()
        {
            Init();
        }
        void Init()
        {
            spawnOffset = Vector2.zero;

            stackBlockSpeed = stackBlockSO.SpeedMov;
            stackBlockScale = stackBlockSO.StackPrefab.transform.localScale;

            startPlatform.material.color = stackBlockSO.Gradient.GetColor();
        }

        private void OnEnable()
        {
            OnStackBlockPlaced.OnEventRaised += SpawnStackBlock;
            OnGameRestart.OnEventRaised += RestartGame;
        }

        private void OnDisable()
        {
            OnStackBlockPlaced.OnEventRaised -= SpawnStackBlock;
            OnGameRestart.OnEventRaised -= RestartGame;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                SpawnStackBlock();
            }
        }

        private void SpawnStackBlock()
        {
            Vector3 spawnPos = (directionAxis == DirectionAxis.A_RIGHT) ? spawnPointX.position : spawnPointZ.position;
            spawnPos.y = GameManager.Instance.StackCount * stackBlockSO.OffsetY;
            spawnPos += transform.right * spawnOffset.x + transform.forward * spawnOffset.y;

            Vector3 centerPos = GameManager.Instance.StackCount * stackBlockSO.OffsetY * transform.up + transform.right * spawnOffset.x + transform.forward * spawnOffset.y;

            StackBlock stackBlock = Instantiate(stackBlockSO.StackPrefab, spawnPos, Quaternion.identity, transform).
                Init(directionAxis, stackBlockSpeed, centerPos, stackBlockSO.Gradient.GetColor(), this);
             stackBlock.transform.localScale = stackBlockScale;

            stackBlockSpeed += stackBlockSO.AccMov;
            directionAxis = (directionAxis == DirectionAxis.A_RIGHT) ? DirectionAxis.A_FORWARD : DirectionAxis.A_RIGHT;
            stackBlocks.Add(stackBlock);

            GameManager.Instance.IncreaseStackCount();
        }

        private void RestartGame()
        {
            Init();

            StartCoroutine(DestroyStack());
        }

        private IEnumerator DestroyStack()
        {
            for(int i = stackBlocks.Count - 1; i >= 0; i--)
            {
                stackBlocks[i].Explode();
                yield return new WaitForSeconds(0.05f);
            }

            stackBlocks.Clear();
        }

        public void SetStackBlockScale(Vector3 newScale)
        {
            stackBlockScale = newScale;
        }

        public void SetSpawnOffset(Vector2 newSpawnOffset)
        {
            spawnOffset = newSpawnOffset;
        }

        public void ResetComboCount()
        {
            comboCount = 0;
        }

        public void IncreaseComboCount()
        {
            comboCount++;
        }

        public int GetComboCount()
        {
            return comboCount;
        }
    }
}
