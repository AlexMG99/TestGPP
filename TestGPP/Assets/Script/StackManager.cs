using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Stack.Core
{
    public class StackManager : MonoBehaviour
    {
        [SerializeField] private StackBlockSO stackBlockSO;

        [SerializeField] private Transform spawnPointX;
        [SerializeField] private Transform spawnPointZ;

        private DirectionAxis directionAxis = DirectionAxis.A_FORWARD;
        private float stackBlockSpeed;
        private List<StackBlock> stackBlocks = new List<StackBlock>();

        [Header("Events")]
        [SerializeField] private VoidEventSO OnStackBlockPlaced;
        [SerializeField] private VoidEventSO OnGameRestart;

        public enum DirectionAxis
        {
            A_NONE,
            A_FORWARD,
            A_RIGHT
        }

        private void Start()
        {
            stackBlockSpeed = stackBlockSO.SpeedMov;
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

            StackBlock stackBlock = Instantiate(stackBlockSO.StackPrefab, spawnPos, Quaternion.identity, transform).Init(directionAxis, stackBlockSpeed, stackBlockSO.OffsetY, GameManager.Instance.StackCount);
            stackBlockSpeed += stackBlockSO.AccMov;
            directionAxis = (directionAxis == DirectionAxis.A_RIGHT) ? DirectionAxis.A_FORWARD : DirectionAxis.A_RIGHT;
            stackBlocks.Add(stackBlock);

            GameManager.Instance.IncreaseStackCount();
        }

        private void RestartGame()
        {
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
    }
}
