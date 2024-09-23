using System;
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
            Helper.Log("Adding Card to this pile: " + gameObject, gameObject);
            cardsInPile.Add(visualCard);
            CheckCollider();
        }

        private void CheckCollider()
        {
            if (slot.slotType != SlotType.Tableau) return;
            if(cardsInPile.Count>0)
            {
                if (_collider2D.enabled == true) _collider2D.enabled = false;
            }
            else
            {
                if (_collider2D.enabled == false) _collider2D.enabled = true;
            }
        }

        public void AddAndUpdateCardToPile(VisualCard visualCard)
        {
            Helper.Log("Adding Card to this pile: " + gameObject, gameObject);
            cardsInPile.Add(visualCard);
            UpdateCardPositions(); // Update the visual stacking of cards in the pile
            CheckCollider();
        }

        public void RemoveCardFromPile(VisualCard visualCard, bool isToUpdateCardPosition = true)
        {
            if (cardsInPile.Contains(visualCard))
            {
                cardsInPile.Remove(visualCard);
                if(isToUpdateCardPosition) UpdateCardPositions(); // Adjust positions after a card is removed
            }

            CheckCollider();
        }


        private void UpdateCardPositions()
        {
            Helper.Log("stacking cards: " + cardsInPile.Count);

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
                        Helper.Log("Stting Position: "+cardsInPile.Count+ " ind: "+i+" card: "+visualCard);
                        float offset = -(cardsInPile.Count - 1 - i) * 0.65f;
                        visualCard.transform.localPosition = transform.position + new Vector3(offset, 0, 0);
                        
                    }
                    /*else
                    {
                        // Move the cards beneath the 3rd card in the stack
                        float offset = -(cardsInPile.Count - 2);
                        visualCard.transform.localPosition = transform.position + new Vector3(offset, 0, 0);
                        //visualCard.transform.localPosition = transform.position; // Align with the waste slot position
                    }*/
                }
            }

            else if (slot.slotType == SlotType.Tableau)
            {
                // Original tableau pile stacking logic
                for (int i = 0; i < cardsInPile.Count; i++)
                {
                    VisualCard visualCard = cardsInPile[i];
                    visualCard.transform.localPosition =
                        transform.position + new Vector3(0, -i * 0.65f, 0); // Stack cards with vertical spacing
                }
            }
            else
            {
                //GetTopCard().transform.localPosition = transform.position;
                var position = transform.position;
                GetTopCard().transform.localPosition = new Vector3(position.x,position.y,0);
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
                var card =  cardsInPile[index].GetCard();
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
            }

            CheckCollider();
        }
    }
}