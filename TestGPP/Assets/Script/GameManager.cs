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

        int idxPlayerSkin = 0;

        // Start is called before the first frame update
        void Start()
        {
            // Player
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
    }
}
