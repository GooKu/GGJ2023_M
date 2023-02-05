using System.Collections.Generic;
using UnityEngine;

namespace GGJ23M
{
    public class EffectManager : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> prefabs = new();

        public static EffectManager Instance;

        private readonly Dictionary<GameObject, UnityObjectPool<GameObject>> pools = new();

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this);
        }

        public void Play(int index, Vector3 position)
        {
            if (index < 0 || index >= prefabs.Count)
            {
                Debug.LogWarning($"Invalid effect index {index}");
                return;
            }

            GameObject obj = Get(prefabs[index]);
            obj.transform.position = position;
            obj.SetActive(true);
        }


        public GameObject Get(GameObject key)
        {
            if (!pools.ContainsKey(key))
            {
                CreateNewPool(key);
            }

            return pools[key].Get();
        }

        private void CreateNewPool(GameObject key)
        {
            UnityObjectPool<GameObject> pool = new UnityObjectPool<GameObject>(key, 5, false);
            pools.Add(key, pool);
        }

        public void Recover(GameObject obj)
        {
            foreach (var pool in pools.Values)
            {
                if (pool.IsObjectBelongToPool(obj))
                {
                    pool.Recover(obj);
                    return;
                }
            }
        }

        public void Destroy()
        {
            foreach (var pool in pools.Values)
            {
                pool.Destroy();
            }

            pools.Clear();
        }
    }
}
