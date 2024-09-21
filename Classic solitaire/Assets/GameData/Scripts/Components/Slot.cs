using UnityEngine;
using UnityEngine.EventSystems;

namespace TheSyedMateen.ClassicSolitaire
{
    public enum SlotType
    {
        Tableau,
        Foundation,
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
            Helper.Log("Checking for card: "+card);
            if (CurrentCard == null)
            {
                // Empty slot can accept any card
                Helper.Log("Checking for return true ");
                return true;
            }

            Helper.Log("Checking slot type "+slotType+" can: "+CanStack(CurrentCard, card));
            // Check stacking rules based on slot type
            return slotType == SlotType.Tableau ? CanStack(CurrentCard, card) : CanPlaceInFoundation(CurrentCard, card);
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

            // Check if the moving card matches the foundation card's suit and is one rank higher
            return foundationCard != null && foundationCard.Suit == movingCard.Suit && movingCard.CardType == foundationCard.CardType + 1;
        }

        public void PlaceCard(Card card)
        {
            CurrentCard = card;
            card.Slot = this; // Set the slot reference in the card
            // Here you can add logic to update the card's position in the game world
        }

        public void RemoveCard()
        {
            if (CurrentCard != null)
            {
                CurrentCard.Slot = null; // Clear the slot reference in the card
                CurrentCard = null;
            }
        }
    }
}
