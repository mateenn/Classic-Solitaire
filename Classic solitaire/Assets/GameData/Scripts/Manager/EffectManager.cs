using UnityEngine;

namespace TheSyedMateen.ClassicSolitaire
{
    public class EffectManager : MonoBehaviour
    {
        private void OnEnable()
        {
            EventManager.OnWrongMove += PlayWrongEffect;
            EventManager.OnLevelComplete += PlayLevelCompleteEffect;
        }

        private void OnDisable()
        {
            EventManager.OnWrongMove -= PlayWrongEffect;
            EventManager.OnLevelComplete -= PlayLevelCompleteEffect;
        }

        private void PlayWrongEffect()
        {
            Vibration.VibratePop();
        }

        private void PlayLevelCompleteEffect()
        {
            //play confetti effect
        }
    }
}