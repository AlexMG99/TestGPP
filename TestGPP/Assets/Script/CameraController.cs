using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Game.Stack.Core
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private StackBlockSO stackBlockSO;

        [Header("Events")]
        [SerializeField] private VoidEventSO OnStackBlockPlaced;
        [SerializeField] private VoidEventSO OnGameRestart;

        private Vector3 initialPos;

        private void Start()
        {
            initialPos = transform.position;
        }

        private void OnEnable()
        {
            OnStackBlockPlaced.OnEventRaised += MoveCameraUp;
            OnGameRestart.OnEventRaised += SetCameraInitialPos;
        }

        private void OnDisable()
        {
            OnStackBlockPlaced.OnEventRaised -= MoveCameraUp;
            OnGameRestart.OnEventRaised -= SetCameraInitialPos;
        }

        void MoveCameraUp()
        {
            transform.DOMoveY(transform.position.y + stackBlockSO.OffsetY, 0.5f);
        }

        void SetCameraInitialPos()
        {
            transform.DOMove(initialPos, 0.5f);
        }
    }
}
