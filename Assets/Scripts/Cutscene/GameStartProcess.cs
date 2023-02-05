using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ23M
{
    public class GameStartProcess : MonoBehaviour
    {
        public event Action Finished;

        [SerializeField]
        private Animator cutsceneAnimator;
        [SerializeField]
        private Image image;
        [SerializeField]
        private List<Sprite> sprites = new();

        public void Do()
        {
            gameObject.SetActive(true);
            StartCoroutine(Process());
        }

        private IEnumerator Process()
        {
            for (var i = 0; i < sprites.Count; i++)
            {
                yield return PlaySpriteProcess(sprites[i]);
            }

            Finished?.Invoke();
        }

        private IEnumerator PlaySpriteProcess(Sprite sprite)
        {
            image.sprite = sprite;
            cutsceneAnimator.Play("Start");

            while (true)
            {
                AnimatorStateInfo stateInfo = cutsceneAnimator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("Start") && stateInfo.normalizedTime < 1f)
                {
                    yield return null;
                    continue;
                }

                break;
            }
        }
    }
}
