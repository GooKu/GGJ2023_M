using UnityEngine;

namespace GGJ23M
{
    public partial class GameMapView : MonoBehaviour
    {
        private int currentLayer;
        private Vector3 moveVelocity;
        private float sizeVelocity;

        public void UpdateCamera()
        {
            Vector2 center = GetLayerCenter(currentLayer);
            Vector3 position = Vector3.SmoothDamp(mainCamera.transform.position, center, ref moveVelocity, 0.25f);
            position.z = mainCamera.transform.position.z;
            mainCamera.transform.position = position;

            float width = GetLayerWidth(currentLayer);
            float targetSize = width * 0.5f * Screen.height / Screen.width;
            float size = Mathf.SmoothDamp(mainCamera.orthographicSize, targetSize, ref sizeVelocity, 0.25f);
            mainCamera.orthographicSize = size;
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
