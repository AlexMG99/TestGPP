using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GradientSO", menuName = "ScriptableObjects/StackBlocksGradient", order = 1)]
public class GradientSO : ScriptableObject
{
    [SerializeField]
    private Gradient gradient;
    public Gradient Gradient { get => gradient; set => gradient = value; }

    [SerializeField]
    private float speed = 0;

    private float currentValue = 0;
    public Color GetColor()
    {
        if (currentValue > 1)
            currentValue = 0;
        else
            currentValue += speed;
        return gradient.Evaluate(currentValue);
    }
}
