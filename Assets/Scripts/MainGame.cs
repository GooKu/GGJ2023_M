using System.Collections.Generic;
using UnityEngine;

namespace GGJ23M
{
    public class MainGame : MonoBehaviour
    {
        [SerializeField]
        private GameMapView gameMapView;
        [SerializeField]
        private GameUI gameUI;
        [SerializeField]
        private int startEnergy = 5;
        [SerializeField]
        private List<ScriptableLayerData> layerDatas = new();

        private Player player;
        private GameMap gameMap;

        private Root mainRoot;
        private Hex[] mainEmpty = new Hex[3];

        private int scroe = 0;

        private void Awake()
        {
            gameMap = BuildGameMap(layerDatas);
            gameMapView.SetUp(gameMap, layerDatas);
            startEnergy++;//++for frist root
            player = new(startEnergy);
            player.EnergyChnageEvent += gameUI.SetEnergyAmount;
            gameUI.SetEnergyAmount(startEnergy);
            gameUI.SetScore(scroe);
            SetRoot(new Hex(), null);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !player.IfPlayerDead())
            {
                InputHandle();
            }
        }

        private static GameMap BuildGameMap(List<ScriptableLayerData> layerDatas)
        {
            var gameMap = new GameMap(layerDatas);
            gameMap.Initialize();
            return gameMap;
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

            foreach (var p in mainEmpty)
            {
                if (pos.Equals(p))
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

            if (tileData.Type == TileData.TileType.Water)
            {
                player.AddEnergy(8);
            }

            if (tileData.Type == TileData.TileType.Light)
            {
                gameMap.UnlockNextLayer();
                gameMapView.MoveDown();
            }

            tileData.UpdateType(TileData.TileType.Root);

            Root.Level level = Root.Level.Main;

            if (parent != null)
            {
                //Debug.Log($"{pos}, root:{parent.ReturnHex()}, {parent.BranchLevel}");
                level = parent.BranchLevel;
                scroe++;
                gameUI.SetScore(scroe);
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

                player.RemoveEnergy(1);
            }
            else
            {
                player.RemoveEnergy(2);
            }

            if (parent != null)
            {
                parent.UpdateLevel(Root.Level.Sub);
            }

            player.AddRoot(root);
        }
    }
}
