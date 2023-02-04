using UnityEngine;

namespace GGJ23M
{
    public class TileView : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer render;

        [Space]
        [SerializeField]
        private Sprite[] tileSprites = new Sprite[0];
        [SerializeField]
        private GameObject[] tileObjectPrefabs = new GameObject[0];
        [SerializeField]
        private Sprite[] rootSprites = new Sprite[0];

        private TileData.TileType tileType;
        private GameObject tileObject;

        public void SetType(TileData.TileType tileType)
        {
            if (this.tileType == tileType)
            {
                return;
            }

            if (tileObject != null)
            {
                Destroy(tileObject);
                tileObject = null;
            }

            this.tileType = tileType;

            Sprite sprite = tileSprites[(int)tileType];
            if (sprite == null)
            {
                sprite = tileSprites[0];
            }
            render.sprite = sprite;

            GameObject objPrefab = tileObjectPrefabs[(int)tileType];
            if (objPrefab != null)
            {
                tileObject = Instantiate(objPrefab, transform);
                tileObject.transform.localPosition = new Vector3();
            }
        }

        public void UpdateSprite(int index)
        {
            if (index == -1)
            {
                render.sprite = tileSprites[0];
                return;
            }

            render.sprite = rootSprites[index];
        }
    }
}
