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
            float hCol;
            float sCol;
            float vCol;
            Color.RGBToHSV(playerSkinSO.ColSkin, out hCol, out sCol, out vCol);
            Color emissionColor = Color.HSVToRGB(hCol, sCol, vCol - 0.15f);
            meshRenderer.material.SetColor("_EmissionColor", emissionColor);
            meshRenderer.material.EnableKeyword("_EMISSION");

            trail.colorGradient = playerSkinSO.TrailColor;
        }
    }
}
