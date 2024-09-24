using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TheSyedMateen.ClassicSolitaire
{
    public class Hud : MonoBehaviour
    {
        [SerializeField] private GameObject levelCompleteUi;
        [SerializeField] private Button nextButton;

        private void OnEnable()
        {
            EventManager.OnLevelComplete += PlayLevelCompleteEffect;
            nextButton.onClick.AddListener(RestartGame);
        }

        private void OnDisable()
        {
            EventManager.OnLevelComplete -= PlayLevelCompleteEffect;
            nextButton.onClick.RemoveListener(RestartGame);
        }

        private void PlayLevelCompleteEffect()
        {
            //play confetti effect
            levelCompleteUi.SetActive(true);
        }

        private void RestartGame()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
        }
    }
}