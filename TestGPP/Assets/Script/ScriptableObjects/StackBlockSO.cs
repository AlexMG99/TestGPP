using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Stack.Core
{
    [CreateAssetMenu(fileName = "StackBlock", menuName = "ScriptableObjects/StackBlocks", order = 1)]
    public class StackBlockSO : ScriptableObject
    {
        [Header("Stack Block Properties")]
        [SerializeField]
        private StackBlock stackPrefab;
        public StackBlock StackPrefab { get => stackPrefab; set => stackPrefab = value; }

        [SerializeField]
        private float offsetY;
        public float OffsetY { get => offsetY; set => offsetY = value; }

        [SerializeField]
        private float speedMov;
        public float SpeedMov { get => speedMov; set => speedMov = value; }

        [SerializeField]
        private float accMov;
        public float AccMov { get => accMov; set => accMov = value; }

        [SerializeField]
        private GradientSO gradient;
        public GradientSO Gradient { get => gradient; set => gradient = value; }

    }
}
