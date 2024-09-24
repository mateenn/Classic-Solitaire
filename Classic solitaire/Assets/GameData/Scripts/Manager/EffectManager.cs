using UnityEngine;

namespace TheSyedMateen.ClassicSolitaire
{
    public class EffectManager : MonoBehaviour
    {
        private void OnEnable()
        {
            EventManager.OnWrongMove += PlayWrongEffect;
        }

        private void OnDisable()
        {
            EventManager.OnWrongMove -= PlayWrongEffect;
        }

        private void PlayWrongEffect()
        {
            Vibration.VibratePop();
        }

       
    }
}