using System.Collections.Generic;
using UnityEngine;

namespace GGJ23M
{
    public class GameMap : MonoBehaviour
    {
        [SerializeField]
        private Camera mainCamera;
        [SerializeField]
        private GameObject tilePrefab;
        [SerializeField]
        private float hexSize = 1f;
        [SerializeField]
        private List<Vector2Int> sizeEachLayer = new();

        private void Start()
        {
            SetUp();
        }

        public void SetUp()
        {
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
                }
            }
        }
    }
}
