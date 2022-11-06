using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

namespace Game.Stack.Core
{
    public class UIController : MonoBehaviour
    {
        [Header("Title Elements")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private Button startButton;
        [SerializeField] private TextMeshProUGUI startText;
        [SerializeField] private Button restartButton;
        [SerializeField] private TextMeshProUGUI restartText;

        [SerializeField] private TextMeshProUGUI scoreText;

        [Header("Events")]
        [SerializeField] private VoidEventSO OnStackBlockPlaced;
        [SerializeField] private VoidEventSO OnGameStarted;
        [SerializeField] private VoidEventSO OnGameEnd;
        [SerializeField] private VoidEventSO OnGameRestart;

        private void OnEnable()
        {
            OnStackBlockPlaced.OnEventRaised += ModifyScore;
            OnGameEnd.OnEventRaised += EndGame;
        }

        private void OnDisable()
        {
            OnStackBlockPlaced.OnEventRaised -= ModifyScore;
            OnGameEnd.OnEventRaised -= EndGame;
        }

        public void StartGame()
        {
            float time = 0.5f;
            titleText.DOFade(0f, time);
            startText.DOFade(0f, time);
            startButton.enabled = false;

            OnGameStarted.RaisedEvent();
            OnStackBlockPlaced.RaisedEvent();
        }
        public void EndGame()
        {
            float time = 0.5f;
            restartText.DOFade(1f, time);
            restartButton.enabled = true;
        }

        public void RestartGame()
        {
            float time = 0.5f;
            titleText.DOFade(1f, time);
            startText.DOFade(1f, time);
            scoreText.DOFade(0f, 0.01f);
            restartText.DOFade(0f, time);
            restartButton.enabled = false;
            startButton.enabled = true;

            OnGameRestart.RaisedEvent();
        }

        void ModifyScore()
        {
            scoreText.text = GameManager.Instance.StackCount.ToString();
            if(GameManager.Instance.StackCount != 0)
                scoreText.DOFade(1f, 0.5f);
        }
    }
}
