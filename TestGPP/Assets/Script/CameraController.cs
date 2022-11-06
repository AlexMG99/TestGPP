using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            StartCoroutine(MoveCameraUpRoutine());
        }

        IEnumerator MoveCameraUpRoutine()
        {
            Vector3 reachPos = transform.position + Vector3.up * stackBlockSO.OffsetY;
            while (Vector3.Distance(reachPos, transform.position) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, reachPos, 2f * Time.deltaTime);
                yield return null;
            }
        }

        void SetCameraInitialPos()
        {
            transform.position = initialPos;
        }
    }
}
