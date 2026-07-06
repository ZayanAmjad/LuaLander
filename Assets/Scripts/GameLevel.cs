using UnityEngine;

public class GameLevel : MonoBehaviour
{
    [SerializeField] private int levelNumber;
    [SerializeField] private Transform LanderPosition;
    [SerializeField] private Transform CameraStartTarget;
    [SerializeField] private float zoomOutSize;

    public int LevelNumber => levelNumber;

    public Vector3 GetLanderPosition()
    {
        return LanderPosition.position;
    } 

    public Transform GetCameraStartTarget()
    {
        return CameraStartTarget;
    }

    public float GetZoomOutSize()
    {
        return zoomOutSize;
    }
}
