using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Stack.Core
{
    public class GameManager : MonoBehaviour
    {
        [Header("Player Settings")]
        [SerializeField] private GameObject playerPrefab;
        public GameObject PlayerInstance => playerInstance;
        private GameObject playerInstance;
        [SerializeField] private Transform playerSpawn;
        public List<SkinPlayerSO> PlayerSkins => playerSkins;
        [SerializeField] List<SkinPlayerSO> playerSkins = new List<SkinPlayerSO>();

        public StackManager StackManagerInstace => stackManagerInstance;
        [SerializeField] private StackManager stackManagerInstance;
        public StackBlockSO StackBlockSO => stackBlockSO;
        [SerializeField] private StackBlockSO stackBlockSO;

        [Header("Events")]
        [SerializeField] private VoidEventSO OnStartGame;
        [SerializeField] private VoidEventSO OnGameEnd;
        [SerializeField] private VoidEventSO OnGameRestart;

        public int StackCount => stackCount;
        private int stackCount = 0;

        public int Coins => coins;
        private int coins = 0;

        int idxPlayerSkin = 0;

        public static GameManager Instance;

        private GameState gameState = GameState.GS_START;

        public enum GameState
        {
            GS_RESTART,
            GS_START,
            GS_PLAY,
            GS_END
        };

        private void OnEnable()
        {
            OnStartGame.OnEventRaised += StartGame;
            OnGameEnd.OnEventRaised += EndGame;
            OnGameRestart.OnEventRaised += RestartGame;
        }

        private void OnDisable()
        {
            OnStartGame.OnEventRaised -= StartGame;
            OnGameEnd.OnEventRaised -= EndGame;
            OnGameRestart.OnEventRaised -= RestartGame;
        }

        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else
                Destroy(gameObject);

            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 60;

            coins = PlayerPrefs.GetInt("Coins", 0);
        }

        private void OnApplicationQuit()
        {
            PlayerPrefs.SetInt("Coins", coins);
        }

        void CreatePlayer()
        {
            if (playerInstance)
                Destroy(playerInstance.gameObject);

            playerInstance = Instantiate(playerPrefab, playerSpawn.position, playerSpawn.rotation);
            idxPlayerSkin = PlayerPrefs.GetInt("idxPlayerSkin", 0);
            playerInstance.GetComponent<PlayerSkin>().ChangeSkin(playerSkins[idxPlayerSkin]);
        }

        public void SetPlayerSkin(int idx)
        {
            idxPlayerSkin = idx;
            PlayerPrefs.SetInt("idxPlayerSkin", idxPlayerSkin);

            if (playerInstance)
                Destroy(playerInstance.gameObject);
            playerInstance = Instantiate(playerPrefab, playerSpawn.position, playerSpawn.rotation);
            playerInstance.GetComponent<PlayerSkin>().ChangeSkin(playerSkins[idxPlayerSkin]);
        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                idxPlayerSkin = 0;
                playerInstance.GetComponent<PlayerSkin>().ChangeSkin(playerSkins[idxPlayerSkin]);
                PlayerPrefs.SetInt("idxPlayerSkin", idxPlayerSkin);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                idxPlayerSkin = 1;
                playerInstance.GetComponent<PlayerSkin>().ChangeSkin(playerSkins[idxPlayerSkin]);
                PlayerPrefs.SetInt("idxPlayerSkin", idxPlayerSkin);
            }
#endif
        }

        public void StartGame()
        {
            stackCount = 0;
            CreatePlayer();
            gameState = GameState.GS_PLAY;
        }

        public void EndGame()
        {
            gameState = GameState.GS_END;
            PlayerPrefs.SetInt("Coins", coins);

            Audio.AudioManager.Instance.BreakIncrementPitch("SFX_Perfect");
        }

        public void RestartGame()
        {
            gameState = GameState.GS_RESTART;
            
        }

        public void IncreaseStackCount()
        {
            stackCount++;
        }

        public void AddCoin()
        {
            coins++;
        }

        public void ReduceCoin(int value)
        {
            if(coins - value >= 0)
                coins -= value;
        }

        public bool CheckGameState(GameState _gameState)
        {
            return (gameState == _gameState);
        }
    }
}
