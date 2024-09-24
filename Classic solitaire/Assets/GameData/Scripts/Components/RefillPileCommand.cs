using System.Collections.Generic;

namespace TheSyedMateen.ClassicSolitaire
{
    public class RefillPileCommand : ICommand
    {
        private readonly Pile _stackPile;
        private readonly Pile _wastePile;
        private readonly List<Card> _movedCards;

        public RefillPileCommand(Pile stackPile, Pile wastePile)
        {
            _stackPile = stackPile;
            _wastePile = wastePile;
            _movedCards = new List<Card>();
        }

        public void Execute()
        {
            var count = _wastePile.GetCardCount();
            for (int i = count - 1; i >= 0; i--)
            {
                var visualCard = _wastePile.GetAndRemoveCardAtIndex(i).VisualCardRef;
                var card = visualCard.GetCard();
                _stackPile.AddAndUpdateCardToPile(visualCard);
                visualCard.FlipCard(false);

                // Store the card for undo purposes
                _movedCards.Add(card);

                // Update card slot
                card.Slot = _stackPile.slot;
                card.Slot.CurrentCard = card;
            }
        }

        public void Undo()
        {
            // Undo by moving cards back from stack to waste
            for (int i = _movedCards.Count - 1; i >= 0; i--)
            {
                var card = _movedCards[i];
                var visualCard = card.VisualCardRef;
                
                // Move back to waste
                _stackPile.RemoveCardFromPile(visualCard);
                _wastePile.AddAndUpdateCardToPile(visualCard);
                visualCard.FlipCard(true);

                // Restore original slot
                card.Slot = _wastePile.slot;
                card.Slot.CurrentCard = card;
            }
        }
    }
}