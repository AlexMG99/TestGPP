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

        private int stackCount = 0;
        private DirectionAxis directionAxis = DirectionAxis.A_FORWARD;
        private float stackBlockSpeed;
        public static StackManager Instance;

        [SerializeField] private VoidEventSO OnStackBlockPlaced;

        public enum DirectionAxis
        {
            A_NONE,
            A_FORWARD,
            A_RIGHT
        }

        void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            stackBlockSpeed = stackBlockSO.SpeedMov;
        }

        private void OnEnable()
        {
            OnStackBlockPlaced.OnEventRaised += SpawnStackBlock;
        }

        private void OnDisable()
        {
            OnStackBlockPlaced.OnEventRaised -= SpawnStackBlock;
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
            spawnPos.y = stackCount * stackBlockSO.OffsetY;

            StackBlock stackBlock = Instantiate(stackBlockSO.StackPrefab, spawnPos, Quaternion.identity, transform).Init(directionAxis, stackBlockSpeed, stackBlockSO.OffsetY, stackCount);
            stackBlockSpeed += stackBlockSO.AccMov;
            directionAxis = (directionAxis == DirectionAxis.A_RIGHT) ? DirectionAxis.A_FORWARD : DirectionAxis.A_RIGHT;

            stackCount++;
        }
    }
}
