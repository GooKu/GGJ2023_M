using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    }

    public void Again()
    {
        SceneManager.LoadScene(1);
    }
}
