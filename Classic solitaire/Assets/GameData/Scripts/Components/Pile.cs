using System.Collections.Generic;
using UnityEngine;

namespace TheSyedMateen.ClassicSolitaire
{
    public class Pile : MonoBehaviour
    {
        public Slot slot;
        private List<VisualCard> cardsInPile = new List<VisualCard>();
        private BoxCollider2D _collider2D;

        private void Awake()
        {
            if (GetComponent<BoxCollider2D>()) _collider2D = GetComponent<BoxCollider2D>();
        }

        public void AddCardToPile(VisualCard visualCard)
        {
            cardsInPile.Add(visualCard);
            CheckCollider();

            EvaluateFoundationCompletion(visualCard);
        }


        public void AddAndUpdateCardToPile(VisualCard visualCard)
        {
            cardsInPile.Add(visualCard);
            UpdateCardPositions(); // Update the visual stacking of cards in the pile
            CheckCollider();
            EvaluateFoundationCompletion(visualCard);
        }

        public void RemoveCardFromPile(VisualCard visualCard, bool isToUpdateCardPosition = true)
        {
            if (cardsInPile.Contains(visualCard))
            {
                cardsInPile.Remove(visualCard);
                if (isToUpdateCardPosition) UpdateCardPositions(); // Adjust positions after a card is removed
            }

            CheckCollider();
        }


        private void UpdateCardPositions()
        {
            // Check if the slot is a waste pile
            if (slot.slotType == SlotType.Waste)
            {
                // Stack only the top 3 cards with an offset, the rest stay beneath the 3rd card
                for (int i = 0; i < cardsInPile.Count; i++)
                {
                    VisualCard visualCard = cardsInPile[i];

                    if (i >= cardsInPile.Count - 3) // For all cards except the top 3
                    {
                        // Add an offset for the top 3 cards
                        float offset = -(cardsInPile.Count - 1 - i) * Variables.CardOffsetMultiplier;
                        visualCard.transform.localPosition = transform.position + new Vector3(offset, 0, 0);
                    }
                }
            }

            else if (slot.slotType == SlotType.Tableau)
            {
                // Original tableau pile stacking logic
                for (int i = 0; i < cardsInPile.Count; i++)
                {
                    VisualCard visualCard = cardsInPile[i];
                    visualCard.transform.localPosition =
                        transform.position +
                        new Vector3(0, -i * Variables.CardOffsetMultiplier, 0); // Stack cards with vertical spacing
                }
            }
            else
            {
                //GetTopCard().transform.localPosition = transform.position;
                var position = transform.position;
                var topCard = GetTopCard();
                if (topCard != null) topCard.transform.localPosition = new Vector3(position.x, position.y, 0);
            }
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

        public Card GetAndRemoveCardAtIndex(int index)
        {
            if (index >= 0 && index < cardsInPile.Count)
            {
                var card = cardsInPile[index].GetCard();
                cardsInPile.RemoveAt(index);
                CheckCollider();
                return card;
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

                //else
            }

            CheckCollider();
        }


        private readonly List<VisualCard> _splitCards = new List<VisualCard>();

        public IList<VisualCard> SplitAt(VisualCard card)
        {
            var index = GetIndexOfCard(card);

            if (index < 0 || index >= cardsInPile.Count)
                return null;

            _splitCards.Clear();

            for (var i = index; i < cardsInPile.Count; i++)
                _splitCards.Add(cardsInPile[i]);

            return _splitCards;
        }

        private void EvaluateFoundationCompletion(VisualCard visualCard)
        {
            if (slot.slotType != SlotType.Foundation) return;
            EventManager.InvokeMoveSuccessful();
            if (visualCard.cardType == Variables.CardTypes.King)
            {
                //it means all card of slot are sucessfully filled
                //we can check win condition here
                EventManager.InvokeFoundationPileFilled();
            }
        }

        private void CheckCollider()
        {
            CheckCardCollider();
            CheckPileCollider();
        }

        private void CheckPileCollider()
        {
            if (slot.slotType != SlotType.Tableau) return;
            if (cardsInPile.Count > 0)
            {
                if (_collider2D.enabled == true) _collider2D.enabled = false;
            }
            else
            {
                if (_collider2D.enabled == false) _collider2D.enabled = true;
            }
        }

        private void CheckCardCollider()
        {
            if (slot.slotType == SlotType.Waste || slot.slotType == SlotType.Foundation)
            {
                var cardCount = cardsInPile.Count;
                if (cardCount <= 0) return;
                if (cardCount == 1)
                {
                    //only single card in pile
                    //enabling top card collider so user can interact with it
                    cardsInPile[0].GetCard().Collider2D.enabled = true;
                    return;
                }

                cardsInPile[cardCount - 2].GetCard().Collider2D.enabled =
                    false; //setting the second last card collider false
                cardsInPile[cardCount - 1].GetCard().Collider2D.enabled = true; //setting the current card collider true
            }
        }
    }
}