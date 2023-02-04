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
        [SerializeField]
        private Sprite[] tileSprites;

        private List<Vector2Int> sizeEachLayer = new();

        private Dictionary<Hex, TileView> tileViews = new();

        public void SetUp(GameMap gameMap, List<Vector2Int> sizeEachLayer)
        {
            this.sizeEachLayer = sizeEachLayer;
            foreach (TileData tileData in gameMap.AllTileData)
            {
                var hex = tileData.Position;
                Vector2 point = hex.ToPoint(hexSize);
                GameObject clone = Instantiate(tilePrefab, point, Quaternion.identity, transform);
                clone.name = $"hex_{hex.column}_{hex.row}";
                tileViews.Add(hex, clone.GetComponent<TileView>());
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

        public void UpdateTile(Hex pos, TileData.TileType tileType)
        {
            if(!tileViews.TryGetValue(pos, out var tile)) { return; }

            Sprite sprite = null;

            switch (tileType)
            {
                case TileData.TileType.Root:
                    sprite = tileSprites[0];
                    break;
            }

            tile.UpdateSprite(sprite);
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
