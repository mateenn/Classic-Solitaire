using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TheSyedMateen.ClassicSolitaire
{
    public class Hud : MonoBehaviour
    {
        [SerializeField] private GameObject levelCompleteUi;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button undoButton;

        private void OnEnable()
        {
            EventManager.OnLevelComplete += PlayLevelCompleteEffect;
            nextButton.onClick.AddListener(RestartGame);
            undoButton.onClick.AddListener(UndoButtonClick);
        }

        private void OnDisable()
        {
            EventManager.OnLevelComplete -= PlayLevelCompleteEffect;
            nextButton.onClick.RemoveListener(RestartGame);
            undoButton.onClick.RemoveListener(UndoButtonClick);
        }

        private void PlayLevelCompleteEffect()
        {
            //play confetti effect
            undoButton.gameObject.SetActive(false);
            levelCompleteUi.SetActive(true);
        }

        private void RestartGame()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
        }

        private void UndoButtonClick()
        {
            CommandInvoker.UndoCommand();
        }
    }
}