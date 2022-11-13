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
    private float step = 0;

    private float currentValue = 0;
    public Color GetColor()
    {
        if (currentValue > 1)
            currentValue = 0;
        else
            currentValue += step;
        return gradient.Evaluate(currentValue);
    }

    public Color GetRandomColor()
    {
        float randValue = Random.Range(0f, 1f);
        return gradient.Evaluate(randValue);
    }
}
