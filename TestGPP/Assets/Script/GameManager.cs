using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Stack.Core
{
    public class GameManager : MonoBehaviour
    {
        [Header("Player Settings")]
        [SerializeField] private GameObject playerPrefab;
        private GameObject playerInstance;
        [SerializeField] private Transform playerSpawn;
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
        }

        void CreatePlayer()
        {
            if (playerInstance)
                Destroy(playerInstance.gameObject);

            playerInstance = Instantiate(playerPrefab, playerSpawn.position, playerSpawn.rotation);
            idxPlayerSkin = PlayerPrefs.GetInt("idxPlayerSkin", 0);
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
        }

        public void RestartGame()
        {
            gameState = GameState.GS_RESTART;
            
        }

        public void IncreaseStackCount()
        {
            stackCount++;
        }

        public bool CheckGameState(GameState _gameState)
        {
            return (gameState == _gameState);
        }
    }
}
