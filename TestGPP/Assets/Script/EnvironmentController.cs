using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Stack.Core
{
    [RequireComponent(typeof(StackManager))]
    public class EnvironmentController : MonoBehaviour
    {
        StackManager stackManager;

        Material skyboxMat;

        private void Awake()
        {
            stackManager = GetComponent<StackManager>();

            skyboxMat = RenderSettings.skybox;
        }

        public IEnumerator ChangeBackground()
        {
            float time = 1.5f;
            float startTime = Time.time;

            while (time > Time.time - startTime)
            {
                skyboxMat.SetColor("_SkyGradientTop", Color.Lerp(skyboxMat.GetColor("_SkyGradientTop"), stackManager.GetColorSlabFromList(5), ((Time.time - startTime) / time)));
                skyboxMat.SetColor("_SkyGradientBottom", Color.Lerp(skyboxMat.GetColor("_SkyGradientBottom"), stackManager.GetColorSlabFromList(5), ((Time.time - startTime) / time)));

                skyboxMat.SetColor("_HorizonLineColor", Color.Lerp(skyboxMat.GetColor("_HorizonLineColor"), stackManager.GetColorSlabFromList(1), ((Time.time - startTime) / time)));
                yield return null;
            }

            /*float time = 1f;
            float startTime = Time.deltaTime;

            float hCol;
            float sCol;
            float vCol;
            Color.RGBToHSV(stackManager.GetColorSlabFromList(5), out hCol, out sCol, out vCol);
            Color bottomColor = Color.HSVToRGB(hCol, sCol - 0.15f, vCol);

            Color.RGBToHSV(stackManager.GetColorSlabFromList(1), out hCol, out sCol, out vCol);
            Color topColor = Color.HSVToRGB(hCol, sCol - 0.15f, vCol);

            while (time > Time.deltaTime - startTime)
            {
                skyboxMat.SetColor("_SkyGradientTop", Color.Lerp(skyboxMat.GetColor("_SkyGradientTop"), bottomColor, ((Time.deltaTime - startTime) / time)));
                skyboxMat.SetColor("_SkyGradientBottom", Color.Lerp(skyboxMat.GetColor("_SkyGradientBottom"), bottomColor, ((Time.deltaTime - startTime) / time)));
                skyboxMat.SetColor("_HorizonLineColor", Color.Lerp(skyboxMat.GetColor("_SkyGradientTop"), topColor, ((Time.deltaTime - startTime) / time)));

                yield return null;
            }*/
        }
    }
}
