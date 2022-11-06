using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Stack.Core
{
    public class PlayerSkin : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private TrailRenderer trail;

        public void ChangeSkin(SkinPlayerSO playerSkinSO)
        {
            meshRenderer.material.color = playerSkinSO.ColSkin;
            trail.colorGradient = playerSkinSO.TrailColor;
        }
    }
}
