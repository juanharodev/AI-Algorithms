using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

public class Fuzzy : MonoBehaviour
{
    public AnimationCurve curve;

    public float GetValue(float value)
    {
        return curve.Evaluate(value);
    }
}
