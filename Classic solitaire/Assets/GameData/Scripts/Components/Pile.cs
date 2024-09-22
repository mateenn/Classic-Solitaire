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
            Debug.Log("Adding Card to this pile: " + gameObject, gameObject);
            cardsInPile.Add(visualCard);
            //visualCard.transform.SetParent(this.transform);
            //UpdateCardPositions(); // Update the visual stacking of cards in the pile
        }

        public void AddAndUpdateCardToPile(VisualCard visualCard)
        {
            Debug.Log("Adding Card to this pile: " + gameObject, gameObject);
            cardsInPile.Add(visualCard);
            UpdateCardPositions(); // Update the visual stacking of cards in the pile
        }

        public void RemoveCardFromPile(VisualCard visualCard)
        {
            if (cardsInPile.Contains(visualCard))
            {
                cardsInPile.Remove(visualCard);
                UpdateCardPositions(); // Adjust positions after a card is removed
            }
        }

        private void UpdateCardPositions()
        {
            Helper.Log("stacking cards: " + cardsInPile.Count);
            for (int i = 0; i < cardsInPile.Count; i++)
            {
                VisualCard visualCard = cardsInPile[i];
                visualCard.transform.localPosition =
                    transform.position + new Vector3(0, -i * 0.5f, 0); // Stack cards with some vertical spacing
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

        public int GetCardCount()
        {
            return cardsInPile.Count;
        }
        
        public int GetIndexOfCard(VisualCard visualCard)
        {
            return cardsInPile.IndexOf(visualCard);
        }
        public Card GetCardAtIndex(int index)
        {
            if (index >= 0 && index < cardsInPile.Count)
            {
                return cardsInPile[index].GetCard();
            }
            return null;
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