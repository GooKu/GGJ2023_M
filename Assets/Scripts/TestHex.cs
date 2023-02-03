using UnityEngine;

namespace GGJ23M
{
    public class TestHex : MonoBehaviour
    {
        public GameObject prefab;
        public float hexSize = 1f;

        private void Start()
        {
            for (var i = -5; i <= 5; i++)
            {
                for (var j = 0; j <= 5; j++)
                {
                    var hex = new Hex(i, j);
                    Vector2 point = hex.ToPoint(hexSize);
                    GameObject clone = Instantiate(prefab, point, Quaternion.identity, transform);
                    clone.name = $"hex_{i}_{j}";
                }
            }
        }

        private void Update()
        {
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Hex hex = Hex.PointToHex(worldPosition, hexSize);
            Debug.Log($"{worldPosition} => {hex}");
        }
    }
}
