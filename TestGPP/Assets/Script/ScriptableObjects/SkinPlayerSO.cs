using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Stack.Core
{
    [CreateAssetMenu(fileName = "SkinPlayer", menuName = "ScriptableObjects/PlayerSkin", order = 1)]
    public class SkinPlayerSO : ScriptableObject
    {
        [Header("Color Properties")]
        [SerializeField]
        private Color colSkin;
        public Color ColSkin { get => colSkin; set => colSkin = value; }

        [SerializeField]
        private Gradient trailColor;
        public Gradient TrailColor { get => trailColor; set => trailColor = value; }

        [SerializeField]
        private bool hasTexture = false;
        public bool HasTexture { get => hasTexture; set => hasTexture = value; }

        [Header("Texture Properties")]
        [SerializeField]
        private Texture2D textSkin;
        public Texture2D TextSkin { get => textSkin; set => textSkin = value; }
        

        [Header("Store Properties")]
        [SerializeField]
        private string nameSkin;
        public string NameSkin { get => nameSkin; set => nameSkin = value; }
        [SerializeField]
        private int priceSkin;
        public int PriceSkin { get => priceSkin; set => priceSkin = value; }
        [SerializeField]
        private Sprite storeSprite;
        public Sprite StoreSprite { get => storeSprite; set => storeSprite = value; }

    }
}
