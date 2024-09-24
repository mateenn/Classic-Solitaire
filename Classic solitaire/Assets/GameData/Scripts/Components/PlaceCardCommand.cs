using System.Collections.Generic;
using UnityEngine;

namespace TheSyedMateen.ClassicSolitaire
{
    public class PlaceCardCommand : ICommand
    {
        private readonly IList<VisualCard> _movableCards;
        private readonly Slot _slot;
        private readonly Slot _previousSlot; // To track the previous slot for undo

        public PlaceCardCommand(IList<VisualCard> visualCards, Slot slot, Slot previousSlot)
        {
            this._movableCards = visualCards;
            this._slot = slot;
            _previousSlot = previousSlot;
        }

        public void Execute()
        {
            _slot.PlaceCard(_movableCards);
        }

        public void Undo()
        {
            if (_previousSlot != null)
            {
                /*//if card is from stack slot previously then there is only one card in a sing move that came
                //so we only sending a single card
                if (_previousSlot.slotType == SlotType.Stack)
                {
                    Debug.Log("Undo undoing: "+_previousSlot.slotType+" card: "+_movableCards[0]);
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
                else */
                    _previousSlot.PlaceCard(_movableCards); // Move the cards back to the previous slot
            }
        }
    }
}