using UnityEngine;
using TMPro;

public class LandingPadVisual : MonoBehaviour
{
    [SerializeField] private TextMeshPro scoreMultiplierText;

    void Awake()
    {
        LandingPad pad = GetComponent<LandingPad>();

        scoreMultiplierText.text = "x" + pad.GetScoreMultiplier().ToString();
    }

}
