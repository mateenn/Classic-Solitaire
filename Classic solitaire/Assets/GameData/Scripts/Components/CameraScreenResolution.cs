using UnityEngine;

namespace TheSyedMateen.ClassicSolitaire
{
    public class CameraScreenResolution : MonoBehaviour
    {
        private Camera _mainCamera;

        private void OnEnable()
        {
            _mainCamera = Camera.main;
            SetCameraOrthoSize();
        }

        private void SetCameraOrthoSize()
        {
            float aspectRatio = _mainCamera.aspect;

            if (aspectRatio >= 0.5f)
            {
                SetOrthoSizeForWide(aspectRatio);
            }
            else
            {
                SetOrthoSizeForNarrow(aspectRatio);
            }
        }

        private void SetOrthoSizeForWide(float aspectRatio)
        {
            if (aspectRatio >= 0.56f) _mainCamera.orthographicSize = 9.66f;
            else if (aspectRatio >= 0.55f) _mainCamera.orthographicSize = 9.6f;
            else if (aspectRatio >= 0.54f) _mainCamera.orthographicSize = 10.0f;
            else if (aspectRatio >= 0.53f) _mainCamera.orthographicSize = 10.26f;
            else if (aspectRatio >= 0.52f) _mainCamera.orthographicSize = 10.38f;
            else if (aspectRatio >= 0.51f) _mainCamera.orthographicSize = 10.65f;
            else _mainCamera.orthographicSize = 10.8f; // Default
        }

        private void SetOrthoSizeForNarrow(float aspectRatio)
        {
            if (aspectRatio <= 0.45f) _mainCamera.orthographicSize = 11.9f;
            else if (aspectRatio <= 0.46f) _mainCamera.orthographicSize = 11.68f;
            else if (aspectRatio <= 0.47f) _mainCamera.orthographicSize = 11.2f;
            else if (aspectRatio <= 0.48f) _mainCamera.orthographicSize = 11f;
            else _mainCamera.orthographicSize = 10.9f; // Default
        }
    }
}