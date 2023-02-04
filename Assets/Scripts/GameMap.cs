using System.Collections.Generic;

namespace GGJ23M
{
    public class GameMap
    {
        private readonly Dictionary<Hex, TileData> mapData = new();

        public void AddTile(TileData tileData)
        {
            mapData.Add(tileData.Position, tileData);
        }

        public TileData GetTile(Hex hex)
        {
            bool success = mapData.TryGetValue(hex, out TileData tileData);
            if (success)
            {
                return tileData;
            }

            return null;
        }
    }
}
