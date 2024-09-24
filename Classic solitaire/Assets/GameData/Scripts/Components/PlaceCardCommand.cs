using System.Collections.Generic;
using UnityEngine;

namespace TheSyedMateen.ClassicSolitaire
{
    public class PlaceCardCommand : ICommand
    {
        private readonly IList<VisualCard> _movableCards;
        private readonly Slot _slot;
        private readonly Slot _previousSlot; // To track the previous slot for undo
        
        // Track the second card's face state
        private readonly VisualCard _secondCard;
        private readonly bool _wasSecondCardFaceUp;

        public PlaceCardCommand(IList<VisualCard> visualCards, Slot slot, Slot previousSlot)
        {
            this._movableCards = visualCards;
            this._slot = slot;
            _previousSlot = previousSlot;

            /*var pile = _previousSlot.GetComponent<Pile>();
            // Find the second card in the previous slot if it exists
            if (pile.GetCardCount() > 1)
            {
                _secondCard = pile.GetCardAtIndex(pile.GetCardCount() - 2).VisualCardRef;  // Assuming this returns the second card after the top card is moved
                _wasSecondCardFaceUp = _secondCard.GetCard().IsFaceUp;  // Store whether the second card was face up
            }*/
        }

        public void Execute()
        {
            _slot.PlaceCard(_movableCards);
        }

        public void Undo()
        {
            if (_previousSlot != null)
            {
                //if card is from stack slot previously then there is only one card in a sing move that came
                //so we only sending a single card
                Debug.Log("Undo undoing: "+_previousSlot.slotType+" card: "+_movableCards[0]+" currentSlot: "+_slot.slotType);
                if (_previousSlot.slotType == SlotType.Stack)
                {
                    
                    var pile = _slot.GetComponent<Pile>();
                    //_previousSlot.AssignCard(_movableCards[0].GetCard());
                    var visualCard = pile.GetAndRemoveCardAtIndex(pile.GetCardCount() - 1).VisualCardRef;
                    var card = visualCard.GetCard();
                    _previousSlot.GetComponent<Pile>().AddAndUpdateCardToPile(visualCard);
                    visualCard.FlipCard(false);
                    //updating card slot
                        
                    card.Slot =  _previousSlot;
                    card.Slot.CurrentCard = card;
                }
                else 
                {
                    _previousSlot.PlaceCard(_movableCards); // Move the cards back to the previous slot
                    /*if (_secondCard != null && _secondCard.GetCard().IsFaceUp != _wasSecondCardFaceUp)
                    {
                        _secondCard.FlipCard(_wasSecondCardFaceUp);  // Flip it back to its original state
                    }*/
                }
                
            }
        }
    }
}