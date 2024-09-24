using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace TheSyedMateen.ClassicSolitaire
{
    public class VisualCard : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer front;
        [SerializeField] private SpriteRenderer back;

        public Variables.Suits suit;
        public Variables.CardTypes cardType;
        private Card _card;

        private int _previousSortingOrder = 1;

        private Vector3 _originalPosition;
        private bool _isDragging = false;

        private IList<VisualCard> _movableCards = new List<VisualCard>(); // List to track all the cards being moved

        public Card SetCard(Slot slot)
        {
            _card = new Card(suit, cardType, slot, GetComponent<Collider2D>());
            _card.VisualCardRef = this; // Set the reference to the VisualCard
            UpdateCardVisual();
            return _card;
        }

        public Card GetCard()
        {
            return _card;
        }

        public void FlipCard(bool isFaceUp)
        {
            _card.SetFaceUp(isFaceUp); // Assuming your Card class has this method
            UpdateCardVisual();
            if (_card.Slot.slotType == SlotType.Tableau)
            {
                _card.Collider2D.enabled = isFaceUp;
            }
        }

        private void UpdateCardVisual()
        {
            front.gameObject.SetActive(_card.IsFaceUp);
            back.gameObject.SetActive(!_card.IsFaceUp);
        }

        public void SetSortingOrder(int order)
        {
            front.sortingOrder = order;
            back.sortingOrder = order - 1;
            if (order != 1000) _previousSortingOrder = order;
            Helper.Log("Setting Card Order: " + order + " : " + gameObject, gameObject);
        }

        private void Update()
        {
            if (_isDragging)
            {
                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPosition.z = 0; // Ensure the z-coordinate is zero for 2D

                //transform.position = mouseWorldPosition; // Move the card with the mouse

                // Move all the cards that are being dragged
                for (int i = 0; i < _movableCards.Count; i++)
                {
                    VisualCard visualCard = _movableCards[i]._card.VisualCardRef;
                    visualCard.transform.position =
                        mouseWorldPosition +
                        new Vector3(0, -i * Variables.CardOffsetMultiplier, 0); // Stack with offset
                }

                // Check for mouse release
                if (Input.GetMouseButtonUp(0))
                {
                    _isDragging = false;

                    // Check for overlapping colliders (instead of raycasting)
                    Collider2D hitCollider = Physics2D.OverlapPoint(mouseWorldPosition);

                    if (hitCollider != null)
                    {
                        // Try to get the Slot or Pile
                        if (hitCollider.TryGetComponent<Slot>(out Slot slot))
                        {
                            // Drop the card on the slot itself (empty tableau slot or foundation)
                            TryDropOnSlot(slot);
                        }
                        else if (hitCollider.TryGetComponent<VisualCard>(out VisualCard visualCard))
                        {
                            // Drop the card on another card (in tableau piles)
                            TryDropOnCard(visualCard);
                        }
                        else
                        {
                            // No valid drop target, return to original position
                            ReturnToOriginalPosition();
                            ;
                            EventManager.InvokeWrongMove();
                        }
                    }
                    else
                    {
                        // No collider hit, return the card to the original position
                        ReturnToOriginalPosition();
                    }

                    for (int i = 0; i < _movableCards.Count; i++)
                    {
                        Debug.Log("Setting Position orig pos " + _movableCards[i]);
                        _movableCards[i]._card.Collider2D.enabled = true;
                        //_movableCards[i]._card.OriginalPosition = _movableCards[i]._card.VisualCardRef.transform.position; // Store the original position
                    }
                    transform.DOScale(1f, .2f);
                }
            }
        }

        private void TryDropOnSlot(Slot slot)
        {
            if (IsValidDropTarget(slot))
            {
                //slot.PlaceCard(_movableCards);
                CommandInvoker.ExecuteCommand(new PlaceCardCommand(_movableCards, slot,_card.Slot));

            }
            else
            {
                // Invalid drop target, return to original position
                ReturnToOriginalPosition();
                EventManager.InvokeWrongMove();
            }
        }

        private void TryDropOnCard(VisualCard visualCard)
        {
            // Get the slot where the card is located
            //Slot cardSlot = visualCard.GetComponentInParent<Slot>();
            Slot cardSlot = visualCard._card.Slot;

            Helper.Log("collider cardSlot: " + cardSlot + " isValid: " + IsValidDropTarget(cardSlot));
            // Check if the drop is valid based on the card in the pile
            if (cardSlot != null && IsValidDropTarget(cardSlot))
            {
                //cardSlot.PlaceCard(_movableCards); // Place the card in the target slot/pile
                CommandInvoker.ExecuteCommand(new PlaceCardCommand(_movableCards, cardSlot,_card.Slot));
            }
            else
            {
                // Invalid drop target, return to original position
                ReturnToOriginalPosition();
                EventManager.InvokeWrongMove();
            }
        }

        private bool IsValidDropTarget(Slot targetSlot)
        {
            // Check if the target slot is valid for the card to be placed in
            return targetSlot.CanMoveCard(_card);
        }

        private void OnMouseDown()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            
            if (!_card.IsMoveable())
            {
                if (_card.Slot == null || _card.Slot.slotType == SlotType.Stack)
                {
                    MoveToWaste();
                }
                else
                {
                    transform.DOScale(1.1f, .2f);
                }

                return;
            }

            transform.DOScale(1.1f, .2f);
            _isDragging = true; // Start dragging
            SetSortingOrder(1000);

            _movableCards = _card.Slot.gameObject.GetComponent<Pile>().SplitAt(_card.VisualCardRef);
            // Get all movable cards from this card onward
            //_movableCards = _card.GetMovableCards();

            for (int i = 0; i < _movableCards.Count; i++)
            {
                _movableCards[i]._card.Collider2D.enabled = false;
                _movableCards[i]._card.OriginalPosition =
                    _movableCards[i]._card.VisualCardRef.transform.position; // Store the original position
            }
        }


        private void ReturnToOriginalPosition()
        {
            // Return all the movable cards to their original positions
            for (int i = 0; i < _movableCards.Count; i++)
            {
                VisualCard visualCard = _movableCards[i];
               // visualCard.transform.DOMove(visualCard._card.OriginalPosition, .2f);
                visualCard.transform.DOMove(visualCard._card.OriginalPosition, .2f);
                //visualCard.transform.position = visualCard._card.OriginalPosition; // Return to original position
                visualCard.SetSortingOrder(visualCard._previousSortingOrder); // Reset sorting order
            }
        }

        private void MoveToWaste()
        {
            var previousSlot = _card.Slot;
            var slot = GameManager.Instance.WasteSlot;

            //Remove From Previous Slot
            _card.Slot.RemoveFromSlot(_card, false, false);

            if (!_card.IsFaceUp)
            {
                FlipCard(true); // Flip the card face up
            }

            // Set the card's new slot to the waste slot
            _card.Slot = slot;

            // Move the card to the waste slot position
            //transform.position = slot.transform.position;
            IList<VisualCard> _temp = new List<VisualCard>();

            _temp.Add(this);

            slot.PlaceCard(_temp);
            //CommandInvoker.ExecuteCommand(new PlaceCardCommand(_temp, slot,previousSlot));
        }
    }
}