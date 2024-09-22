using System.Collections.Generic;
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

        private Vector3 _originalPosition;
        private bool _isDragging = false;

        private Collider2D _cardCollider;

        private void Start()
        {
            _cardCollider = GetComponent<Collider2D>();
            SetCard();
        }

        private void SetCard()
        {
            _card = new Card(suit, cardType);
            _card.VisualCardRef = this; // Set the reference to the VisualCard
            UpdateCardVisual();
        }

        public Card GetCard()
        {
            return _card;
        }

        public void FlipCard(bool isFaceUp)
        {
            _card.SetFaceUp(isFaceUp); // Assuming your Card class has this method
            UpdateCardVisual();
        }

        private void UpdateCardVisual()
        {
            front.gameObject.SetActive(_card.IsFaceUp);
            back.gameObject.SetActive(!_card.IsFaceUp);
            //front.enabled = _card.IsFaceUp;
            //back.enabled = !_card.IsFaceUp;
        }

        public void SetSortingOrder(int order)
        {
            front.sortingOrder = order;
            back.sortingOrder = order - 1;
        }

        private void Update()
        {
            if (_isDragging)
            {
                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPosition.z = 0; // Ensure the z-coordinate is zero for 2D

                transform.position = mouseWorldPosition; // Move the card with the mouse

                // Check for mouse release
                if (Input.GetMouseButtonUp(0))
                {
                    _isDragging = false;

                    // Check for overlapping colliders (instead of raycasting)
                    Collider2D hitCollider = Physics2D.OverlapPoint(mouseWorldPosition);

                    if (hitCollider != null)
                    {
                        Debug.Log("Collider not null: " + hitCollider);
                        // Try to get the Slot or Pile
                        if (hitCollider.TryGetComponent<Slot>(out Slot slot))
                        {
                            Debug.Log("Collider TryDropOnSlot");
                            // Drop the card on the slot itself (empty tableau slot or foundation)
                            TryDropOnSlot(slot);
                        }
                        else if (hitCollider.TryGetComponent<VisualCard>(out VisualCard visualCard))
                        {
                            Debug.Log("Collider TryDropOnCard");
                            // Drop the card on another card (in tableau piles)
                            TryDropOnCard(visualCard);
                        }
                        else
                        {
                            // No valid drop target, return to original position
                            transform.position = _originalPosition;
                            SetSortingOrder(1);
                        }
                    }
                    else
                    {
                        // No collider hit, return the card to the original position
                        transform.position = _originalPosition;
                        SetSortingOrder(1);
                    }

                    _cardCollider.enabled = true;
                }
            }
        }

        /*private void Update()
        {
            if (_isDragging)
            {
                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPosition.z = 0; // Ensure the z-coordinate is zero for 2D

                // Move all selected cards with the mouse
                foreach (var card in _selectedCards)
                {
                    card.VisualCardRef.transform.position =
                        mouseWorldPosition + _mouseOffset; // Move each selected card
                }

                // Check for mouse release
                if (Input.GetMouseButtonUp(0))
                {
                    _isDragging = false;
                    _cardCollider.enabled = true; // Re-enable collider

                    // Check for a valid drop target
                    Collider2D hitCollider = Physics2D.OverlapPoint(mouseWorldPosition);

                    if (hitCollider != null && hitCollider.TryGetComponent<Slot>(out var slot))
                    {
                        if (IsValidDropTarget(slot))
                        {
                            // If the drop is valid, place all selected cards in the new slot
                            foreach (var card in _selectedCards)
                            {
                                slot.PlaceCard(card);
                            }
                        }
                        else
                        {
                            // Invalid drop, return all selected cards to their original positions
                            foreach (var card in _selectedCards)
                            {
                                card.VisualCardRef.transform.position = card.OriginalPosition;
                            }
                        }
                    }
                    else
                    {
                        // No valid drop target, return all selected cards to their original positions
                        foreach (var card in _selectedCards)
                        {
                            card.VisualCardRef.transform.position = card.OriginalPosition;
                        }
                    }
                }
            }
        }*/


        private void TryDropOnSlot(Slot slot)
        {
            if (IsValidDropTarget(slot))
            {
                // Place the card in the target slot
                slot.PlaceCard(_card);
            }
            else
            {
                // Invalid drop target, return to original position
                transform.position = _originalPosition;
                SetSortingOrder(1);
            }
        }

        private void TryDropOnCard(VisualCard visualCard)
        {
            // Get the slot where the card is located
            //Slot cardSlot = visualCard.GetComponentInParent<Slot>();
            Slot cardSlot = visualCard._card.Slot;
            ;

            Debug.Log("collider cardSlot: " + cardSlot+" isValid: "+ IsValidDropTarget(cardSlot));
            // Check if the drop is valid based on the card in the pile
            if (cardSlot != null && IsValidDropTarget(cardSlot))
            {
                cardSlot.PlaceCard(_card); // Place the card in the target slot/pile
            }
            else
            {
                // Invalid drop target, return to original position
                transform.position = _originalPosition;
                SetSortingOrder(1);
            }
        }

        private bool IsValidDropTarget(Slot targetSlot)
        {
            // Check if the target slot is valid for the card to be placed in
            return targetSlot.CanMoveCard(_card);
        }


        /*private Vector3 _mouseOffset;
        private List<Card> _selectedCards;*/

        /*private void OnMouseDown()
        {
            if (!_card.IsMoveable()) return;

            _originalPosition = transform.position; // Store the original position of this card
            _mouseOffset = transform.position - GetMouseWorldPosition(); // Offset for smooth dragging

            // Gather all movable cards starting from this one
            _selectedCards = _card.GetMovableCards();

            // Store original positions for all selected cards
            foreach (var card in _selectedCards)
            {
                card.OriginalPosition = card.VisualCardRef.transform.position; // Store each card's original position
            }

            _isDragging = true; // Start dragging
            _cardCollider.enabled = false;
            SetSortingOrder(1000); // Bring all selected cards on top
        }*/

        private Vector3 GetMouseWorldPosition()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // Ensure the Z-coordinate is zero for 2D
            return mousePosition;
        }

          private void OnMouseDown()
        {
            Debug.Log("is moveabke: "+_card.IsMoveable());
            if (!_card.IsMoveable()) return;
            //if (!_card.IsFaceUp) return;
            _originalPosition = transform.position; // Store the original position

            _isDragging = true; // Start dragging
            _cardCollider.enabled = false;
            SetSortingOrder(1000);
            Debug.Log("Mouse Down: " + gameObject);
        }
    }
}