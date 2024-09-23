using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheSyedMateen.ClassicSolitaire
{
    public class RefillPile : MonoBehaviour
    {
        //this is only for Stack Slot
        [SerializeField] private Pile stackPile,wastePile;


        private void OnMouseDown()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            Debug.Log("Wasste : "+hit.collider);
            if (hit.collider != null)
            {
                if (stackPile.slot.slotType == SlotType.Stack)
                {
                    Debug.Log("Wasste inside: ");
                    //Refill the pile slot
                    if(wastePile == null) return;
                    var count = wastePile.GetCardCount();

                    Debug.Log("Wasste count: "+wastePile.GetCardCount());
                    for (int i = count- 1; i >= 0; i--)
                    {
                        var visualCard = wastePile.GetAndRemoveCardAtIndex(i).VisualCardRef;
                        var card = visualCard.GetCard();
                        stackPile.AddAndUpdateCardToPile(visualCard);
                        visualCard.FlipCard(false);
                        //updating card slot
                        
                        card.Slot = stackPile.slot;
                        card.Slot.CurrentCard = card;
                    }
                }
            }
            
        }
    }
}
