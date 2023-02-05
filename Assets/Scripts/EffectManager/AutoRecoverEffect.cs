using UnityEngine;

namespace GGJ23M
{
    public class AutoRecoverEffect : MonoBehaviour
    {
        [SerializeField]
        private float recoverTime;

        private float timer;

        private void OnEnable()
        {
            timer = 0f;
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer > recoverTime)
            {
                if (EffectManager.Instance != null)
                {
                    EffectManager.Instance.Recover(gameObject);
                }
            }
        }
    }
}
