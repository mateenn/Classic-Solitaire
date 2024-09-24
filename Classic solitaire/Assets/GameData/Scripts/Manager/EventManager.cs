using System;
using UnityEngine;

namespace TheSyedMateen.ClassicSolitaire
{
    public class EventManager : MonoBehaviour
    {
        public static event Action OnWrongMove;
        public static event Action OnMoveComplete;
        public static event Action OnMoveSuccessful;

        public static event Action OnFoundationPileFilled; 
        public static event Action OnLevelComplete; 

        public static void InvokeWrongMove()
        {
            OnWrongMove?.Invoke();
        }

        public static void InvokeMoveComplete()
        {
            OnMoveComplete?.Invoke();
        }
        public static void InvokeMoveSuccessful()
        {
            OnMoveSuccessful?.Invoke();
        }
        public static void InvokeFoundationPileFilled()
        {
            OnFoundationPileFilled?.Invoke();
        }
        public static void InvokeLevelComplete()
        {
            OnLevelComplete?.Invoke();
        }
    }
}