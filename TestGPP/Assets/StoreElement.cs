using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Game.Stack.Core
{
    public class StoreElement : MonoBehaviour
    {
        [Header("Bought Button")]
        [SerializeField] private GameObject boughtButton;
        [SerializeField] private TextMeshProUGUI nameElement;
        [SerializeField] private Image textureElement;

        [Header("Not Bought Button")]
        [SerializeField] private GameObject notBoughtButton;
        [SerializeField] private TextMeshProUGUI nameElementNB;
        [SerializeField] private TextMeshProUGUI priceText;

        private int elementPrice;
        private string elementName;

        int isBought = -1;
        int idxSkin = 0;

        private UIController UIController;

        // Start is called before the first frame update
        public StoreElement Init(string _elementName, Sprite imgButton, Color _colElement, int _elementPrice, int _idxSkin, UIController _UIController)
        {
            elementPrice = _elementPrice;
            elementName = _elementName;
            idxSkin = _idxSkin;
            UIController = _UIController;

            nameElement.text = nameElementNB.text = elementName;
            textureElement.sprite = imgButton;
            textureElement.color = _colElement;
            priceText.text = elementPrice.ToString();

            if (idxSkin != 0)
                isBought = PlayerPrefs.GetInt($"Skin_{elementName}", -1);
            else
                isBought = 1;

            if (isBought == 1)
            {
                boughtButton.SetActive(true);
                notBoughtButton.SetActive(false);
            }
            else
            {
                boughtButton.SetActive(false);
                notBoughtButton.SetActive(true);
            }

            return this;
        }

        public void ClickButton()
        {
            if (isBought == -1)
                BuyButton();
            else
                ChangePlayerSkin();
        }

        void BuyButton()
        {
            if (GameManager.Instance.Coins > elementPrice)
            {
                isBought = 1;
                PlayerPrefs.SetInt($"Skin_{elementName}", isBought);

                boughtButton.SetActive(true);
                notBoughtButton.SetActive(false);
                ChangePlayerSkin();
                GameManager.Instance.ReduceCoin(elementPrice);

                UIController.LoseCoins();
            }
        }

        void ChangePlayerSkin()
        {
            GameManager.Instance.SetPlayerSkin(idxSkin);
        }
    }
}
