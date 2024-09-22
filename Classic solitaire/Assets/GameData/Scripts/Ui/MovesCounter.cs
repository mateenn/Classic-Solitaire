
using TMPro;
using UnityEngine;

namespace TheSyedMateen.ClassicSolitaire
{
    [RequireComponent(typeof(TMP_Text))]
    public class MovesCounter : MonoBehaviour
    {
        private TMP_Text _counterText;
        private int _moves;

        private void OnEnable()
        {
            _counterText = GetComponent<TMP_Text>();
            EventManager.OnMoveComplete += SetMoves;
        }

        private void OnDisable()
        {
            EventManager.OnMoveComplete -= SetMoves;
        }

        private void SetMoves()
        {
            _moves += 1;
            _counterText.text = "Total Moves: " + _moves;
        }
    }
}