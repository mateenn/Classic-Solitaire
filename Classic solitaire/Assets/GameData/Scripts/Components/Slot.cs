using System.Collections.Generic;
using UnityEngine;

namespace TheSyedMateen.ClassicSolitaire
{
    public enum SlotType
    {
        Tableau,
        Foundation,
        Waste,
        Stack
    }

    public class Slot : MonoBehaviour
    {
        public SlotType slotType; // Define the type of the slot
        public Card CurrentCard { get; set; }
        

        public bool CanMoveCard(Card card)
        {
            Helper.Log("Checking for card: " + card.VisualCardRef + " slot name: " + gameObject);

            if (CurrentCard == null)
            {
                // For an empty Tableau slot, only allow Kings
                if (slotType == SlotType.Tableau && card.CardType == Variables.CardTypes.King)
                {
                    return true;
                }

                // For an empty Foundation slot, only allow Aces
                if (slotType == SlotType.Foundation && card.CardType == Variables.CardTypes.Ace)
                {
                    return true;
                }

                return false; // For other slots or card types, return false
            }
            
            // Check stacking rules based on slot type
            return slotType == SlotType.Tableau ? CanStack(CurrentCard, card) : CanPlaceInFoundation(CurrentCard, card);
        }


        public Card GetNextCard(Card card)
        {
            if (slotType == SlotType.Tableau)
            {
                Pile pile = GetComponent<Pile>();
                if (pile != null)
                {
                    int index = pile.GetIndexOfCard(card.VisualCardRef); // Implement this method
                    if (index > 0)
                    {
                        return pile.GetCardAtIndex(index - 1); // Get the card just below it
                    }
                }
            }

            return null;
        }

        private bool CanStack(Card baseCard, Card movingCard)
        {
            bool isOppositeColor = (baseCard.Suit == Variables.Suits.Heart || baseCard.Suit == Variables.Suits.Diamond)
                ? (movingCard.Suit == Variables.Suits.Spade || movingCard.Suit == Variables.Suits.Club)
                : (movingCard.Suit == Variables.Suits.Heart || movingCard.Suit == Variables.Suits.Diamond);

            return isOppositeColor && (movingCard.CardType == baseCard.CardType - 1);
        }

        private bool CanPlaceInFoundation(Card foundationCard, Card movingCard)
        {
            // Assuming foundation slots only accept Ace of the same suit or a sequence
            if (foundationCard == null && movingCard.CardType == Variables.CardTypes.Ace)
            {
                return true; // Foundation is empty and accepts Ace
            }

            Helper.Log("Moving cardType is: " + movingCard.CardType + "foundation: " + foundationCard.CardType +
                       " inc: " + foundationCard.CardType + 1);
            // Check if the moving card matches the foundation card's suit and is one rank higher
            return foundationCard != null && foundationCard.Suit == movingCard.Suit &&
                   movingCard.CardType == foundationCard.CardType + 1;
        }

        public void AssignCard(Card card)
        {
            if (slotType == SlotType.Tableau || slotType == SlotType.Stack)
            {
                RemoveFromSlot(card, false, false);

                // Handle Tableau slot: Use the Pile for stacking cards
                Pile pile = GetComponent<Pile>();

                if (pile != null)
                {
                    pile.AddCardToPile(card.VisualCardRef); // Add the card to the pile (stacking)

                    //CurrentCard = card;
                    card.Slot = this;
                }
            }
            else
            {
                card.Slot = this;
            }

            CurrentCard = card;
        }

        public void PlaceCard(IList<VisualCard> cards)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                RemoveFromSlot(cards[i].GetCard(), true);
            }

            if (slotType == SlotType.Stack) return;

            Pile pile = GetComponent<Pile>();

            if (pile != null)
            {
                for (int i = 0; i < cards.Count; i++)
                {
                    pile.AddAndUpdateCardToPile(cards[i]); // Add the card to the pile (stacking)

                    // Set the sorting order to be on top of the pile based on card count
                    int newSortingOrder = pile.GetCardCount();
                    cards[i].SetSortingOrder(newSortingOrder); // Set correct sorting order in pile
                    CurrentCard = cards[i].GetCard();
                }
            }

            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].GetCard().Slot = this; // Update the card's slot reference
            }

            EventManager.InvokeMoveComplete();
        }

        public void RemoveFromSlot(Card card, bool isToFlipFollowUpCard, bool isToUpdateCardPosition = true)
        {
            if (card.Slot != null && card.Slot is Slot previousSlot)
            {
                Pile previousPile = previousSlot.GetComponent<Pile>();
                if (previousPile != null)
                {
                    previousPile.RemoveCardFromPile(card.VisualCardRef,
                        isToUpdateCardPosition); // Remove the card from the old pile
                    if (isToFlipFollowUpCard) previousPile.FlipTopCard();
                    var topCard = previousPile.GetTopCard()?.GetCard();
                    if (topCard != null) topCard.Slot.CurrentCard = topCard;
                    else previousPile.slot.CurrentCard = null;
                }
            }
        }
    }
}