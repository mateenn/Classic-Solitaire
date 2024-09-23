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

        private int _previousSortingOrder;

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
        }

        public void SetSortingOrder(int order)
        {
            front.sortingOrder = order;
            back.sortingOrder = order - 1;
            if (order != 1000) _previousSortingOrder = order;
            Helper.Log("Setting Card Order: "+order+" : "+gameObject,gameObject);
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
                            transform.position = _originalPosition;
                            SetSortingOrder(_previousSortingOrder);
                            EventManager.InvokeWrongMove();
                        }
                    }
                    else
                    {
                        // No collider hit, return the card to the original position
                        transform.position = _originalPosition;
                        SetSortingOrder(_previousSortingOrder);
                    }

                    _cardCollider.enabled = true;
                }
            }
        }

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
                SetSortingOrder(_previousSortingOrder);
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
                cardSlot.PlaceCard(_card); // Place the card in the target slot/pile
            }
            else
            {
                // Invalid drop target, return to original position
                transform.position = _originalPosition;
                SetSortingOrder(_previousSortingOrder);
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
            if (hit.collider != null)
            {
                Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
            }
            
            Debug.Log("Raycast moveable: " + _card.IsMoveable() );
            if (_card.Slot != null)
            {
                Debug.Log( " slotType: "+_card.Slot.slotType);
            }
            else
            {
                Debug.Log( " slot is null: ");
            }
            if (!_card.IsMoveable())
            {
                if (_card.Slot == null || _card.Slot.slotType == SlotType.Stack)
                {
                    MoveToWaste();
                }

                return;
            }

            //if (!_card.IsFaceUp) return;
            _originalPosition = transform.position; // Store the original position

            _isDragging = true; // Start dragging
            _cardCollider.enabled = false;
            SetSortingOrder(1000);
            Debug.Log("Mouse Down: " + gameObject);
        }

        private void MoveToWaste()
        {
            var slot = GameManager.Instance.WasteSlot;
            if (!_card.IsFaceUp)
            {
                FlipCard(true); // Flip the card face up
            }

            // Set the card's new slot to the waste slot
            _card.Slot = slot;

            // Move the card to the waste slot position
            //transform.position = slot.transform.position;
            slot.PlaceCard(_card);
        }
    }
}