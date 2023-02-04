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

        public void SetEnergyAmount(int amount)
        {
            energyText.text = $"Energy: {amount}";
        }

        public void SetScore(int score)
        {
            scoreText.text = $"Score: {score}";
        }
    }
}
