namespace GGJ23M
{
    public class TileData
    {
        private readonly Hex hex;

        public enum TileType
        {
            Empty,
            Root,
            Water,
            Obstacle
        }

        public Hex Position => hex;

        public TileType Type { get; private set; }

        public enum RootableType
        {
            Invalid,
            Main,
            Sub,
        }

        public RootableType Rootable { get; private set; }

        public Root Parent { get; private set; }

        public TileData(Hex hex)
        {
            this.hex = hex;
        }

        public TileData(int column, int row)
        {
            hex = new Hex(column, row);
        }

        public void UpdateType(TileType type)
        {
            Type = type; 
            if(type == TileType.Root)
            {
                UpdateRootableType(RootableType.Invalid);
            }
        }

        public void UpdateRootableType(RootableType type)
        {
            Rootable = type;
        }

        public void SetParent(Root parent)
        {
            Parent = parent;
        }

        public bool IsNeighbor(TileData tileData)
        {
            return hex.IsNeighbor(tileData.hex);
        }
    }
}
