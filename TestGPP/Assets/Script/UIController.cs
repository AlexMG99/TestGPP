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
        [SerializeField] private CanvasGroup menuGame;
        [SerializeField] private CanvasGroup playGame;
        [SerializeField] private CanvasGroup endGame;

        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI scoreTextEndScreen;
        [SerializeField] private TextMeshProUGUI highScoreText;

        [SerializeField] private TextMeshProUGUI coinsText;

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

        private void Start()
        {
            coinsText.text = GameManager.Instance.Coins.ToString();
        }

        public void StartGame()
        {
            float time = 0.5f;
            menuGame.DOFade(0f, 0.1f);
            menuGame.interactable = false;
            playGame.DOFade(1f, time);

            OnGameStarted.RaisedEvent();
            OnStackBlockPlaced.RaisedEvent();
        }
        public void EndGame()
        {
            float time = 0.5f;
            endGame.interactable = true;
            endGame.DOFade(1f, time);

            playGame.DOFade(0f, 0.1f);

            CheckHighScore();
        }

        public void RestartGame()
        {
            float time = 0.5f;
            endGame.DOFade(0f, 0.1f);
            endGame.interactable = false;

            menuGame.DOFade(1f, time);
            menuGame.interactable = true;

            OnGameRestart.RaisedEvent();
        }

        void ModifyScore()
        {
            scoreText.text = GameManager.Instance.StackCount.ToString();
            coinsText.text = GameManager.Instance.Coins.ToString();

            scoreTextEndScreen.text = GameManager.Instance.StackCount.ToString();

            if (GameManager.Instance.StackCount != 0)
                scoreText.DOFade(1f, 0.5f);
        }

        void CheckHighScore()
        {
            if (GameManager.Instance.StackCount > PlayerPrefs.GetInt("HighScore", 0))
                PlayerPrefs.SetInt("HighScore", GameManager.Instance.StackCount);

            highScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        }
    }
}
