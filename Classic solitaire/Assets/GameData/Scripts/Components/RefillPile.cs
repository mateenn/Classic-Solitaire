using UnityEngine;

namespace TheSyedMateen.ClassicSolitaire
{
    public class RefillPile : MonoBehaviour
    {
        [SerializeField] private Pile stackPile, wastePile;

        private void OnMouseDown()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            
            if (hit.collider != null)
            {
                if (stackPile.slot.slotType == SlotType.Stack)
                {
                    if (wastePile == null) return;

                    // Create and execute the RefillPileCommand
                    var refillPileCommand = new RefillPileCommand(stackPile, wastePile);
                    CommandInvoker.ExecuteCommand(refillPileCommand);
                }
            }
        }
    }
}