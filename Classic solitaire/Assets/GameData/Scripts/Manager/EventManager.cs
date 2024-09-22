using System;
using UnityEngine;

namespace TheSyedMateen.ClassicSolitaire
{
    public class EventManager : MonoBehaviour
    {
        public static event Action OnWrongMove;

        public static void InvokeWrongMove()
        {
            OnWrongMove?.Invoke();
        }
    }
}
