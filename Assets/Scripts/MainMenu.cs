using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGJ23M
{
    public class MainMenu : MonoBehaviour
    {

        private void Start()
        {
            AudioPlayer.Instance.PlayMusic(0);
        }

        public void ChangeScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

}
