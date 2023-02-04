using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGJ23M
{
    public class MainMenu : MonoBehaviour
    {


        void Update()
        {
            if (Input.anyKeyDown)
            {
                ChangeScene();
            }
        }

        void ChangeScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

}
