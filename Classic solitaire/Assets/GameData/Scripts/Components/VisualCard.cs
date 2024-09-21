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

        private void Start()
        {
            SetCard();
        }

        private void SetCard()
        {
            _card = new Card(suit, cardType);
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

                    if (hitCollider != null && hitCollider.TryGetComponent<Slot>(out var slot))
                    {
                        if (slot.CanMoveCard(_card))
                        {
                            Debug.Log("5");
                            slot.PlaceCard(_card); // Place the card in the slot
                        }
                        else
                        {
                            Debug.Log("4");
                            transform.position = _originalPosition; // Return to original position
                        }
                    }
                    else
                    {
                        Debug.Log("3");
                        transform.position = _originalPosition; // Return to original position
                    }
                }
            }
        }
        private void OnMouseDown()
        {
            if(!_card.IsMoveable()) return;
            _originalPosition = transform.position; // Store the original position
            
            _isDragging = true; // Start dragging
            Debug.Log("Mouse Down: "+gameObject);
        }
        
        /*private void OnMouseDown()
        {
            _originalPosition = transform.position; // Store the original position
            _isDragging = true; // Start dragging
            Debug.Log("Mouse Down: "+gameObject);
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

                    if (hitCollider != null && hitCollider.TryGetComponent<Slot>(out var slot))
                    {
                        if (slot.CanMoveCard(_card))
                        {
                            Debug.Log("5");
                            slot.PlaceCard(_card); // Place the card in the slot
                        }
                        else
                        {
                            Debug.Log("4");
                            transform.position = _originalPosition; // Return to original position
                        }
                    }
                    else
                    {
                        Debug.Log("3");
                        transform.position = _originalPosition; // Return to original position
                    }
                }
            }
        }*/
    }
}
