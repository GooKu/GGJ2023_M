using System.Collections.Generic;
using UnityEngine;

namespace GGJ23M
{
    public class MainGame : MonoBehaviour
    {
        [SerializeField]
        private GameMapView gameMapView;
        [SerializeField]
        private List<ScriptableLayerData> layerDatas = new();

        private Player player = new();
        private GameMap gameMap;

        private Root mainRoot;
        private Hex[] mainEmpty = new Hex[3];

        private void Awake()
        {
            gameMap = BuildGameMap(layerDatas);
            gameMapView.SetUp(gameMap, layerDatas);
            SetRoot(new Hex(), null);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                InputHandle();
            }
        }

        private static GameMap BuildGameMap(List<ScriptableLayerData> layerDatas)
        {
            var gameMap = new GameMap();

            int maxWidth = 0;
            int totalHeight = 0;
            for (var i = 0; i < layerDatas.Count; i++)
            {
                if (maxWidth < layerDatas[i].size.x)
                {
                    maxWidth = layerDatas[i].size.x;
                }

                totalHeight += layerDatas[i].size.y;
            }

            int offsetY = 0;
            for (var i = 0; i < layerDatas.Count; i++)
            {
                BuildLayer(gameMap, layerDatas[i], offsetY);
                offsetY += layerDatas[i].size.y;
            }

            return gameMap;
        }

        private static void BuildLayer(GameMap gameMap, ScriptableLayerData layerData, int offsetY)
        {
            int width = layerData.size.x;
            int height = layerData.size.y;

            // Build tiles
            int extend = (width - 1) / 2;
            for (var i = -extend; i <= extend; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    var hex = new Hex(i, j + offsetY);
                    gameMap.AddTile(new TileData(hex));
                }
            }

            // Setup tile types
            for (var i = 0; i < layerData.tiles.Count; i++)
            {
                var hex = new Hex(layerData.tiles[i].position.x, layerData.tiles[i].position.y + offsetY);
                TileData tileData = gameMap.GetTile(hex);
                tileData.UpdateType((TileData.TileType)layerData.tiles[i].tileId);
            }
        }

        private void InputHandle()
        {
            var worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Hex pos = Hex.PointToHex(worldPosition, gameMapView.GetHexSize());
            var tileData = gameMap.GetTile(pos);

            if (tileData == null)
            {
                return;
            }

            switch (tileData.Type)
            {
                case TileData.TileType.Obstacle:
                case TileData.TileType.Root:
                    return;
            }

            foreach(var p in mainEmpty)
            {
                if(pos.Equals(p))
                {
                    SetRoot(pos, mainRoot);
                    return;
                }
            }

            foreach (var r in player.ReturnRoots())
            {
//                Debug.Log($"{r.ReturnHex()}, {r.ReturnHex().IsNeighbor(pos)}");
                if (r.ReturnHex().IsNeighbor(pos) && r != mainRoot)
                {
                    SetRoot(pos, r);
                    return;
                }
            }
        }

        private void SetRoot(Hex pos, Root parent)
        {
            var tileData = gameMap.GetTile(pos);

            if(tileData.Type == TileData.TileType.Water)
            {
                player.AddEnergy(8);
            }

            tileData.UpdateType(TileData.TileType.Root);

            Root.Level level = Root.Level.Main;

            if (parent != null)
            {
                //Debug.Log($"{pos}, root:{parent.ReturnHex()}, {parent.BranchLevel}");
                level = parent.BranchLevel;
            }

            gameMapView.UpdateTile(pos, TileData.TileType.Root, level == Root.Level.Main);

            var root = new Root(parent, pos, level);

            if (level == Root.Level.Main)
            {
                mainRoot = root;

                var checkIndex = (pos.column & 1) == 0 ? 0 : 1;

                var checkVects = new Vector2Int[] { Hex.Directions[checkIndex, 0],
                    Hex.Directions[checkIndex, 4],
                    Hex.Directions[checkIndex, 5]
                };

                for (int i = 0; i < checkVects.Length; i++)
                {
                    var offset = checkVects[i];
                    var checkPos = new Hex(tileData.Position.column + offset.x, tileData.Position.row + offset.y);
                    mainEmpty[i] = checkPos;
                }

                player.AddEnergy(-1);
            }
            else
            {
                player.AddEnergy(-2);
            }

            if (parent != null)
            {
                parent.UpdateLevel(Root.Level.Sub);
            }

            player.AddRoot(root);
        }
    }
}
