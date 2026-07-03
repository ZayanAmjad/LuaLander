using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
