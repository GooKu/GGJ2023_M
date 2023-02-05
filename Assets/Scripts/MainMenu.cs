using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGJ23M
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private GameStartProcess startProcess;

        private bool isStartClicked = false;

        private void Start()
        {
            AudioPlayer.Instance.PlayMusic(0);
        }

        public void OnStartClicked()
        {
            if (!isStartClicked)
            {
                isStartClicked = true;
                startProcess.Finished += ChangeScene;

                gameObject.SetActive(false);
                startProcess.Do();
            }
        }

        private void ChangeScene()
        {
            startProcess.Finished -= ChangeScene;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
