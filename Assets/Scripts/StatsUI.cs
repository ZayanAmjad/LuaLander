using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] private GameObject UpArrow;
    [SerializeField] private GameObject DownArrow;
    [SerializeField] private GameObject LeftArrow;
    [SerializeField] private GameObject RightArrow;
    [SerializeField] private Image fuelBar;

    private void Update()
    {
        if (GameManager.Instance != null)
        {
            UpArrow.SetActive(Lander.Instance.GetSpeedY() > 0);
            DownArrow.SetActive(Lander.Instance.GetSpeedY() < 0);
            LeftArrow.SetActive(Lander.Instance.GetSpeedX() < 0);
            RightArrow.SetActive(Lander.Instance.GetSpeedX() > 0);
    
            fuelBar.fillAmount = Lander.Instance.GetFuel();

            statsText.text = 
                GameManager.Instance.GetScore().ToString() + '\n' +
                Mathf.Round(GameManager.Instance.GetTimeElapsed()).ToString() + "s" + '\n' +
                Mathf.Abs(Mathf.Round(Lander.Instance.GetSpeedX())).ToString() + '\n' +
                Mathf.Abs(Mathf.Round(Lander.Instance.GetSpeedY())).ToString() + '\n';
        }
    }
}
