using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ23M
{
    public partial class GameMapView : MonoBehaviour
    {
        private void InputHandle()
        {
            var worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Hex hex = Hex.PointToHex(worldPosition, hexSize);
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

                if(checkTile == null)
                {
                    continue;
                }

                if(checkTile.Type == TileData.TileType.Root)
                {
                    tileData.UpdateType(TileData.TileType.Root);
                    //TODO:set root
                    return;
                }
            }
        }
    }
}