using TMPro;
using UnityEngine;

namespace GGJ23M
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI energyText;
        [SerializeField]
        private TextMeshProUGUI scoreText;
        [SerializeField]
        private GameResultUI resultUI;
        [SerializeField]
        private Animator hitEffect;

        public void SetEnergyAmount(int amount)
        {
            energyText.text = $"Energy: {amount}";
        }

        public void SetScore(int score)
        {
            scoreText.text = $"Score: {score}";
        }

        public void ShowEnd(int score, bool isPass)
        {
            resultUI.Show(score, isPass);
        }

        public void ShowHit()
        {
            hitEffect.Play("Hit");
        }
    }
}
