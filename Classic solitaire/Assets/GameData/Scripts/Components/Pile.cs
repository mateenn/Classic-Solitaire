using System.Collections.Generic;
using UnityEngine;

namespace TheSyedMateen.ClassicSolitaire
{
    public class Pile : MonoBehaviour
    {
        public Slot slot;
        private List<VisualCard> cardsInPile = new List<VisualCard>();

        public void AddCardToPile(VisualCard visualCard)
        {
            cardsInPile.Add(visualCard);
            visualCard.transform.SetParent(this.transform);
            UpdateCardPositions(); // Update the visual stacking of cards in the pile
        }

        public void RemoveCardFromPile(VisualCard visualCard)
        {
            if (cardsInPile.Contains(visualCard))
            {
                cardsInPile.Remove(visualCard);
                visualCard.transform.SetParent(null);
                UpdateCardPositions(); // Adjust positions after a card is removed
            }
        }

        private void UpdateCardPositions()
        {
            for (int i = 0; i < cardsInPile.Count; i++)
            {
                VisualCard visualCard = cardsInPile[i];
                visualCard.transform.localPosition = new Vector3(0, -i * 0.2f, 0); // Stack cards with some vertical spacing
            }
        }

        public bool CanPlaceCard(VisualCard visualCard)
        {
            if (cardsInPile.Count == 0)
            {
                return true; // Can place any card if the pile is empty
            }

            VisualCard topCard = cardsInPile[cardsInPile.Count - 1];
            return slot.CanMoveCard(topCard.GetCard()); // Check if the card can be stacked on the top card
        }

        public VisualCard GetTopCard()
        {
            if (cardsInPile.Count > 0)
            {
                return cardsInPile[cardsInPile.Count - 1];
            }
            return null;
        }

        public void FlipTopCard()
        {
            if (cardsInPile.Count > 0)
            {
                VisualCard topCard = GetTopCard();
                topCard.FlipCard(true); // Flip the top card to face up
            }
        }
    }
}
