using System.Collections.Generic;
using UnityEngine;

namespace TheSyedMateen.ClassicSolitaire
{
    public class Card
    {
        public Variables.Suits Suit { get; private set; }
        public Variables.CardTypes CardType { get; private set; }

        public VisualCard VisualCardRef { get; set; } // Reference to the VisualCard
        public Vector3 OriginalPosition { get; set; } // Reference to the VisualCard

        public bool IsFaceUp { get; private set; }
        public Slot Slot { get; set; }
        
        public Collider2D Collider2D { get; set; }

        public Card(Variables.Suits suit, Variables.CardTypes cardType, Slot slot,Collider2D collider2D)
        {
            Suit = suit;
            CardType = cardType;
            IsFaceUp = false; // Initially face down
            Slot = slot;
            Collider2D = collider2D;
        }

        public void SetFaceUp(bool isFaceUp)
        {
            IsFaceUp = isFaceUp;
        }

        public void Flip()
        {
            IsFaceUp = !IsFaceUp;
        }

        // Get all face-up cards below this card in the pile
        public List<Card> GetMovableCards()
        {
            List<Card> movableCards = new List<Card>();
            Card currentCard = this;

            // Continue adding cards until we hit a card that is not face up or is not on top
            while (currentCard != null && currentCard.IsFaceUp)
            {
                movableCards.Add(currentCard);
                if (currentCard.Slot is Slot slot)
                {
                    currentCard = slot.GetNextCard(currentCard); // Get the next card below
                }
                else
                {
                    break;
                }
            }

            return movableCards;
        }

        private bool IsTopCard()
        {
            var pile = Slot.GetComponent<Pile>();
            return pile != null && pile.GetTopCard() == VisualCardRef;
        }

        public bool IsMoveable()
        {
            if (Slot == null || Slot.slotType == SlotType.Foundation) return false;
            return ((Slot.slotType == SlotType.Waste && IsTopCard() && IsFaceUp) ||
                    (Slot.slotType != SlotType.Waste && IsFaceUp));
        }
    }
}
