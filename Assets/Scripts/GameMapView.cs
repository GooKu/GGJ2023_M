using System.Collections.Generic;
using UnityEngine;

namespace GGJ23M
{
    public partial class GameMapView : MonoBehaviour
    {
        [SerializeField]
        private Camera mainCamera;
        [SerializeField]
        private GameObject tilePrefab;
        [SerializeField]
        private float hexSize = 1f;

        private List<Vector2Int> sizeEachLayer = new();

        private Dictionary<Hex, TileView> tileViews = new();

        private void Start()
        {
        }

        public void SetUp(List<Vector2Int> sizeEachLayer)
        {
            this.sizeEachLayer = sizeEachLayer;

            int maxWidth = 0;
            int totalHeight = 0;
            for (var i = 0; i < sizeEachLayer.Count; i++)
            {
                if (maxWidth < sizeEachLayer[i].x)
                {
                    maxWidth = sizeEachLayer[i].x;
                }

                totalHeight += sizeEachLayer[i].y;
            }

            int offsetY = 0;
            for (var i = 0; i < sizeEachLayer.Count; i++)
            {
                BuildLayer(sizeEachLayer[i].x, sizeEachLayer[i].y, offsetY);
                offsetY += sizeEachLayer[i].y;
            }
        }

        private void BuildLayer(int width, int height, int offsetY)
        {
            int extend = (width - 1) / 2;
            for (var i = -extend; i <= extend; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    var hex = new Hex(i, j + offsetY);

                    Vector2 point = hex.ToPoint(hexSize);
                    GameObject clone = Instantiate(tilePrefab, point, Quaternion.identity, transform);
                    clone.name = $"hex_{i}_{j}";
                    tileViews.Add(hex, clone.GetComponent<TileView>());
                }
            }
        }

        private void Update()
        {
            UpdateCamera();
            if (Input.GetKeyDown(KeyCode.Z))
            {
                MoveUp();
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                MoveDown();
            }
        }

        private float GetLayerWidth(int indexOfLayer)
        {
            int hexWidth = sizeEachLayer[indexOfLayer].x;
            return hexWidth * hexSize * 1.5f + hexSize * 0.5f;
        }

        private Vector2 GetLayerCenter(int indexOfLayer)
        {
            int offsetY = 0;
            for (var i = 0; i < sizeEachLayer.Count; i++)
            {
                if (i != indexOfLayer)
                {
                    offsetY += sizeEachLayer[i].y;
                    continue;
                }

                offsetY += sizeEachLayer[i].y / 2;
                var hex = new Hex(0, offsetY);
                return hex.ToPoint(hexSize);
            }

            {
                var hex = new Hex(0, offsetY);
                return hex.ToPoint(hexSize);
            }
        }

        public float GetHexSize() { return hexSize; }
    }
}
