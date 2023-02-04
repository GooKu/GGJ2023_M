using UnityEngine;

namespace GGJ23M
{
    public partial class GameMapView : MonoBehaviour
    {
        private int currentLayer;
        private Vector3 moveVelocity;
        private float sizeVelocity;

        private bool scrolled;
        private float scrolledTimer;

        public void UpdateCamera()
        {
            Vector2 center = GetLayerCenter(currentLayer);
            Vector3 position = Vector3.SmoothDamp(mainCamera.transform.position, center, ref moveVelocity, 0.8f);
            position.z = mainCamera.transform.position.z;
            mainCamera.transform.position = position;

            float targetSize = GetCameraTargetSize();
            float size = Mathf.SmoothDamp(mainCamera.orthographicSize, targetSize, ref sizeVelocity, 0.8f);
            mainCamera.orthographicSize = size;

            UpdateScroll();
        }

        private float GetCameraTargetSize()
        {
            float width = GetLayerWidth(currentLayer);
            float targetSizeForWidth = width * 0.5f * Screen.height / Screen.width;

            float height = GetLayerHeight(currentLayer);
            float targetSizeForHeight = height * 0.5f;

            return Mathf.Max(targetSizeForWidth, targetSizeForHeight);
        }

        private void UpdateScroll()
        {
            if (!scrolled && Input.mouseScrollDelta.y > 0f)
            {
                scrolled = true;
                MoveUp();
            }
            if (!scrolled && Input.mouseScrollDelta.y < 0f)
            {
                scrolled = true;
                MoveDown();
            }

            if (scrolled)
            {
                scrolledTimer += Time.deltaTime;
                if (scrolledTimer > 0.5f)
                {
                    scrolled = false;
                    scrolledTimer = 0f;
                }
            }
        }

        public void MoveUp()
        {
            currentLayer--;
            if (currentLayer < 0)
            {
                currentLayer = 0;
            }
        }

        public void MoveDown()
        {
            currentLayer++;
            if (currentLayer > gameMap.MaxUnlockedLayer)
            {
                currentLayer = gameMap.MaxUnlockedLayer;
            }
        }
    }
}
