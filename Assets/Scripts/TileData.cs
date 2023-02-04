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
        }

        public Hex Position => hex;

        public TileType Type { get; private set; }

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
        }
    }
}
