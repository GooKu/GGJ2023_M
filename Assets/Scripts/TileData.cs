namespace GGJ23M
{
    public class TileData
    {
        private readonly Hex hex;

        public Hex Position => hex;

        public TileData(Hex hex)
        {
            this.hex = hex;
        }

        public TileData(int column, int row)
        {
            hex = new Hex(column, row);
        }
    }
}
