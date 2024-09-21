using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TheSyedMateen.ClassicSolitaire
{
    public class dragTest : MonoBehaviour,IDragHandler
    {

        public void OnDrag(PointerEventData eventData)
        {
           Debug.Log("Draging");
        }
    }
}
