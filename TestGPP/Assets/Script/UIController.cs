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

        [SerializeField] private Button storeButton;

        [SerializeField] private TextMeshProUGUI coinsText;

        [Header("Player Store")]
        [SerializeField] private CanvasGroup playerStore;
        [SerializeField] private GameObject playerStoreContainer;
        [SerializeField] private StoreElement storeElementPrefab;
        private List<StoreElement> skinPlayerElements = new List<StoreElement>();

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

            CreatePlayerSkinStore();
        }

        void CreatePlayerSkinStore()
        {
            for(int i = 0; i < GameManager.Instance.PlayerSkins.Count; i++)
            {
                SkinPlayerSO playerSkin = GameManager.Instance.PlayerSkins[i];
                StoreElement newSkin = Instantiate(storeElementPrefab, playerStoreContainer.transform).Init(playerSkin.NameSkin, playerSkin.StoreSprite, playerSkin.ColSkin, playerSkin.PriceSkin, i, this);
                skinPlayerElements.Add(newSkin);
            }
        }

        public void StartGame()
        {
            float time = 0.5f;
            menuGame.DOFade(0f, 0.1f);
            menuGame.interactable = false;
            menuGame.blocksRaycasts = false;
            playGame.DOFade(1f, time);

            OnGameStarted.RaisedEvent();
            OnStackBlockPlaced.RaisedEvent();
        }
        public void EndGame()
        {
            float time = 0.5f;
            endGame.interactable = true;
            endGame.blocksRaycasts = true;
            endGame.DOFade(1f, time);

            playGame.DOFade(0f, 0.1f);

            CheckHighScore();
        }

        public void RestartGame()
        {
            float time = 0.5f;
            endGame.DOFade(0f, 0.1f);
            endGame.interactable = false;
            endGame.blocksRaycasts = false;

            menuGame.DOFade(1f, time);
            menuGame.interactable = true;
            menuGame.blocksRaycasts = true;

            OnGameRestart.RaisedEvent();
        }

        void ModifyScore()
        {
            scoreText.text = GameManager.Instance.StackCount.ToString();
            coinsText.text = GameManager.Instance.Coins.ToString();
            coinsText.transform.DOPunchScale(Vector3.one * 0.05f, 0.25f, 2);

            scoreTextEndScreen.text = GameManager.Instance.StackCount.ToString();

            if (GameManager.Instance.StackCount != 0)
                scoreText.DOFade(1f, 0.5f);
        }

        public void LoseCoins()
        {
            coinsText.text = GameManager.Instance.Coins.ToString();
            coinsText.transform.DOPunchScale(Vector3.one * -0.05f, 0.25f, 2);
        }

        void CheckHighScore()
        {
            if (GameManager.Instance.StackCount > PlayerPrefs.GetInt("HighScore", 0))
                PlayerPrefs.SetInt("HighScore", GameManager.Instance.StackCount);

            highScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        }

        public void OpenPlayerStore()
        {
            playerStore.transform.DOMove(transform.position, 1f);
            playerStore.DOFade(1f, 0.25f);
            playerStore.interactable = true;
            playerStore.blocksRaycasts = true;
            storeButton.gameObject.SetActive(false);
        }

        public void ClosePlayerStore()
        {
            playerStore.transform.DOMoveY(transform.position.y - Screen.height, 1f);
            playerStore.DOFade(1f, 0.25f);
            playerStore.interactable = false;
            playerStore.blocksRaycasts = false;
            storeButton.gameObject.SetActive(true);
        }
    }
}
