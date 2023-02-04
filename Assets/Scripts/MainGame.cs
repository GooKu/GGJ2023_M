using System.Collections.Generic;
using UnityEngine;

namespace GGJ23M
{
    public class MainGame : MonoBehaviour
    {
        [SerializeField]
        private GameMapView gameMapView;
        [SerializeField]
        private List<Vector2Int> sizeEachLayer = new();
        [SerializeField]
        private Sprite[] tileSprites;

        private Player player = new();
        private GameMap gameMap = new();

        private void Awake()
        {
            SetUp(sizeEachLayer);
            gameMapView.SetUp(sizeEachLayer);
            var root = new Root(null, new Hex(), 0);
            player.AddRoot(root);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                InputHandle();
            }
        }

        public void SetUp(List<Vector2Int> sizeEachLayer)
        {
            gameMap = new GameMap();

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
                    gameMap.AddTile(new TileData(hex));
                }
            }
        }

        private void InputHandle()
        {
            var worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Hex hex = Hex.PointToHex(worldPosition, gameMapView.GetHexSize());
            var tileData = gameMap.GetTile(hex);

            if (tileData == null || tileData.Type == TileData.TileType.Root)
            {
                return;
            }

            int checkIndex = (hex.row & 1) == 0 ? 0 : 1;

            for (int i = 0; i < 6; i++)
            {
                var offset = Hex.Directions[checkIndex, i];

                var pos = new Hex(tileData.Position.column + offset.x, tileData.Position.row + offset.y);

                var checkTile = gameMap.GetTile(pos);

                if (checkTile == null)
                {
                    continue;
                }

                if (checkTile.Type == TileData.TileType.Root)
                {
                    tileData.UpdateType(TileData.TileType.Root);
                    //TODO:set root
                    return;
                }
            }
        }


    }
}
