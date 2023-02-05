using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GGJ23M
{
    public class GameResultUI : MonoBehaviour
    {
        [SerializeField]
        private Text scoreText;
        [SerializeField]
        private Animator anim;

        public void Show(int score, bool isPass)
        {
            gameObject.SetActive(true);
            scoreText.text = $"Score: {score}";
            anim.Play(isPass ? "PassEnd" : "NormalEnd");
            if (isPass)
            {
                AudioPlayer.Instance.PlayMusic(2);
            }
            else
            {
                AudioPlayer.Instance.PlayMusic(1);
            }
        }

        public void Again()
        {
            SceneManager.LoadScene(1);
        }
    }
}
