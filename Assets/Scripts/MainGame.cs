using UnityEngine;

namespace GGJ23M
{
    public class MainGame : MonoBehaviour
    {
        [SerializeField]
        private GameMapView gameMapView;

        private Player player = new();

        private void Awake()
        {
            gameMapView.SetUp();
            var root = new Root(null, new Hex(), 0);
            player.AddRoot(root);
        }
    }
}
