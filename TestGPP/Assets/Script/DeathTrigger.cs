using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Stack.Core
{
    public class DeathTrigger : MonoBehaviour
    {
        [SerializeField] private StackBlockSO stackBlockSO;

        [Header("Events")]
        [SerializeField] private VoidEventSO OnStackBlockPlaced;
        [SerializeField] private VoidEventSO OnGameRestart;

        private Vector3 initialPos;

        void MoveTriggerUp()
        {
            transform.DOMoveY(transform.position.y + stackBlockSO.OffsetY, 0.5f);
        }

        void SetTriggerInitialPos()
        {
            transform.DOMove(initialPos, 0.5f);
        }
    }
}
