using UnityEngine;
using Bhaptics.SDK2;
using System.Collections;

public class HapticTest : MonoBehaviour
{

    public HapticTest instance;

    private void Awake() {
        instance = this;
    }

    public void AddDouleur(float intensity)
    {
        BhapticsLibrary.Play("douleur", 0, intensity);
    }
}

