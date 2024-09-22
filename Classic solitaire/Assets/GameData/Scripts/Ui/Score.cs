using TMPro;
using UnityEngine;

namespace TheSyedMateen.ClassicSolitaire
{
    [RequireComponent(typeof(TMP_Text))]
    public class Score : MonoBehaviour
    {
        private TMP_Text _scoreText;
        private int _score;

        private void OnEnable()
        {
            _scoreText = GetComponent<TMP_Text>();
            EventManager.OnMoveComplete += SetScore;
        }

        private void OnDisable()
        {
            EventManager.OnMoveComplete -= SetScore;
        }

        private void SetScore()
        {
            _score += 5;
            _scoreText.text = _score.ToString();
        }
    }
}