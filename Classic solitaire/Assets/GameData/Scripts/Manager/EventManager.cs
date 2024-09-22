using System;
using UnityEngine;

namespace TheSyedMateen.ClassicSolitaire
{
    public class EventManager : MonoBehaviour
    {
        public static event Action OnWrongMove;
        public static event Action OnMoveComplete;

        public static void InvokeWrongMove()
        {
            OnWrongMove?.Invoke();
        }

        public static void InvokeMoveComplete()
        {
            OnMoveComplete?.Invoke();
        }
    }
}