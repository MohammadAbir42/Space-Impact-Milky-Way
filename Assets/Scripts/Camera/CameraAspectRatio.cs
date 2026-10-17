
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraAspectRatio : MonoBehaviour
{
    [SerializeField] private float targetWidth = 16f;
    [SerializeField] private float targetHeight = 9f;

    private Camera gameCamera;
    private int lastWidth;
    private int lastHeight;

    private void Awake()
    {
        gameCamera = GetComponent<Camera>();
        UpdateViewport();
    }

    private void Update()
    {
        if (Screen.width != lastWidth ||
            Screen.height != lastHeight)
        {
            UpdateViewport();
        }
    }

    private void UpdateViewport()
    {
        lastWidth = Screen.width;
        lastHeight = Screen.height;

        if (lastWidth <= 0 || lastHeight <= 0 ||
            targetWidth <= 0 || targetHeight <= 0)
            return;

        float targetAspect = targetWidth / targetHeight;
        float screenAspect = (float)lastWidth / lastHeight;

        Rect viewport = new Rect(0f, 0f, 1f, 1f);

        if (screenAspect > targetAspect)
        {
            // Screen is wider: add side bars.
            viewport.width = targetAspect / screenAspect;
            viewport.x = (1f - viewport.width) / 2f;
        }
        else if (screenAspect < targetAspect)
        {
            // Screen is taller: add top/bottom bars.
            viewport.height = screenAspect / targetAspect;
            viewport.y = (1f - viewport.height) / 2f;
        }

        gameCamera.rect = viewport;
    }
}
