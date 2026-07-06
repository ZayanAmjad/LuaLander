using Unity.Cinemachine;
using UnityEngine;

public class CinemachineCameraZoom : MonoBehaviour
{

    private const float NORMAL_ORTHOGRAPHIC_SIZE = 10f;
    public static CinemachineCameraZoom Instance { get; private set; }

    private float orthographicSize = 10f;
    [SerializeField] private CinemachineCamera cineCamera;

    private void Update()
    {
        if (cineCamera == null)
        {
            return;
        }

        float zoomSpeed = 2f;
        cineCamera.Lens.OrthographicSize = Mathf.Lerp(cineCamera.Lens.OrthographicSize, orthographicSize, zoomSpeed * Time.deltaTime);
    }

    private void Awake()
    {
        Instance = this;

        if (cineCamera == null)
        {
            cineCamera = Object.FindFirstObjectByType<CinemachineCamera>();
        }
    }



    public void SetOrthographicSize(float size)
    {
        orthographicSize = size;
    }

    public void SetOrthographicSizeToNormal()
    {
        orthographicSize = NORMAL_ORTHOGRAPHIC_SIZE;
    }
}
