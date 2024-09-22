using UnityEngine;
using UnityEngine.EventSystems;

namespace TheSyedMateen.ClassicSolitaire
{
    public enum SlotType
    {
        Tableau,
        Foundation,
        Waste,
        Stack
    }

    public class Slot : MonoBehaviour, IDropHandler
    {
        public SlotType slotType; // Define the type of the slot
        public Card CurrentCard { get; private set; }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                Card draggedCard = eventData.pointerDrag.GetComponent<Card>();
                if (draggedCard != null && CanMoveCard(draggedCard))
                {
                    PlaceCard(draggedCard);
                }
                else
                {
                    // Play error sound or show feedback
                    Debug.Log("Cannot place card here.");
                }
            }
        }


        public bool CanMoveCard(Card card)
        {
            Helper.Log("Checking for card: " + card);

            if (CurrentCard == null)
            {
                // For an empty Tableau slot, only allow Kings
                if (slotType == SlotType.Tableau && card.CardType == Variables.CardTypes.King)
                {
                    Helper.Log("Empty Tableau slot, only allow Kings.");
                    return true;
                }

                // For an empty Foundation slot, only allow Aces
                if (slotType == SlotType.Foundation && card.CardType == Variables.CardTypes.Ace)
                {
                    Helper.Log("Empty Foundation slot, only allow Aces.");
                    return true;
                }

                return false; // For other slots or card types, return false
            }

            Helper.Log("CanStack: " + CanStack(CurrentCard, card) + " currentCard: " + CurrentCard.VisualCardRef
                       + " movingCard: " + card.VisualCardRef);
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
            if (slotType == SlotType.Tableau)
            {
                // Handle Tableau slot: Use the Pile for stacking cards
                Pile pile = GetComponent<Pile>();
                Debug.Log("Placing card: " + pile, pile);

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

        public void PlaceCard(Card card)
        {
            if (card.Slot != null && card.Slot is Slot previousSlot)
            {
                Pile previousPile = previousSlot.GetComponent<Pile>();
                if (previousPile != null)
                {
                    previousPile.RemoveCardFromPile(card.VisualCardRef); // Remove the card from the old pile
                    previousPile.FlipTopCard();
                    var topCard = previousPile.GetTopCard()?.GetCard();
                    if (topCard != null) topCard.Slot.CurrentCard = topCard;
                    else previousPile.slot.CurrentCard = null;
                }
            }

            if (slotType == SlotType.Stack) return;

            Pile pile = GetComponent<Pile>();

            if (pile != null)
            {
                pile.AddAndUpdateCardToPile(card.VisualCardRef); // Add the card to the pile (stacking)

                // Set the sorting order to be on top of the pile based on card count
                int newSortingOrder = pile.GetCardCount();
                card.VisualCardRef.SetSortingOrder(newSortingOrder); // Set correct sorting order in pile
                CurrentCard = card;
            }

            card.Slot = this; // Update the card's slot reference
            EventManager.InvokeMoveComplete();
        }
    }
}