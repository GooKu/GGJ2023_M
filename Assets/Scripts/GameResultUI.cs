using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameResultUI : MonoBehaviour
{
    [SerializeField]
    private GameObject summaryRoot;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Animator anim;

    public void Show(int score, bool isPass)
    {
        gameObject.SetActive(true);
        scoreText.text = score.ToString();
        anim.Play(isPass ? "PassEnd" : "NormalEnd");
    }
}
